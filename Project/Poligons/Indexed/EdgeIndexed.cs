using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DoubleEngine
{
    public readonly partial struct EdgeIndexed
    {
        public EdgeVec2D ToEdgeVec2D(ROSpanVec2D vertices) => new EdgeVec2D(vertices[start], vertices[end]);
        public double RelationToEdge(ROSpanVec2D vertices, Vec2D point) => //Vector2dUtil.CrossLike(source[start] - point, source[end] - point);
                        EdgeVec2D.Relation(vertices[start], vertices[end], point);
    }
}
