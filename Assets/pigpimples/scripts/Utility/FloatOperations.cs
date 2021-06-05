using UnityEngine;

public static class FloatOperations
{
    public static float Roundf(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}