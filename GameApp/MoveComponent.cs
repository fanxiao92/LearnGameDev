namespace GameApp;

public class MoveComponent : Component
{
    public float AngularSpeed { get; set; }
    public float ForwardSpeed { get; set; }

    public MoveComponent(Actor owner, int updateOrder = 10) : base(owner, updateOrder)
    {
    }

    public override void Update(float deltaTime)
    {
        if (!Math.IsNearZero(AngularSpeed))
        {
            Owner.Rotation += AngularSpeed * deltaTime;
        }

        if (!Math.IsNearZero(ForwardSpeed))
        {
            Owner.Position += Owner.Forward * ForwardSpeed * deltaTime;
        }
    }
}
