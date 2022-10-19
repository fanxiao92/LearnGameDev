namespace GameApp
{
    internal class InputComponent : MoveComponent
    {
        public InputComponent(Actor owner) : base(owner)
        {
        }

        public override void ProcessInput(byte[] keyState)
        {
            float nextForwardSpeed = 0.0f;
            if (keyState[(int)ForwardKey] == 1)
            {
                nextForwardSpeed += MaxForwardSpeed;
            }
            if (keyState[(int)BackKey] == 1)
            {
                nextForwardSpeed -= MaxForwardSpeed;
            }
            ForwardSpeed = nextForwardSpeed;

            float nextAngularSpeed = 0.0f;
            if (keyState[(int)ClockwiseKey] == 1)
            {
                nextAngularSpeed += MaxAngularSpeed;
            }
            if (keyState[(int)CounterClockwiseKey] == 1)
            {
                nextAngularSpeed -= MaxAngularSpeed;
            }
            AngularSpeed = nextAngularSpeed;
        }

        public float MaxForwardSpeed { get; set; }
        public float MaxAngularSpeed { get; set; }

        public SDL.SDL_Scancode ForwardKey { get; set; }
        public SDL.SDL_Scancode BackKey { get; set; }

        public SDL.SDL_Scancode ClockwiseKey { get; set; }
        public SDL.SDL_Scancode CounterClockwiseKey { get; set; }
    }
}

