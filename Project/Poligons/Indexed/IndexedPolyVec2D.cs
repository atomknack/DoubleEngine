using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    //WIP
    public partial record IndexedPolyVec2D
    {
        internal static readonly int[] EmptyCorners = new int[0];
        internal static readonly Sliver[] EmptySlivers = new Sliver[0];

        public static readonly IndexedPolyVec2D Empty = new IndexedPolyVec2D();

        private IndexedPolyVec2D()
        {
            _vertices = MeshFragmentVec2D.EmptyVertices;
            _allCorners = EmptyCorners;
            _slivers = EmptySlivers;
            _iEPoly = IndexedEdgePoly.Empty;
        }

        internal readonly Vec2D[] _vertices;
        private readonly int[] _allCorners;
        internal readonly Sliver[] _slivers;
        internal readonly IndexedEdgePoly _iEPoly;

        public ReadOnlySpan<Vec2D> GetVertices() => new ReadOnlySpan<Vec2D>(_vertices);
        public ReadOnlySpan<Sliver> GetSlivers() => new ReadOnlySpan<Sliver>(_slivers);
        public IndexedEdgePoly GetIndexedEdgePoly() => _iEPoly;

        public static IndexedPolyVec2D CreateIndexedPolyVec2D(MeshFragmentVec2D meshFragment)
        {
            if(meshFragment == MeshFragmentVec2D.Empty)
                return Empty;
            return new IndexedPolyVec2D(meshFragment);
        }
        public IndexedPolyVec2D(MeshFragmentVec2D meshFragment): 
            this(meshFragment.vertices, IndexedEdgePoly.CreateIndexedEdgePolyFromTriangles(meshFragment.vertices, meshFragment.triangles))
        {
        }

        public static IndexedPolyVec2D CreateIndexedPolyVec2D(Vec2D[] immutableVertices, IndexedEdgePoly iEPoly)
        {
            if (immutableVertices.Length == 0 || iEPoly == IndexedEdgePoly.Empty)
                return Empty;
            return new IndexedPolyVec2D(immutableVertices, iEPoly);
        }


        private IndexedPolyVec2D(Vec2D[] immutableVertices, IndexedEdgePoly iEPoly)
        {
            _vertices=immutableVertices;
            _iEPoly = iEPoly;
            (List<Subpoly> subpolys, _allCorners) = MakeSubpolys(_iEPoly, _vertices);
            (List<Subpoly> sortedPolys, List<Subpoly> allHoles) = Builder.SplitSortedPolysAndHoles(
                subpolys,
                _vertices);//this._vertices);
            //Debug.Log($"sortedPolys {sortedPolys.Count}, allHoles {allHoles.Count}");
            _slivers = Builder.GetListOfSlivers(sortedPolys, allHoles, _vertices).ToArray();
            //Debug.Log($"constructor slivers count {_slivers.Length}");
        }

            //[System.Diagnostics.Conditional("DEBUG")]
            //public 

            private static (List<Subpoly> subpolys, int[] allCorners) MakeSubpolys(IndexedEdgePoly iEPoly, ROSpanVec2D vertices)
        {
            List<Subpoly> subpolys = new List<Subpoly>();
            int[] allCorners = new int[iEPoly.allEdges.Length];
            int cornerIndex = 0;
            for (int i = 0; i < iEPoly.subPolygons.Length; i++)
            {
                int polyStart = cornerIndex;
                ReadOnlyMemory<EdgeIndexed> edgesMemory = iEPoly.subPolygons[i];
                ReadOnlySpan<EdgeIndexed> edges = edgesMemory.Span;
                for (int s = 0; s < edges.Length; s++, cornerIndex++)
                {
                    //Debug.Log($"{cornerIndex} {allCorners.Length} ; {s} {edges.Length}");
                    allCorners[cornerIndex] = edges[s].start;
                }
                ReadOnlyMemory<int> cornersMemory = new Memory<int>(allCorners, polyStart, cornerIndex - polyStart);
                Subpoly newPoly = new Subpoly(cornersMemory, edgesMemory);
                subpolys.Add(newPoly);
            }
            return (subpolys, allCorners);
        }

        public List<IndexedTri> Triangulate()
        {
            List<IndexedTri> triangles = new List<IndexedTri>();
            for (int i = 0; i < _slivers.Length; i++)
            {
                if (!TrianglesBuilder.TryTriangulateSliver(_slivers[i], ref triangles, _vertices, _allCorners))
                {
                    throw new Exception($"Cannot triangulate sliver {i}");
                    //Debug.Log($"Cannot triangulate sliver {i}");
                }
                //Debug.Log($"result triangles after sliver{i}, corners {_slivers[i]._poly.cornersMemory.Length}, holes {_slivers[i]._holes?.Length}, triangulation count {triangles.Count}");
            }
            return triangles;
        }

    }
}
