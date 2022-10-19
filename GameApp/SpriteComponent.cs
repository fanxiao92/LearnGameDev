namespace GameApp;

internal class SpriteComponent : Component
{
    public int DrawOrder { get; }

    private int _textureWidth;
    private int _textureHeight;
    private IntPtr _texture = IntPtr.Zero;
    public IntPtr Texture
    {
        set
        {
            _texture = value;
            SDL.SDL_QueryTexture(_texture, out uint _, out int _, out _textureWidth, out _textureHeight);
        }
    }

    public SpriteComponent(Actor owner, int drawOrder = 100) : base(owner)
    {
        DrawOrder = drawOrder;

        owner.Game.AddSprite(this);
    }

    public virtual void Draw(IntPtr renderer)
    {
        if (_texture == IntPtr.Zero)
        {
            return;
        }

        SDL.SDL_Rect rect;
        rect.w = (int)(_textureWidth * Owner.Scale);
        rect.h = (int)(_textureHeight * Owner.Scale);
        rect.x = (int)(Owner.Position.X - rect.w / 2.0f);
        rect.y = (int)(Owner.Position.Y - rect.h / 2.0f);

        SDL.SDL_RenderCopyEx(renderer, _texture, IntPtr.Zero, ref rect, Math.ToDegrees(Owner.Rotation), IntPtr.Zero,
            SDL.SDL_RendererFlip.SDL_FLIP_NONE);
    }

    public void SetTexture(IntPtr texture)
    {
    }

    public override void Remove()
    {
        Owner.Game.RemoveSprite(this);
    }
}
