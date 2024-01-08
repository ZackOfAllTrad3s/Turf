using System;

[Serializable]
public struct MinMax
{
    public float min;
    public float max;

    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public bool Contains(float value)
    {
        return min <= value && value <= max;
    }

    private static Random random = new Random();
    public float Random()
    {
        return (float)(min + random.NextDouble() * (max - min));
    }
}

public static class MathEx
{
    private static readonly Random random = new Random();
    public static float GetRandomFloat(float minValue, float maxValue)
    {
        return (float)(random.NextDouble() * (maxValue - minValue + 1) + minValue);
    }

    public static float MapToRange(this float dvalue, float dmin, float dmax, float tmin, float tmax)
    {
        //tmin + (DVALUE - DMIN)(tmax - tmin) / (DMAX - DMIN)
        return tmin + (dvalue - dmin) * (tmax - tmin) / (dmax - dmin);
    }
}
