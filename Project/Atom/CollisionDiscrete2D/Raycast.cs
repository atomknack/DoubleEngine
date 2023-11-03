using System;
using System.Collections.Generic;
using System.Text;
using DoubleEngine.Atom;

namespace DoubleEngine
{
    public static partial class CollisionDiscrete2D
    {
        public static bool RaycastToTri(RayVec2D ray, Vec2D v0, Vec2D v1, Vec2D v2, out double distance, out Vec2D point)
        {
            bool collision = false;
            if (RaycastToEdge(ray, v0, v1, out distance, out point))
                collision = true;
            if (RaycastToEdge(ray, v1, v2, out double tempDistance, out Vec2D tempPoint))
            {
                if (collision)
                {
                    if (distance > tempDistance)
                    {
                        distance = tempDistance;
                        point = tempPoint;
                    }
                }
                else
                {
                    distance = tempDistance;
                    point = tempPoint;
                    collision = true;
                }
            }
            if (RaycastToEdge(ray, v2, v0, out tempDistance, out tempPoint))
            {
                if (collision)
                {
                    if (distance > tempDistance)
                    {
                        distance = tempDistance;
                        point = tempPoint;
                    }
                }
                else
                {
                    distance = tempDistance;
                    point = tempPoint;
                    collision = true;
                }
            }
            return collision;
        }
        public static bool RaycastToEdge(RayVec2D ray, Vec2D edgeStart, Vec2D edgeEnd, out double distance, out Vec2D point)
        {
            Vec2D rayDirection = ray.directionNormalized;
            Vec2D otherEndMinusStart = edgeEnd - edgeStart;
            double denominator = Vec2D.Cross(rayDirection, otherEndMinusStart);

            if (Math.Abs(denominator) > 0.00001f)
            {
                Vec2D rayEdgeCommon = ray.origin - edgeStart;
                distance = Vec2D.Cross(otherEndMinusStart, rayEdgeCommon) / denominator;
                double positionOnEdge = Vec2D.Cross(rayDirection, rayEdgeCommon) / denominator;

                if (distance >= 0 && positionOnEdge >= 0 && positionOnEdge <= 1)
                {
                    point = EdgeVec2D.GetPoint(edgeStart, edgeEnd, positionOnEdge);
                    return true;
                }
            }
            point = new Vec2D();
            distance = 0;
            return false;
        }

        /*public static bool RaycastToEdge(RayVec2D ray, Vec2D edgeStart, Vec2D edgeEnd, out Vec2D point)
        {
            if (RaycastToEdge(ray, edgeStart, edgeEnd, out double distance))
            {
                point = ray.GetPoint(distance);
                return true;
            }
            point = new Vec2D();
            return false;
        }*/
    }
}
