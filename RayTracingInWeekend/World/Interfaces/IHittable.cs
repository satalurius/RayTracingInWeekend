using RayTracingInWeekend.Geometry;

namespace RayTracingInWeekend.World.Interfaces;

public interface IHittable
{
    bool Hit(Ray ray, Interval interval, HitRecord record);
}
