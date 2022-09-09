using System.Runtime.InteropServices;

namespace GameApp;

public class Game
{
    public Random Random { get; } = new();
    public const float WindowWidth = 1024.0f;
    public const float WindowHeight = 768.0f;

    private bool _isInitialized;
    private bool _shouldShutdown;
    private IntPtr _window;
    private IntPtr _renderer;

    private const long FrameTick = 16 * TimeSpan.TicksPerMillisecond;
    private long _accumulatorTick;
    private DateTime _lastTime;

    private Ship? _ship;
    private bool _isUpdatingActors;
    private readonly List<Actor> _pendingActors = new();
    private readonly List<Actor> _actors = new();
    private readonly Dictionary<string, IntPtr> _name2Texture = new();
    private readonly List<SpriteComponent> _sprites = new();

    public IntPtr GetTexture(string fileName)
    {
        if (_name2Texture.TryGetValue(fileName, out IntPtr texture))
        {
            return texture;
        }

        var surf = SDL_image.IMG_Load(fileName);
        if (surf == IntPtr.Zero)
        {
            SDL.SDL_Log($"Failed to load texture file: {fileName}");
            return IntPtr.Zero;
        }

        texture = SDL.SDL_CreateTextureFromSurface(_renderer, surf);
        SDL.SDL_FreeSurface(surf);
        if (texture == IntPtr.Zero)
        {
            SDL.SDL_Log($"Failed to convert surface to texture for {fileName}");
            return IntPtr.Zero;
        }

        _name2Texture.Add(fileName, texture);
        return texture;
    }

    private void LoadData()
    {
        // Create player's ship
        _ship = new Ship(this) { Position = new Vector2D { X = 100.0f, Y = 384.0f }, Scale = 1.5f };
        // Create actor for the background(this doesn't need a subclass)
        var tmp = new Actor(this) { Position = new Vector2D { X = 512.0f, Y = 384.0f } };
        // Create the "far back" background
        var bg = new BackGroundSpriteComponent(tmp)
        {
            ScreenSize = new Vector2D { X = WindowWidth, Y = WindowHeight },
            ScrollSpeed = -100.0f
        };
        bg.SetBackGroundTextures(new List<IntPtr>
        {
            GetTexture("Assets/Farback01.png"),
            GetTexture("Assets/Farback02.png"),
        });
        // Create the closer background
        bg = new BackGroundSpriteComponent(tmp, 50)
        {
            ScreenSize = new Vector2D { X = WindowWidth, Y = WindowHeight },
            ScrollSpeed = -200.0f
        };
        bg.SetBackGroundTextures(new List<IntPtr>
        {
            GetTexture("Assets/Stars.png"),
            GetTexture("Assets/Stars.png"),
        });

        for (int i = 0; i < 20; i++)
        {
            new Asteroid(this);
        }
    }

    public bool Initialize()
    {
        if (_isInitialized)
        {
            return true;
        }

        int result = SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
        if (result != 0)
        {
            SDL.SDL_Log($"Unable to initialize SDL: {SDL.SDL_GetError()}");
            return false;
        }

        _window = SDL.SDL_CreateWindow("Game Programming in c# (Chapter 1)", 100, 100, (int)WindowWidth,
            (int)WindowHeight, 0);
        if (_window == IntPtr.Zero)
        {
            SDL.SDL_Log($"Failed to create window: {SDL.SDL_GetError()}");
            return false;
        }

        _renderer = SDL.SDL_CreateRenderer(_window, -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        if (_renderer == IntPtr.Zero)
        {
            SDL.SDL_Log($"Failed to create renderer: {SDL.SDL_GetError()}");
            return false;
        }

        result = SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);
        if (result != (int)SDL_image.IMG_InitFlags.IMG_INIT_PNG)
        {
            SDL.SDL_Log($"Unable to initialize SDL_IMG_PNG:{SDL.SDL_GetError()}");
            return false;
        }

        LoadData();

        _isInitialized = true;
        return true;
    }

    private void ProcessInput()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    _shouldShutdown = true;
                    break;
            }
        }

        IntPtr state = SDL.SDL_GetKeyboardState(out int size);
        byte[] keys = new byte[size];
        Marshal.Copy(state, keys, 0, size);
        if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE] == 1)
        {
            _shouldShutdown = true;
        }

        _ship?.ProcessKeyboard(keys);
    }

    private void UpdateGame(float deltaTime)
    {
        _isUpdatingActors = true;
        foreach (Actor actor in _actors)
        {
            actor.Update(deltaTime);
        }
        _isUpdatingActors = false;

        foreach (Actor actor in _pendingActors)
        {
            _actors.Add(actor);
        }
        _pendingActors.Clear();

        var deadActors = new List<Actor>();
        foreach (Actor actor in _actors)
        {
            if (actor.IsDead)
            {
                deadActors.Add(actor);
            }
        }

        foreach (Actor actor in deadActors)
        {
            actor.Remove();
            _actors.Remove(actor);

            if (actor == _ship)
            {
                _ship = null;
            }
        }
    }

    private void GenerateOutput()
    {
        // Clear the back buffer
        SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 255, 255);
        SDL.SDL_RenderClear(_renderer);

        // Draw all sprite components
        foreach (SpriteComponent sprite in _sprites)
        {
            sprite.Draw(_renderer);
        }

        // Swap front back buffer
        SDL.SDL_RenderPresent(_renderer);
    }

    public void RunLoop()
    {
        if (!_isInitialized)
        {
            return;
        }

        _lastTime = DateTime.Now;
        while (!_shouldShutdown)
        {
            DateTime currTime =  DateTime.Now;
            long diffTick = (currTime - _lastTime).Ticks;
            if (diffTick > 50 * TimeSpan.TicksPerMillisecond)
            {
                diffTick = 50 * TimeSpan.TicksPerMillisecond;
            }
            _accumulatorTick += diffTick;
            _lastTime = currTime;

            while (_accumulatorTick >= FrameTick)
            {
                _accumulatorTick -= FrameTick;

                ProcessInput();
                UpdateGame((float)FrameTick / TimeSpan.TicksPerSecond);
                GenerateOutput();
            }

            Thread.Sleep(1);
        }
    }

    private void UnloadData()
    {
        _actors.Clear();
        _pendingActors.Clear();
        _sprites.Clear();

        foreach ((string _, IntPtr value) in _name2Texture)
        {
            SDL.SDL_DestroyTexture(value);
        }
        _name2Texture.Clear();
    }

    public void Shutdown()
    {
        UnloadData();
        SDL_image.IMG_Quit();
        SDL.SDL_DestroyRenderer(_renderer);
        SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }

    public void AddActor(Actor actor)
    {
        if (_isUpdatingActors)
        {
            _pendingActors.Add(actor);
        }
        else
        {
            _actors.Add(actor);
        }
    }

    public void AddSprite(SpriteComponent sprite)
    {
        int drawOrder = sprite.DrawOrder;
        int index = 0;
        for (; index < _sprites.Count; index++)
        {
            if (drawOrder < _sprites[index].DrawOrder )
            {
                break;
            }
        }

        _sprites.Insert(index, sprite);
    }

    public void RemoveSprite(SpriteComponent sprite)
    {
        _sprites.Remove(sprite);
    }
}
