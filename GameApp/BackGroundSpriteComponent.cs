namespace GameApp;

internal sealed class BackGroundSpriteComponent : SpriteComponent
{
    private class BackGroundTexture
    {
        public IntPtr Texture;

        public Vector2D Offset;
    }

    private readonly List<BackGroundTexture> _backGroundTextures = new();
    public Vector2D ScreenSize { get; init; }
    public float ScrollSpeed { get; init; }

    public BackGroundSpriteComponent(Actor owner, int drawOrder = 10) : base(owner, drawOrder)
    {
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        foreach (BackGroundTexture backGroundTexture in _backGroundTextures)
        {
            backGroundTexture.Offset.X += ScrollSpeed * deltaTime;
            if (backGroundTexture.Offset.X < -ScreenSize.X)
            {
                backGroundTexture.Offset.X  = (_backGroundTextures.Count - 1) * ScreenSize.X - 1;
            }
        }
    }

    public override void Draw(IntPtr renderer)
    {
        foreach (BackGroundTexture backGroundTexture in _backGroundTextures)
        {
            SDL.SDL_Rect rect;
            rect.w = (int)ScreenSize.X;
            rect.h = (int)ScreenSize.Y;
            rect.x = (int)(Owner.Position.X - rect.w / 2.0f + backGroundTexture.Offset.X);
            rect.y = (int)(Owner.Position.Y - rect.h / 2.0f + backGroundTexture.Offset.Y);

            SDL.SDL_RenderCopy(renderer, backGroundTexture.Texture, IntPtr.Zero, ref rect);
        }
    }

    public void SetBackGroundTextures(List<IntPtr> textures)
    {
        for (int i = 0; i < textures.Count; i++)
        {
            _backGroundTextures.Add(new BackGroundTexture
            {
                Offset = new Vector2D {X = i * ScreenSize.X, Y = 0},
                Texture = textures[i],
            });
        }
    }
}
