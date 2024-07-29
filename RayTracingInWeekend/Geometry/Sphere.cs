using RayTracingInWeekend.Materials;
using RayTracingInWeekend.World;
using RayTracingInWeekend.World.Interfaces;

namespace RayTracingInWeekend.Geometry;

public class Sphere : IHittable
{
    public Point3 Center { get; } = new(0, 0, 0);
    public double Radius { get; }
    public IMaterial Material { get; set; }

    public Sphere(Point3 center, double radius, IMaterial material)
    {
        Center = center;
        Radius = radius;
        Material = material;
    }

    public bool Hit(Ray ray, Interval interval, HitRecord record)
    {
        var oc = Center - ray.Orig;
        var a = ray.Direction.LengthSquared();
        var h = Vec3.Dot(ray.Direction, oc);
        var c = oc.LengthSquared() - Math.Pow(Radius, 2);
        var discriminant = h * h - a * c;

        if (discriminant < 0)
        {
            return false;
        }

        var discriminantSqrt = Math.Sqrt(discriminant);

        // Find the nearest root that lies in the acceptable range
        var root = (h - discriminantSqrt) / a;
        if (!interval.Surrounds(root))
        {
            root = (h + discriminantSqrt) / a;
            if (!interval.Surrounds(root))
            {
                return false;
            }
        }

        record.T = root;
        record.Point = ray.CalcAt(record.T);
        var outwardNormal = (record.Point - Center) / Radius;
        record.SetFaceNormal(ray, outwardNormal);
        record.Material = Material;

        return true;
    }
}
