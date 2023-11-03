using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using JM.LinqFaster;

namespace DoubleEngine
{
    public partial record IndexedEdgePoly
    {
        internal static readonly EdgeIndexed[] EmptyIndexedEdges = new EdgeIndexed[0];
        internal static readonly Memory<EdgeIndexed>[] EmptyMemoryIndexedEdges = new Memory<EdgeIndexed>[0];

        public static readonly IndexedEdgePoly Empty = new IndexedEdgePoly();

        public readonly EdgeIndexed[] allEdges;
        public readonly Memory<EdgeIndexed>[] subPolygons;

        private IndexedEdgePoly()
        {
            allEdges = EmptyIndexedEdges;
            subPolygons = EmptyMemoryIndexedEdges;
        }
        internal static IndexedEdgePoly CreateIndexedEdgePolyFromTriangles(ROSpanVec2D vertices, ROSpanInt triangles) =>
            vertices.Length == 0 || triangles.Length == 0 ? 
            Empty :
            new IndexedEdgePoly(EdgeIndexed.SingleEdgesFromTriangles(triangles), vertices);

        public static IndexedEdgePoly DebugIndexedEdgePolyFromSingleEdges(Span<EdgeIndexed> singleEdges, ROSpanVec2D vertices) =>
            vertices.Length == 0 || singleEdges.Length == 0 ?
            Empty :
            new IndexedEdgePoly(singleEdges, vertices);

        internal IndexedEdgePoly(Span<EdgeIndexed> singleEdges, ROSpanVec2D vertices)
        {
            if (vertices.Length == 0)
                throw new ArgumentException("referencedVertices is empty");
            //if (Builder.SingleEdgesNotValid(singleEdges))
            //    throw new ArgumentException("single Edges Not Valid");
            allEdges = singleEdges.ToArray();//.Clone();//IndexedEdge.FromSingleEdgesTupleArray(singleEdges).ToArray();
            //
            List<Memory<EdgeIndexed>> tempSubPolygons = new List<Memory<EdgeIndexed>>();

            Memory<EdgeIndexed> toCutFrom = allEdges.AsMemory();
            while (toCutFrom.IsEmpty == false)
            {
                (Memory<EdgeIndexed> carvedPolygon, Memory<EdgeIndexed> remainder) = Builder.CutSubPolygon(vertices, toCutFrom);
                tempSubPolygons.Add(carvedPolygon);
                toCutFrom = remainder;
            }
            subPolygons = tempSubPolygons.ToArray();
            //Debug.Log($"IndexedEdgePoly, subpoly count {subPolygons.Length}");
        }
        public static Vec2D[] GetCorners(IndexedEdgePoly indexedPoly, int subpoly, Vec2D[] vertices) =>
            vertices.AssembleIndices(GetCorners(indexedPoly, subpoly));
        public static T[] GetCorners<T>(IndexedEdgePoly indexedPoly, int subpoly, T[] vertices) =>
            vertices.AssembleIndices(GetCorners(indexedPoly, subpoly));
        public static int[] GetCorners(IndexedEdgePoly indexedPoly, int subpoly) =>
            GetCorners(indexedPoly.subPolygons[subpoly].Span);
        private static int[] GetCorners(ReadOnlySpan<EdgeIndexed> iEdges) //should be correct enclosed poly 
        {
            int[] corners = new int[iEdges.Length];
            for (int i = 0; i < iEdges.Length; i++)
                corners[i] = iEdges[i].start;
            return corners;
        }

        /*
        public EdgeVec2D[] AllEdgesVec2D(Vec2D[] vertices)
        {
            EdgeVec2D[] edges = new EdgeVec2D[allEdges.Length];
            for (int i = 0; i < allEdges.Length; i++)
                edges[i] = allEdges[i].EdgeVec2D(vertices);
            return edges;
        }
        */
    }

}