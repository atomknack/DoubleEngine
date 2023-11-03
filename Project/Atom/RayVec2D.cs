#define ZEROLENGTHVECTORCHECKS

using System;
using System.Collections.Generic;
using System.Text;
using DoubleEngine.Atom;

namespace DoubleEngine
{
    public readonly struct RayVec2D
    {
        public readonly Vec2D origin;
        public readonly Vec2D directionNormalized;
        public readonly Vec2D GetOrigin() => origin;
        public readonly Vec2D GetDirection() => directionNormalized;
        public RayVec2D(Vec2D origin, Vec2D direction)
        {
            this.origin = origin;
#if ZEROLENGTHVECTORCHECKS
            if (direction.MagnitudeSqr() < 0.0000_0000_1)
                throw new ArgumentException("direction vector should NOT be zero");
#endif
            directionNormalized = direction.Normalized();
        }
        public static RayVec2D FromEdgeVec2D(in EdgeVec2D edge) => new RayVec2D(edge.start, edge.segmentVector); // need testing
        public static RayVec2D FromEdgeVec2D(in Vec2D edgeStart, in Vec2D edgeEnd) => new RayVec2D(edgeStart, edgeEnd - edgeStart); // need testing

        public readonly Vec2D GetPoint(double distance) => origin + (directionNormalized * distance);
    }
}
