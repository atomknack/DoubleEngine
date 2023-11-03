using System;
using System.Collections.Generic;
using System.Text;
using DoubleEngine;

namespace DoubleEngine
{
    public static partial class CollisionDiscrete2D
    {
        public static bool PointInsidePolygon(this ReadOnlySpan<Vec2D> corners, Vec2D point)
        {
            int polyCorners = corners.Length - 1;
            bool result = false;
            (double x, double y) = point;
            Vec2D prevCorner = corners[polyCorners];

            for (int i = 0; i <= polyCorners; i++)
            {
                Vec2D corner = corners[i];
                if( (prevCorner.y >= y && corner.y<y) || (corner.y >= y && prevCorner.y <y ) )
                {
                    Vec2D edgeVector = corner - prevCorner;
                    double yPosition = (y - prevCorner.y) / edgeVector.y;
                    if (prevCorner.x + (yPosition * edgeVector.x) < x)
                        result = !result;
                }
                prevCorner = corner;
            }

            return result;
        }

    }
}
