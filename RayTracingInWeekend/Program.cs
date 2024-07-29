using RayTracingInWeekend.Geometry;
using RayTracingInWeekend.Materials;
using RayTracingInWeekend.World;
using RayTracingInWeekend.World.Interfaces;

namespace RayTracingInWeekend;

internal class Program
{
    static void Main(string[] args)
    {
        // image
        var world = CreateMultipleItemsWorld();


        var camera = new Camera
        {
            AspectRatio = 16.0 / 9.0,
            ImageWidth = 1200,
            SamplesPerPixel = 10,
            MaxDepth = 50,
            VFow = 20,
            LookFrom = new Point3(13, 2, 3),
            LookAt = new Point3(0, 0, 0),
            VUP = new Point3(0, 1, 0),
            DefocusAngle = 0.6,
            FocusDist = 10
        };

        camera.Render(world);

        Console.WriteLine("done");
    }

    private static HittableItems CreateMultipleItemsWorld()
    {
        var hittables = new List<IHittable>();

        var groundMaterial = new Lambertian(new Color(0.5, 0.5, 0.5));
        hittables.Add(new Sphere(new Point3(0, -1000, 0), 1000, groundMaterial));

        for (var a = -6; a < 6; a++)
        {
            for (var b = -6; b < 6; b++)
            {
                var chooseMat = Utils.GetRandom();
                var center = new Point3(a + 0.9 * Utils.GetRandom(), 0.2, b + 0.9 * Utils.GetRandom());

                if (!((center - new Point3(4, 0.2, 0)).Length > 0.9))
                {
                    continue;
                }

                IMaterial material;

                if (chooseMat < 0.95) {
                    // Metall
                    var albedo = Vec3.Random(0.5, 1);
                    var fuzz = Utils.GetRandom(0, 0.5);
                    material = new Metal(new Color(albedo.X, albedo.Y, albedo.Z), fuzz);
                    hittables.Add(new Sphere(center, 0.2, material));

                }
                if (chooseMat < 0.8) {
                    // Diffuse
                    var albedo = Vec3.Random() * Vec3.Random();
                    material = new Lambertian(new Color(albedo.X, albedo.Y, albedo.Z));
                    hittables.Add(new Sphere(center, 0.2, material));
                }
                else 
                {
                    // Glass
                    material = new Dielectric(1.5);
                    hittables.Add(new Sphere(center, 0.2, material));
                }
            }
        }

        var dielectric = new Dielectric(1.5);
        hittables.Add(new Sphere(new Point3(0, 1, 0), 1.0, dielectric));

        var lambertian = new Lambertian(new Color(0.4, 0.2, 0.1));
        hittables.Add(new Sphere(new Point3(-4, 1, 0), 1.0, lambertian));

        var metal = new Metal(new Color(0.7, 0.6, 0.5), 0);
        hittables.Add(new Sphere(new Point3(4, 1, 0), 1.0, metal));

        return new HittableItems(hittables);
    }
}
