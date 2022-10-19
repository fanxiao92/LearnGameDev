namespace GameApp
{
    internal class Asteroid : Actor
    {
        public CircleComponent Circle { get; }

        public Asteroid(Game game) : base(game)
        {
            Position = game.Random.NextVector(Vector2D.Zero, new Vector2D { X = Game.WindowWidth, Y = Game.WindowHeight });
            Rotation = game.Random.NextSingle() * (float)System.Math.PI * 2;

            var _1 = new SpriteComponent(this) { Texture = game.GetTexture("assets/Asteroid.png") };
            var _2 = new MoveComponent(this) { ForwardSpeed = 150.0f };
            Circle = new CircleComponent(this) { Raidus = 40.0f };
        }
    }
}

