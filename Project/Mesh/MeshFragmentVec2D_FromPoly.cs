using CollectionLike;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public partial record MeshFragmentVec2D
    {
        public static MeshFragmentVec2D FromPolyCopy(ReadOnlySpan<Vec2D> poly)
        {
            Vec2D[] vertices = poly.ToArray();
            Span<int> indices = stackalloc int[poly.Length];
            indices.FillAsRange();
            var singleEdges = ((ReadOnlySpan<int>)indices).IndexedEdgesFromCorners();
            var iEPoly = IndexedEdgePoly.DebugIndexedEdgePolyFromSingleEdges(singleEdges, vertices);
            var iPolyVec2D = IndexedPolyVec2D.CreateIndexedPolyVec2D(vertices, iEPoly);
            int[] triangles = iPolyVec2D.Triangulate().ToTriangles();
            return new MeshFragmentVec2D(vertices, triangles, 0);
        }
    }
}
