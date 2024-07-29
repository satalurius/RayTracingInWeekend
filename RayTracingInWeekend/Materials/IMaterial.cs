using RayTracingInWeekend.Geometry;
using RayTracingInWeekend.World;

namespace RayTracingInWeekend.Materials;

public interface IMaterial
{
    (bool result, Color attenuation, Ray scattered) Scatter(Ray rayIn, HitRecord rec, Color attenuation, Ray scattered);
}
