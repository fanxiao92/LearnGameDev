namespace GameApp
{
    internal class Laser : Actor
    {
        private float _deathTimer = 1.0f;
        private readonly CircleComponent _circle;

        public Laser(Game game) : base(game)
        {
            var _1 = new SpriteComponent(this) { Texture = game.GetTexture("assets/Laser.png") };
            var _2 = new MoveComponent(this) { ForwardSpeed = 800.0f };
            _circle = new CircleComponent(this) { Raidus = 11.0f };
        }

        protected override void UpdateActor(float deltaTime)
        {
            _deathTimer -= deltaTime;
            if (_deathTimer <= 0.0f)
            {
                State_ = State.Dead;
                return;
            }

            foreach (Asteroid asteroid in Game.GetAsteroids())
            {
                if (asteroid.Circle.Intersect(_circle))
                {
                    State_ = State.Dead;
                    asteroid.State_ = State.Dead;
                    break;
                }
            }
        }
    }
}
