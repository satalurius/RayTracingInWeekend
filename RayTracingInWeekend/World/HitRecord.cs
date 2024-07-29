using RayTracingInWeekend.Geometry;
using RayTracingInWeekend.Materials;

namespace RayTracingInWeekend.World;
public class HitRecord
{
    public Point3 Point { get; set;} = new(0, 0, 0);
    public Vec3 Normal { get; set;} = new(0, 0, 0);
    public double T { get; set;}
    public bool FrontFace { get; set; }
    public IMaterial? Material { get; set; }
    
    public HitRecord(Point3 point, Vec3 normal, double t)
    {
        Point = point;
        Normal = normal;
        T = t;
    }
public HitRecord()
{
    
}
    public void SetFaceNormal(Ray ray, Vec3 outwardNormal) 
    {
        // Sets the hit record normal vector.
        // NOTE: the parameter `outward_normal` is assumed to have unit length.

        FrontFace = Vec3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : outwardNormal.ReversePoints();
    }

}
