using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly partial struct TriVec2D
    {
        public readonly bool IsTriangleClockwise()
                => IsTriangleClockwise(v0, v1, v2);
        public static bool IsTriangleVisible(Vec2D v1, Vec2D v2, Vec2D v3) => IsTriangleClockwise(v1, v2, v3);
        public static bool IsTriangleClockwise(Vec2D v1, Vec2D v2, Vec2D v3)
            => (v1.x * v2.y + v3.x * v1.y + v2.x * v3.y - v1.x * v3.y - v3.x * v2.y - v2.x * v1.y) < 0d;
        public static bool IsTriangleCounterClockwise(Vec2D v1, Vec2D v2, Vec2D v3)
            => (v1.x * v2.y + v3.x * v1.y + v2.x * v3.y - v1.x * v3.y - v3.x * v2.y - v2.x * v1.y) > 0d;

    }
}
