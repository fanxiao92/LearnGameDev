namespace GameApp;

public sealed class Ship : Actor
{
    private float _rightSpeed;
    private float _downSpeed;

    public Ship(Game game) : base(game)
    {
        var asc = new AnimationSpriteComponent(this);
        asc.SetAnimationTextures(new List<IntPtr>
        {
            game.GetTexture("Assets/Ship01.png"),
            game.GetTexture("Assets/Ship02.png"),
            game.GetTexture("Assets/Ship03.png"),
            game.GetTexture("Assets/Ship04.png"),
        });
    }

    protected override void UpdateActor(float deltaTime)
    {
        base.UpdateActor(deltaTime);

        Vector2D position = Position;
        position.X += _rightSpeed * deltaTime;
        position.Y += _downSpeed * deltaTime;

        if (position.X < 25.0f)
        {
            position.X = 25.0f;
        }
        else if (position.X > 500.0f)
        {
            position.X = 500.0f;
        }

        if (position.Y < 25.0f)
        {
            position.Y = 25.0f;
        }
        else if (position.Y >= 743.0f)
        {
            position.Y = 743.0f;
        }

        Position = position;
    }

    public void ProcessKeyboard(byte[] state)
    {
        _rightSpeed = 0.0f;
        _downSpeed = 0.0f;

        if (state[(int)SDL.SDL_Scancode.SDL_SCANCODE_D] == 1)
        {
            _rightSpeed += 250.0f;
        }
        if (state[(int)SDL.SDL_Scancode.SDL_SCANCODE_A] == 1)
        {
            _rightSpeed -= 250.0f;
        }

        if (state[(int)SDL.SDL_Scancode.SDL_SCANCODE_S] == 1)
        {
            _downSpeed += 300.0f;
        }
        if (state[(int)SDL.SDL_Scancode.SDL_SCANCODE_W] == 1)
        {
            _downSpeed -= 300.0f;
        }

        if (state[(int)SDL.SDL_Scancode.SDL_SCANCODE_X] == 1)
        {
            _state = State.Dead;
        }
    }
}
