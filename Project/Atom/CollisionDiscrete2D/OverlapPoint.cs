using System;
using System.Collections.Generic;
using System.Text;
using DoubleEngine.Atom;

namespace DoubleEngine
{
    public static partial class CollisionDiscrete2D
    {
        private const double EPSInside = 0.0000_0000_0000_5d;
        public static bool OverlapPoint(Vec2D v0, Vec2D v1, Vec2D v2, Vec2D point) => // need testing
            EdgeVec2D.Relation(v0, v1, point) <= EPSInside && EdgeVec2D.Relation(v1, v2, point) <= EPSInside && EdgeVec2D.Relation(v2, v0, point) <= EPSInside;
        public static bool OverlapPoint(this in TriVec2D tri, Vec2D point) => OverlapPoint(tri.v0, tri.v1, tri.v2, point);
    }
}
