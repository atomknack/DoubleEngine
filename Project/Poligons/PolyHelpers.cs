using CollectionLike;
using CollectionLike.Comparers.SetsWithCustomComparer;
using DoubleEngine.Helpers;
using System;
using System.Collections.Generic;
//using System.Collections.Immutable;
using System.Text;

namespace DoubleEngine
{
    public static partial class PolyHelpers
    {
        public static MeshFragmentVec2D TriangulateSinglePolyWithoutHoles(this Vec2D[] corners)
        {
            int[] indices = new int[corners.Length];
            indices.AsSpan().FillAsRange();
            EdgeIndexed[] edges = IndexedEdgesFromCorners(indices);
            IndexedPolyVec2D.Sliver sliver = 
                new IndexedPolyVec2D.Sliver(new IndexedPolyVec2D.Subpoly(indices.AsMemory(), edges.AsMemory()),null);
            List<IndexedTri> resultTriangles = new List<IndexedTri>(corners.Length * 2);//Expendables.CreateList<IndexedTri>(corners.Length * 2))
            try
            {
                IndexedPolyVec2D.TrianglesBuilder.TryTriangulateSliver(sliver, ref resultTriangles, corners, indices);
            }
            catch (Exception ex)
            {
                Logger.DebugLog("Error triangulating simple poly without holes");
                Logger.LogException(ex);
                throw;
            };
            return new MeshFragmentVec2D(corners,resultTriangles.ToTriangles(), 0);
        }

        public static bool PolyVec2DShiftedEqual(this Vec2D[] poly, Vec2D[] other) =>
            InternalTesting.Polys.PolyShiftedEqual((ReadOnlySpan<Vec2D>)poly, (ReadOnlySpan<Vec2D>)other, Vec2DComparer_2em5.StaticEquals);
        public static bool PolyVec2DShiftedEqual(this Span<Vec2D> poly, Span<Vec2D> other) =>
            InternalTesting.Polys.PolyShiftedEqual((ReadOnlySpan<Vec2D>)poly, (ReadOnlySpan<Vec2D>)other, Vec2DComparer_2em5.StaticEquals);
        [Obsolete("Need testing")]
        public static bool PolyVec2DShiftedEqual(this ReadOnlySpan<Vec2D> poly, ReadOnlySpan<Vec2D> other) =>
            InternalTesting.Polys.PolyShiftedEqual(poly, other, Vec2DComparer_2em5.StaticEquals);

        /* //not tested at all
         public static EdgeVec2D[] EdgesVec2DFromRegisteredCorners(this ReadOnlySpan<EverGrowingVec2D.RegistryIndex> corners, EverGrowingVec2D registry) //should be correct enclosed poly 
        {
            EdgeVec2D[] edges = new EdgeVec2D[corners.Length];
            int indexOfLastElement = corners.IndexOfLastElement();
            for (int i = 0; i < indexOfLastElement; i++)
                edges[i] = new EdgeVec2D(registry.GetItem(corners[i]), registry.GetItem(corners[i + 1]));
            edges[indexOfLastElement] = new EdgeVec2D(registry.GetItem(corners[indexOfLastElement]), registry.GetItem(corners[0]));
            return edges;
        }

        public static IndexedEdge[] EdgesVec2DFromCorners(this ReadOnlySpan<int> corners) //should be correct enclosed poly 
        {
            IndexedEdge[] edges = new IndexedEdge[corners.Length];
            int indexOfLastElement = corners.IndexOfLastElement();
            for (int i = 0; i < indexOfLastElement; i++)
                edges[i] = new IndexedEdge(corners[i], corners[i + 1]);
            edges[indexOfLastElement] = new IndexedEdge(corners[indexOfLastElement], corners[0]);
            return edges;
        }

        public static IEnumerable<IndexedEdge> IndexedEdgesEnumerableFromCorners(this IReadOnlyList<int> corners) //should be correct enclosed poly 
        {
            int indexOfLastElement = corners.IndexOfLastElement();
            for (int i = 0; i < indexOfLastElement; i++)
                yield return new IndexedEdge(corners[i], corners[i + 1]);
            yield return new IndexedEdge(corners[indexOfLastElement], corners[0]);
        }

        //public static IndexedEdge[] IndexedEdgesFromCorners(this ImmutableArray<int> corners) //should be correct enclosed poly
        //    => IndexedEdgesFromCorners(corners.AsSpan());
        public static IndexedEdge[] IndexedEdgesFromCorners(this int[] corners) //should be correct enclosed poly
            => IndexedEdgesFromCorners(new ReadOnlySpan<int>(corners));
        public static IndexedEdge[] IndexedEdgesFromCorners(this ReadOnlySpan<int> corners) //should be correct enclosed poly 
        {
            IndexedEdge[] edges = new IndexedEdge[corners.Length];
            int indexOfLastElement = corners.IndexOfLastElement();
            for (int i = 0; i < indexOfLastElement; i++)
                edges[i] = new IndexedEdge(corners[i], corners[i + 1]);
            edges[indexOfLastElement] = new IndexedEdge(corners[indexOfLastElement], corners[0]);
            return edges;
        }
        */

    }
}
