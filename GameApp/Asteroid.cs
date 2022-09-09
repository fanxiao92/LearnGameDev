namespace GameApp;

public class Asteroid : Actor
{
    public Asteroid(Game game) : base(game)
    {
        Position = game.Random.NextVector(Vector2D.Zero, new Vector2D { X = Game.WindowWidth, Y = Game.WindowHeight });
        Rotation = game.Random.NextSingle() * (float)System.Math.PI * 2;
        new SpriteComponent(this) { Texture = game.GetTexture("assets/Asteroid.png") };
        new MoveComponent(this) { ForwardSpeed = 150.0f };
    }
}
