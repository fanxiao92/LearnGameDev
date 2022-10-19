namespace GameApp
{
    internal class MoveComponent : Component
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
                Vector2D nextPosition = Owner.Position + (Owner.Forward * ForwardSpeed * deltaTime);
                if (nextPosition.X < 0.0f)
                {
                    nextPosition.X = 1022.0f;
                }
                else if (nextPosition.X > 1024.0f)
                {
                    nextPosition.X = 2.0f;
                }

                if (nextPosition.Y < 0.0f)
                {
                    nextPosition.Y = 766.0f;
                }
                else if (nextPosition.Y > 768.0f)
                {
                    nextPosition.Y = 2.0f;
                }
                Owner.Position = nextPosition;
            }
        }
    }
}

