namespace RayTracingInWeekend.Geometry;
public class Vec3
{
    public double[] E { get; set; } = [0, 0, 0];
    public double X => E[0];
    public double Y => E[1];
    public double Z => E[2];
    public double Length => Math.Sqrt(LengthSquared());

    public Vec3(double e1, double e2, double e3)
    {
        E = [e1, e2, e3];
    }

    public Vec3 ReversePoints()
        => new(-E[0], -E[1], -E[2]);
    public static Vec3 ReversePoints(Vec3 v)
    => new(-v.E[0], -v.E[1], -v.E[2]);
    public double GetPoint(int index) => E[index];

    public static Vec3 operator +(Vec3 v1, Vec3 v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

    public static Vec3 operator -(Vec3 v1, Vec3 v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    public static Vec3 operator *(Vec3 v1, Vec3 v2) => new(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);

    public static Vec3 operator *(Vec3 v, double t) => new(v.X * t, v.Y * t, v.Z * t);

    public static Vec3 operator *(double t, Vec3 v) => new(v.X * t, v.Y * t, v.Z * t);

    //public static Vec3 operator /(Vec3 v, double t) => new(v.X / t, v.Y / t, v.Z / t);
    public static Vec3 operator /(Vec3 v, double t) => new Vec3(v.X * (1 / t), v.Y * (1 / t), v.Z * (1 / t));


    public static double Dot(Vec3 v1, Vec3 v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    public static Vec3 Cross(Vec3 left, Vec3 right)
        => new(left.E[1] * right.E[2] - left.E[2] * right.E[1],
            left.E[2] * right.E[0] - left.E[0] * right.E[2],
            left.E[0] * right.E[1] - left.E[1] * right.E[0]
            );


    public double LengthSquared()
        => E[0] * E[0] + E[1] * E[1] + E[2] * E[2];

    public static Vec3 Random()

        => new(Utils.GetRandom(), Utils.GetRandom(), Utils.GetRandom());

    public static Vec3 Random(double min, double max)
        => new(Utils.GetRandom(min, max), Utils.GetRandom(min, max), Utils.GetRandom(min, max));

    public static Vec3 UnitVector(Vec3 v)
        => v / v.Length;

    public static Vec3 RandomInUnitSphere()
    {
        while (true)
        {
            var p = Random(-1, 1);
            if (p.LengthSquared() < 1)
                return p;
        }
    }

    public static Vec3 RandomUnitVector()
        => UnitVector(RandomInUnitSphere());

    public static Vec3 RandomOnHemisphere(Vec3 normal)
    {
        var onUnitOnSphere = RandomUnitVector();

        if (Dot(onUnitOnSphere, normal) > 0.0)
        {        // the same
            return onUnitOnSphere;
        }
        return ReversePoints(onUnitOnSphere);
    }

    public bool NearZero()
    {
        // Return true if vector is close to zero in all dimensions.
        var s = 1e-8;
        return (Math.Abs(E[0]) < s) && (Math.Abs(E[1]) < s) && (Math.Abs(E[2]) < s);
    }

    public static Vec3 Reflect(Vec3 v, Vec3 n)
        => v - 2 * Dot(v, n) * n;

    public static Vec3 Refract(Vec3 uv, Vec3 n, double etailOverEtat)
    {
        var cosTheta = Math.Min(Dot(uv.ReversePoints(), n), 1.0);
        var rOutPerp = etailOverEtat * (uv + cosTheta * n);
        var rOutParallel = -Math.Sqrt(Math.Abs(1.0 - rOutPerp.LengthSquared())) * n;
        return rOutPerp + rOutParallel;
    }

    public static Vec3 RandmonInUnitDisk() {
        while (true) {
            var p = new Vec3(Utils.GetRandom(-1, 1), Utils.GetRandom(-1, 1), 0);
            if (p.LengthSquared() < 1) {
                return p;
            }
        }
    }



}
