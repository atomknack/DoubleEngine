using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DoubleEngine
{

    public readonly partial struct EdgeIndexed: IEquatable<EdgeIndexed>
    {
        public readonly int start;
        public readonly int end;

        public readonly override string ToString() => $"EdgeIndexed({start}, {end})";

        public readonly bool Equals(EdgeIndexed other) => other.start == start && other.end == end;
        public static bool operator !=(EdgeIndexed lhs, EdgeIndexed rhs) => !(lhs == rhs);
        public static bool operator ==(EdgeIndexed lhs, EdgeIndexed rhs) => Equals(lhs, rhs);
        public readonly override int GetHashCode() => (start  << 11) ^ end ;
        public readonly override bool Equals(object other) => (other is EdgeIndexed) ? Equals((EdgeIndexed)other) : false;

        public EdgeIndexed((int a, int b) edge) : this(edge.a, edge.b) { }
        [JsonConstructor]
        public EdgeIndexed(int start, int end)
        {
            if (start == end)
                throw new ArgumentException("start and end indices of edge should not be equal");
            this.start = start;
            this.end = end;
        }

        //public EdgeVec2D ToEdgeVec2D(ROSpanVec2D vertices) => new EdgeVec2D(vertices[start], vertices[end]);

        public readonly EdgeIndexed Backwards() => new EdgeIndexed(end, start);

        public static void ReverseEdgesDirectionInplace(Span<EdgeIndexed> edges)
        {
            for (int i = 0; i < edges.Length; i++)
                edges[i] = edges[i].Backwards();
        }

        //public double RelationToEdge(ROSpanVec2D vertices, Vec2D point) => //Vector2dUtil.CrossLike(source[start] - point, source[end] - point);
        //                EdgeVec2D.Relation(vertices[start], vertices[end], point);
    }

}
