namespace RayTracingInWeekend;

public static class Utils
{
    public static double GetRandom() 
        => new Random().NextDouble();
    public static double GetRandom(double minimum, double maximum)
{
    Random random = new Random();
    return random.NextDouble() * (maximum - minimum) + minimum;
}
}
