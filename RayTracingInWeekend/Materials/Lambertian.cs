using RayTracingInWeekend.Geometry;
using RayTracingInWeekend.World;

namespace RayTracingInWeekend.Materials;

public class Lambertian : IMaterial
{
    public Color Albedo { get; set; }

    public Lambertian(Color albedo)
    {
        Albedo = albedo;
    }

    public (bool result, Color attenuation, Ray scattered) Scatter(Ray rayIn, HitRecord rec, Color attenuation, Ray scattered)
    {
        var scatterDirection = rec.Normal + Vec3.RandomUnitVector();

        if (scatterDirection.NearZero())
        {
            scatterDirection = rec.Normal;
        }

        scattered = new Ray(rec.Point, scatterDirection);
        attenuation = Albedo;
        return (true, attenuation, scattered);
    }
}
