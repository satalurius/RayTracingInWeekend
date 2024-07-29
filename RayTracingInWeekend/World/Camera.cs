using System.Text;
using RayTracingInWeekend.Geometry;

namespace RayTracingInWeekend.World;

public class Camera
{
    public double AspectRatio { get; set; } = 16.0 / 9.0;
    public int ImageWidth { get; set; } = 100;
    public int SamplesPerPixel { get; set; } = 10;      // Count of random samples for each pixel
    public double ReflectionCoefficient { get; set; } = 0.5;
    public int MaxDepth { get; set; } = 10;
    public double VFow { get; set; } = 90;       // Vertical view angle (field of view)
    public Point3 LookFrom { get; set; } = new Point3(0, 0, 0);     // Point camera looking from
    public Point3 LookAt { get; set; } = new Point3(0, 0, -1);      // Point camera looking at
    public Point3 VUP { get; set; } = new Point3(0, 1, 0);          // Camera relative up direction
    public double DefocusAngle { get; set; } = 0;                   // Variation angle of rays through each pixel.
    public double FocusDist { get; set; } = 10;                     // Distance from camera lookfrom point to plane of perfect focus.

    private int imageHeight;
    private Point3 center = new(0, 0, 0);
    private Point3 pixel00Location = new(0, 0, 0);
    private Vec3 pixelDeltaU = new(0, 0, 0);               // Offset to pixel to the right
    private Vec3 pixelDeltaV = new(0, 0, 0);               // Offset to pixel below
    private double pixelSamplesScale;
    private Vec3 u = 
        new(0, 0, 0), 
        v = new (0, 0, 0), 
        w = new(0, 0, 0);                                  // Camera frame basis vectors.
    private Vec3 defocusDiskU = new(0, 0, 0), defocusDiskV = new(0, 0, 0);


    public void Render(HittableItems world)
    {
        Initialize();

        var renderText = $"P3\n{ImageWidth} {imageHeight} \n255\n";
        var sb = new StringBuilder(renderText);

        for (var i = 0; i < imageHeight; i++)
        {

            Console.WriteLine($"\rScanlines remaining {imageHeight - i}");
            for (var j = 0; j < ImageWidth; j++)
            {
                var pixelColor = new Color(0, 0, 0);

                for (var sample = 0; sample < SamplesPerPixel; sample++)
                {
                    var ray = GetRay(j, i);
                    var vecColor = RayColor(ray, MaxDepth, world) + pixelColor;
                    pixelColor = new Color(vecColor.X, vecColor.Y, vecColor.Z);
                }

                var scaledPixel = pixelSamplesScale * pixelColor;
                var finalColor = new Color(scaledPixel.X, scaledPixel.Y, scaledPixel.Z);
                var colorText = finalColor.GetColorString();

                sb.Append(colorText);
            }
        }
        File.WriteAllText("output.ppm", sb.ToString());
    }

