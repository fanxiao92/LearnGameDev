namespace GameApp;

public static class Math
{
    public static float ToRadians(float degrees)
    {
        return (float)(degrees * System.Math.PI / 180.0f);
    }
    public static float ToDegrees(float radians)
    {
        return (float)(radians * 180.0f / System.Math.PI);
    }

    public static bool IsNearZero(float value, float epsilon = 0.001f)
    {
        if (System.Math.Abs(value) <= epsilon)
        {
            return true;
        }

        return false;
    }
}
