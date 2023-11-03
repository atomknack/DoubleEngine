using System;
using System.Collections.Immutable;

namespace DoubleEngine
{
    public partial record IndexedPolyVec2D
    {
        public readonly record struct Subpoly
        {
            public readonly ReadOnlyMemory<int> cornersMemory;
            public readonly ReadOnlyMemory<EdgeIndexed> indexedEdges;
            public readonly ReadOnlySpan<int> Corners { get { return cornersMemory.Span; } }
            public Vec2D[] CornersVec2D(ROSpanVec2D vertices) => vertices.AssembleIndices(Corners);
            internal Subpoly(ReadOnlyMemory<int> cornersMemory, ReadOnlyMemory<EdgeIndexed> indexedEdges)
            {
                this.cornersMemory = cornersMemory;
                this.indexedEdges = indexedEdges;
            }
        }

    }
}
