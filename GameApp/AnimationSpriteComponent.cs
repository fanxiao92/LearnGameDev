namespace GameApp;

internal sealed class AnimationSpriteComponent : SpriteComponent
{
    private float _currFrame;
    private const float AnimationFps = 24.0f;
    private List<IntPtr> _animationTextures = new();

    public AnimationSpriteComponent(Actor owner, int drawOrder = 100) : base(owner, drawOrder)
    {
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (!_animationTextures.Any())
        {
            return;
        }

        _currFrame += AnimationFps * deltaTime;
        while (_currFrame >= _animationTextures.Count)
        {
            _currFrame -= _animationTextures.Count;
        }
        SetTexture(_animationTextures[(int)_currFrame]);
    }

    public void SetAnimationTextures(List<IntPtr> textures)
    {
        _animationTextures = textures;
        if (!_animationTextures.Any())
        {
            return;
        }

        _currFrame = 0.0f;
        Texture = textures[0];
    }
}
