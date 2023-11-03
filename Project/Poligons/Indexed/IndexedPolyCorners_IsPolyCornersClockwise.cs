using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static partial class IndexedPolyCorners
    {
        public static bool IsPolyCornersClockwise(this int[] polyCorners, Vec2D[]  vertices) //need testing
        {
            double sum = 0;
            Vec2D prev = vertices[polyCorners.Last()]; //here was bug: was Vec2D prev = vertices.Last(); TODO white test where last vertice in vertices makes clockwise poly counterclockwise
            for (int i = 0; i < polyCorners.Length; i++)
            {
                Vec2D current = vertices[polyCorners[i]];
                sum += (current.x - prev.x)*(current.y + prev.y);
                prev = current;
            }
            return sum > 0;
        }

    }
}
