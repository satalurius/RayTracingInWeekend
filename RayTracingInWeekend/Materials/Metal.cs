using RayTracingInWeekend.Geometry;
using RayTracingInWeekend.World;

namespace RayTracingInWeekend.Materials;

public class Metal : IMaterial
{
    public Color Albedo { get; set; }
    private double fuzz;
    private double Fuzz { get => fuzz; set => fuzz = value < 1 ? value : 1; }

    public Metal(Color albedo, double fuzz)
    {
        Albedo = albedo;
        Fuzz = fuzz;
    }


    public (bool result, Color attenuation, Ray scattered) Scatter(Ray rayIn, HitRecord rec, Color attenuation, Ray scattered)
    {
        var reflected = Vec3.Reflect(rayIn.Direction, rec.Normal);
        reflected = Vec3.UnitVector(reflected) + (Fuzz * Vec3.RandomUnitVector());

        scattered = new Ray(rec.Point, reflected);
        attenuation = Albedo;
        var result = Vec3.Dot(scattered.Direction, rec.Normal) > 0;

        return (result, attenuation, scattered);
    }
}
