namespace GameApp;

public struct Vector2D
{
    public static Vector2D Zero { get; } = new() { X = 0.0f, Y = 0.0f, };

    public float X;

    public float Y;

    public static Vector2D operator *(Vector2D v, float f)
    {
        return new Vector2D { X = v.X * f, Y = v.Y * f };
    }

    public static Vector2D operator *(Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D { X = lhs.X * rhs.X, Y = lhs.Y * rhs.Y };
    }

    public static Vector2D operator +(Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D { X = lhs.X + rhs.X, Y = lhs.Y + rhs.Y };
    }

    public static Vector2D operator -(Vector2D lhs, Vector2D rhs)
    {
        return new Vector2D { X = lhs.X - rhs.X, Y = lhs.Y - rhs.Y };
    }
}
