namespace RayTracingInWeekend.Geometry;
public class Ray
{
    public Point3 Orig { get; } = new(0, 0, 0);
    public Vec3 Direction { get; } = new(0, 0, 0);
    
    public Ray()
    {
    }

    public Ray(Point3 orig, Vec3 direction)
    {
        Orig = orig;
        Direction = direction;
    }

    public Point3 CalcAt(double t)
    {
        var calc = Orig + t * Direction;
        return new Point3(calc.E[0], calc.E[1], calc.E[2]);
    }

}
