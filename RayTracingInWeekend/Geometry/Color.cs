namespace RayTracingInWeekend.Geometry;
public class Color : Vec3
{
    public Color(double e1, double e2, double e3) : base(e1, e2, e3)
    {
    }

    public double LinearToGamma(double linear)
    {
        if (linear < 0)
            return 0;
        
        return Math.Sqrt(linear);
    }
    public string GetColorString()
    {
        var r = LinearToGamma(X);
        var g = LinearToGamma(Y);
        var b = LinearToGamma(Z);

        // [0,1] to [0, 255]
        var intencity = new Interval(0.000, 0.999);

        int rByte = (int)(256 * intencity.Clamp(r));
        int gByte = (int)(256 * intencity.Clamp(g));
        int bByte = (int)(256 * intencity.Clamp(b));

        return $"{rByte} {gByte} {bByte} \n";
    }
    public static string GetColorString(Color color)
    {
        var r = color.X;
        var g = color.Y;
        var b = color.Z;

        // [0,1] to [0, 255]
        var rByte = (int)(r * 255);
        var gByte = (int)(g * 255);
        var bByte = (int)(b * 255);

        return $"{rByte} {gByte} {bByte}";
    }
}
