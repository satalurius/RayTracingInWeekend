using RayTracingInWeekend.Geometry;
using RayTracingInWeekend.World;

namespace RayTracingInWeekend.Materials;

public class Dielectric : IMaterial
{
    // Refractive index in vacuum or air, or the ratio of the material's refractive index over
    // the refractive index of the enclosing media
    private double refractionIndex;

    public Dielectric(double refractionIndex)
    {
        this.refractionIndex = refractionIndex;
    }
    public (bool result, Color attenuation, Ray scattered) Scatter(Ray rayIn, HitRecord rec, Color attenuation, Ray scattered)
    {
        attenuation = new Color(1.0, 1.0, 1.0);
        var ri = rec.FrontFace ? (1.0 / refractionIndex) : refractionIndex;

        var unitDirection = Vec3.UnitVector(rayIn.Direction);
        var cosTheta = Math.Min(Vec3.Dot(unitDirection.ReversePoints(), rec.Normal), 1.0);
        var sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

        var canRefract = ri * sinTheta < 1.0;
        Vec3 direction;

        if (canRefract || !(Reflectance(cosTheta, ri) > new Random().NextDouble())) {
            direction = Vec3.Refract(unitDirection, rec.Normal, ri);
        }
        else {
            direction = Vec3.Reflect(unitDirection, rec.Normal);
        }

        scattered = new Ray(rec.Point, direction);

        return (true, attenuation, scattered);
    }


    private double Reflectance(double cosine, double refractionIndex)
    {
        // Use Schlick's approximation for reflectance.
        var r0 = (1 - refractionIndex) / (1 + refractionIndex);
        r0 *= r0;
        return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
    }
}
