using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly partial struct EdgeVec2D
    {
        public bool RelationToLeft(in Vec2D point) => Relation(in point) > 0;
        public bool RelationToRight(in Vec2D point) => Relation(in point) < 0;
        public readonly double Relation(in Vec2D point) => Vec2D.Cross(start - point, end - point);
        public static double Relation(in EdgeVec2D edge, in Vec2D point) => Vec2D.Cross(edge.start - point, edge.end - point);
        ///positive left (outside of clockwise polygon). The bigger, the lefter.
        public static double Relation(in Vec2D edgeStart, in Vec2D edgeEnd, in Vec2D point) => Vec2D.Cross(edgeStart - point, edgeEnd - point);


    }
}
