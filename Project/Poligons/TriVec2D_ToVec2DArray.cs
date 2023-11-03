using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly partial struct TriVec2D
    {
        public readonly Vec2D[] ToVec2DArray() => new Vec2D[]{ v0, v1, v2 };
    }
}
