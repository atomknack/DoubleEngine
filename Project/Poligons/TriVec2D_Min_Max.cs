using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly partial struct TriVec2D
    {
        public readonly Vec2D Min() => Vec2D.Min(Vec2D.Min(v0, v1), v2);
        public readonly Vec2D Max() => Vec2D.Max(Vec2D.Max(v0, v1), v2);

    }
}
