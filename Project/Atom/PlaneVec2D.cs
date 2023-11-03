using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly struct PlaneVec2D
    {
        public readonly Vec2D normal;
        public readonly double distance;
        public readonly Vec2D GetNormal() => normal;
        public readonly double GetDistance() => distance;
        public PlaneVec2D(in Vec2D normal, double distance)
        {
            this.normal = normal.Normalized();
            this.distance = distance;
        }
        public PlaneVec2D(in Vec2D normal, in Vec2D point)
        {
            this.normal = normal.Normalized();
            distance = -Vec2D.Dot(this.normal, point);
        }
        /*public Plane3D(in Vec3D a, in Vec3D b, in Vec3D c) //need testing
        {
            normal = Vec3D.NormalFromTriangle(a, b, c);
            distance = -Vec2D.Dot(normal, a);
        }*/
        public readonly PlaneVec2D Flipped() => new PlaneVec2D(-normal, -distance);
        public readonly PlaneVec2D Translated(Vec2D translation) => new PlaneVec2D(normal, distance + Vec2D.Dot(normal, translation)); //need testing
        public bool Raycast(RayVec2D ray, out double collisionDistance) //need testing
        {
            double cosRayPlane = Vec2D.Dot(ray.directionNormalized, normal);
            if (MathU.Abs(cosRayPlane) < 0.0000000001d)
            {
                collisionDistance = 0;
                return false;
            }
            double distanceFromRayOrigin = -Vec2D.Dot(ray.origin, normal) - distance;
            collisionDistance = distanceFromRayOrigin / cosRayPlane;
            return distanceFromRayOrigin > 0d;
        }
    }
}
