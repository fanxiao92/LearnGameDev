namespace GameApp
{
    internal sealed class Ship : Actor
    {
        private float _laserCooldown;

        public Ship(Game game) : base(game)
        {
            var _1 = new SpriteComponent(this) { Texture = game.GetTexture("assets/Ship.png") };

            var _2 = new InputComponent(this)
            {
                ForwardKey = SDL.SDL_Scancode.SDL_SCANCODE_W,
                BackKey = SDL.SDL_Scancode.SDL_SCANCODE_S,
                ClockwiseKey = SDL.SDL_Scancode.SDL_SCANCODE_A,
                CounterClockwiseKey = SDL.SDL_Scancode.SDL_SCANCODE_D,
                MaxForwardSpeed = 300.0f,
                MaxAngularSpeed = (float)System.Math.PI * 2,
            };
        }

        protected override void ActorInput(byte[] keyState)
        {
            if (keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE] == 1 && _laserCooldown <= 0.0f)
            {
                var _ = new Laser(Game)
                {
                    Position = Position,
                    Rotation = Rotation,
                };

                _laserCooldown = 0.5f;
            }
        }

        protected override void UpdateActor(float deltaTime) => _laserCooldown -= deltaTime;
    }

}

