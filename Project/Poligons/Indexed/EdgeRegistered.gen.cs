using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DoubleEngine
{

    public readonly partial struct EdgeRegistered: IEquatable<EdgeRegistered>
    {
        public readonly RegistryIndex start;
        public readonly RegistryIndex end;

        public readonly override string ToString() => $"EdgeRegistered({start.Value}, {end.Value})";

        public readonly bool Equals(EdgeRegistered other) => other.start == start && other.end == end;
        public static bool operator !=(EdgeRegistered lhs, EdgeRegistered rhs) => !(lhs == rhs);
        public static bool operator ==(EdgeRegistered lhs, EdgeRegistered rhs) => Equals(lhs, rhs);
        public readonly override int GetHashCode() => (start.Value  << 11) ^ end.Value ;
        public readonly override bool Equals(object other) => (other is EdgeRegistered) ? Equals((EdgeRegistered)other) : false;

        public EdgeRegistered((RegistryIndex a, RegistryIndex b) edge) : this(edge.a, edge.b) { }
        [JsonConstructor]
        public EdgeRegistered(RegistryIndex start, RegistryIndex end)
        {
            if (start == end)
                throw new ArgumentException("start and end indices of edge should not be equal");
            this.start = start;
            this.end = end;
        }

        //public EdgeVec2D ToEdgeVec2D(ROSpanVec2D vertices) => new EdgeVec2D(vertices[start], vertices[end]);

        public readonly EdgeRegistered Backwards() => new EdgeRegistered(end, start);

        public static void ReverseEdgesDirectionInplace(Span<EdgeRegistered> edges)
        {
            for (int i = 0; i < edges.Length; i++)
                edges[i] = edges[i].Backwards();
        }

        //public double RelationToEdge(ROSpanVec2D vertices, Vec2D point) => //Vector2dUtil.CrossLike(source[start] - point, source[end] - point);
        //                EdgeVec2D.Relation(vertices[start], vertices[end], point);
    }

}
