namespace RayTracingInWeekend;

public class Interval
{
    public double Min { get; set; } = double.NegativeInfinity;
    public double Max { get; set; } = double.PositiveInfinity;

    public double Size => Max - Min;

    public Interval(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(double x)
        => Min <= x && x <= Max;

    public bool Surrounds(double x)
        => Min < x && x <= Max;

    public void Clear()
    {
        Min = double.NegativeInfinity;
        Max = double.PositiveInfinity;
    }

    public void Inverse()
    {
        Min = double.PositiveInfinity;
        Max = double.NegativeInfinity;
    }

    public double Clamp(double x)
    {
        if (x < Min) return Min;
        if (x > Max) return Max;
        return x;
    }

}
