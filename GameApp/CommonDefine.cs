namespace GameApp
{
    internal class Vector2D
    {
        public static Vector2D Zero { get; } = new() { X = 0.0f, Y = 0.0f, };

        public float X { get; set; }

        public float Y { get; set; }

        public float LengthSq => X * X + Y * Y;

        public static Vector2D operator *(Vector2D v, float f) => new() { X = v.X * f, Y = v.Y * f };

        public static Vector2D operator *(Vector2D lhs, Vector2D rhs) => new() { X = lhs.X * rhs.X, Y = lhs.Y * rhs.Y };

        public static Vector2D operator +(Vector2D lhs, Vector2D rhs) => new() { X = lhs.X + rhs.X, Y = lhs.Y + rhs.Y };

        public static Vector2D operator -(Vector2D lhs, Vector2D rhs) => new() { X = lhs.X - rhs.X, Y = lhs.Y - rhs.Y };
    }
}

