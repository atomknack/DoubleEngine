using CollectionLike.Enumerables;
using CollectionLike.Pooled;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace DoubleEngine
{
    public static partial class PolyHelpers
    {
        public static bool IsPolyClockwise(this ImmutableArray<int> polyCornersIndices, ImmutableArray<Vec2D> vertices) =>
            IsPolyClockwise(polyCornersIndices.AsSpan(), vertices.AsSpan());
        public static bool IsPolyClockwise(this int[] polyCornersIndices, Vec2D[] vertices) =>
            IsPolyClockwise(new ReadOnlySpan<int>(polyCornersIndices), new ROSpanVec2D(vertices));
        public static bool IsPolyClockwise(this ReadOnlySpan<int> polyCornersIndices, ROSpanVec2D vertices) //need testing
        {
            double sum = 0;
            Vec2D prev = vertices[polyCornersIndices.Last()]; //here was bug: was Vec2D prev = vertices.Last(); TODO white test where last vertice in vertices makes clockwise poly counterclockwise
            for (int i = 0; i < polyCornersIndices.Length; i++)
            {
                Vec2D current = vertices[polyCornersIndices[i]];
                sum += (current.x - prev.x)*(current.y + prev.y);
                prev = current;
            }
            return sum > 0;
        }

    }
}
