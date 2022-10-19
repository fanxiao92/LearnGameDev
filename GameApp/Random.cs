namespace GameApp;

internal sealed class Random : System.Random
{
    public Vector2D NextVector(Vector2D minValue, Vector2D maxValue)
    {
        Vector2D r = new() { X = NextSingle(), Y = NextSingle() };
        return minValue + (maxValue - minValue) * r;
    }
}