    private void Initialize()
    {
        // Calc image height, and ensure that its at list 1
        imageHeight = (int)(ImageWidth / AspectRatio);
        imageHeight = imageHeight < 1 ? 1 : imageHeight;

        pixelSamplesScale = 1.0 / SamplesPerPixel;

        center = LookFrom;

        // Determine viewport dimensions.

        //var focalLength = (LookFrom - LookAt).Length;

        var theta = double.DegreesToRadians(VFow);
        var h = Math.Tan(theta / 2);

        var viewportHeight = 2 * h * FocusDist;
        var viewportWidth = viewportHeight * ((double)ImageWidth / imageHeight);

        // Calc u, v, w unit basis vectors for the camera coordinates frame.
        w = Vec3.UnitVector(LookFrom - LookAt);
        var test = Vec3.Cross(VUP, w);
        u = Vec3.UnitVector(test);
        v = Vec3.Cross(w, u);

        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        var viewportU = viewportWidth * u;                  // Vector across viewport horizontal edge
        var viewportV = viewportHeight * v.ReversePoints(); // Vector down viewport vertical edge

        // var viewportU = new Vec3(viewportWidth, 0, 0);
        // var viewportV = new Vec3(0, -viewportHeight, 0);

        // Calculate the horizontal and vertical delta vectors from pixel to pixel.
        pixelDeltaU = viewportU / ImageWidth;
        pixelDeltaV = viewportV / imageHeight;

        // Calc the location of the upper left pixel.

        //var viewPortUpperLeft = Center - new Vec3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
        //var viewPortUpperLeft = Center - (focalLength * w) - viewportU / 2 - viewportV / 2;
        var viewPortUpperLeft = center - (FocusDist * w) - viewportU / 2 - viewportV / 2;
        var vectorPixel00 = viewPortUpperLeft + 0.5 * (pixelDeltaU + pixelDeltaV);
        pixel00Location = new Point3(vectorPixel00.X, vectorPixel00.Y, vectorPixel00.Z);

        // Calc the camera defocus basis vectors.
        var defocusRadius = FocusDist * Math.Tan(double.DegreesToRadians(DefocusAngle / 2));
        defocusDiskU = u * defocusRadius;
        defocusDiskV = v * defocusRadius;
    }

    private Color RayColor(Ray ray, int depth, HittableItems world)
    {
        // If we've exceeded the ray bounce limit, no more light is gathered.
        if (depth <= 0)
            return new Color(0, 0, 0);

        Vec3? result;
        HitRecord rec = new();
        if (world.Hit(ray, new Interval(0.001, double.PositiveInfinity), rec))
        {
            //var direction = Vec3.RandomOnHemisphere(rec.Normal);
            // Ламбертово отражение
            //var direction = rec.Normal + Vec3.RandomUnitVector();
            var scattered = new Ray();
            var attenuation = new Color(0, 0, 0);

            var scatter = rec?.Material?.Scatter(ray, rec, attenuation, scattered);

            if (scatter != null && scatter.Value.result)
            {
                result = scatter.Value.attenuation * RayColor(scatter.Value.scattered, depth - 1, world);
                return new Color(result.X, result.Y, result.Z);
            }

            //result = ReflectionCoefficient * RayColor(new Ray(rec.Point, direction), depth - 1, world);

            return new Color(0, 0, 0);
        }

        Vec3 unitDirection = ray.Direction ?? throw new ArgumentNullException(nameof(ray));
        var a = 0.5 * (unitDirection.Y + 1.0);
        result = (1.0 - a) * new Color(1.0, 1.0, 1.0) + a * new Color(0.5, 0.7, 1.0);
        return new Color(result.E[0], result.E[1], result.E[2]);
    }

    private Ray GetRay(int i, int j)
    {
        // Construct a camera ray originating from the defocus disk and directed at a randomly
        // sampled point around the pixel location i, j

        var offset = SampleSquare();
        var pixelSample = pixel00Location + ((i + offset.X) * pixelDeltaU)
                                          + ((j + offset.Y) * pixelDeltaV);

        var rayOrigin = (DefocusAngle <= 0) ? center : DefocusDiskSample();
        var rayDirection = pixelSample - rayOrigin;

        return new Ray(rayOrigin, rayDirection);
    }

    // Returns the vector to a random point in the [-.5,-.5]-[+.5,+.5] unit square
    private Vec3 SampleSquare()
        => new(new Random().NextDouble() - 0.5, new Random().NextDouble() - 0.5, 0);

    private Point3 DefocusDiskSample()
    {
        // Returns a random point in the camera defocus disk.

        var p = Vec3.RandmonInUnitDisk();
        var defocusInVectorMode = center + (p.X * defocusDiskU) + (p.Y * defocusDiskV);
        return new Point3(defocusInVectorMode.X, defocusInVectorMode.Y, defocusInVectorMode.Z);
    }
}
