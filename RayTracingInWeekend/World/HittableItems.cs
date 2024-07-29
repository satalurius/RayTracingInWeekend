using RayTracingInWeekend.Geometry;
using RayTracingInWeekend.World;
using RayTracingInWeekend.World.Interfaces;

namespace RayTracingInWeekend;

public class HittableItems : IHittable
{
    public List<IHittable> Hittables { get; set; }

    public HittableItems(List<IHittable> hittables)
    {
        Hittables = hittables;
    }


    public bool Hit(Ray ray, Interval interval, HitRecord record)
    {
        var hitAnything = false;
        var closestSoFar = interval.Max;

        foreach (var hittable in Hittables)
        {
            if (hittable.Hit(ray, new Interval(interval.Min, closestSoFar), record))
            {
                hitAnything = true;
                closestSoFar = record.T;
            }
        }
        return hitAnything;
    }
}
