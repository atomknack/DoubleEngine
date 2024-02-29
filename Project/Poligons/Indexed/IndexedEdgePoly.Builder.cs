//using JM.LinqFaster;

using CollectionLike;
using CollectionLike.Enumerables;
using DoubleEngine.Helpers;
using VectorCore;
using System;
using System.Linq;

namespace DoubleEngine
{
    public partial record IndexedEdgePoly
    {
        public class Builder
        {
            public static bool SingleEdgesNotValid(EdgeIndexed[] singleEdges)
            {
                foreach (EdgeIndexed edge in singleEdges)
                {
                    if (singleEdges.Count(x => x.start == edge.start) != 1 || singleEdges.Count(x => x.end == edge.end) != 1)
                    {
                        return true;
                    }
                    if (edge.start == edge.end)
                    {
                        Logger.Log($"edge start {edge.start} equal edge {edge.end}, mesh probably incorrect");
                    }
                }
                return false;
            }

            //todo rewrite like IndexedPolyVec2D.Subtracter.TryAssembleFromCutted
            internal static (Memory<EdgeIndexed> subPolygon, Memory<EdgeIndexed> remainder) CutSubPolygon(
                ROSpanVec2D source,
                Memory<EdgeIndexed> edges)
            {
                Span<EdgeIndexed> edgeSpan = edges.Span;
                int edgeIndex = StartingEdgeIndex(source, edgeSpan);
                int tryFindUniqueStartTries = edges.Length+1;
                while (tryFindUniqueStartTries > 0)
                {
                    var edge = edgeSpan[edgeIndex];
                    if (edgeSpan.Count(e => e.start == edge.start) == 1)
                        break;
                    --tryFindUniqueStartTries;
                    edgeIndex = EdgeIndexWhere_StartVectorIndexIsAndPrevEdge(source, edgeSpan, edgeSpan[edgeIndex].end, 0, edgeIndex);
                }
                if (tryFindUniqueStartTries == 0)
                    throw new Exception("polygon consist from only common vertices");

                (edgeSpan[0], edgeSpan[edgeIndex]) = (edgeSpan[edgeIndex], edgeSpan[0]);

                int startVertexIndex = edgeSpan[0].start;
                int nextVertexIndex = edgeSpan[0].end;
                int i;
                for (i = 1; i < edgeSpan.Length && nextVertexIndex != startVertexIndex; i++)
                {
                    edgeIndex = EdgeIndexWhere_StartVectorIndexIsAndPrevEdge(source, edgeSpan, nextVertexIndex, i, i - 1);
                    (edgeSpan[i], edgeSpan[edgeIndex]) = (edgeSpan[edgeIndex], edgeSpan[i]);
                    nextVertexIndex = edgeSpan[i].end;
                }
                if (i < edgeSpan.Length)
                    return (edges.Slice(0, i), edges.Slice(i, edges.Length - i));
                return (edges.Slice(0, i), Memory<EdgeIndexed>.Empty);
            }

            private static int StartingEdgeIndex(ROSpanVec2D source, Span<EdgeIndexed> subPolygon)
            {
                int startVectorIndex = StartVectorIndexWhere_MinXWhereMinY(source, subPolygon);
                return EdgeIndexWhere_StartVectorIndexIs(source, subPolygon, startVectorIndex, 0);
            }
            private static int EdgeIndexWhere_StartVectorIndexIsAndPrevEdge(ROSpanVec2D source, Span<EdgeIndexed> subPolygon, int startVectorIndex, int searchFromIndex, int prevEdgeIndex)
            {
                int foundEdgeIndex = subPolygon.IndexOf(edge => edge.start == startVectorIndex, searchFromIndex);
                if (foundEdgeIndex == -1)
                    throw new Exception($"Cannot find edges starting with startVectorIndex {startVectorIndex}");
                for (int i = searchFromIndex; i < subPolygon.Length; i++)
                {
                    if (i != foundEdgeIndex && subPolygon[i].start == startVectorIndex) // in normal polygon this should never happen, because every outer edge vertice should have only one edge to in and one from it.
                    {
                        /*if (EdgeShouldBeReplaced(
                            subPolygon[prevEdgeIndex].ToEdgeVec2D(source), 
                            subPolygon[foundEdgeIndex].ToEdgeVec2D(source), 
                            subPolygon[i].ToEdgeVec2D(source)))*/
                        if (IndexedPolyVec2D.Subtracter.EdgeShouldBeReplacedByRightMost(
                            subPolygon[prevEdgeIndex].ToEdgeVec2D(source),
                            subPolygon[foundEdgeIndex].ToEdgeVec2D(source),
                            subPolygon[i].ToEdgeVec2D(source)))
                            foundEdgeIndex = i;
                    }
                }
                return foundEdgeIndex;
            }
            /* 
            // public bool RelationToLeft(in Vec2D point) => Relation(in point) > 0;
            // public bool RelationToRight(in Vec2D point) => Relation(in point) < 0;
            internal static bool EdgeShouldBeReplaced(in EdgeVec2D previous, in EdgeVec2D current, in EdgeVec2D newCurrent)
            {
                // RelationToEdge bigger than zero if point is to the left along the edge
                // if new edge end is to the left from old one then it describes bigger polygon 
                if (previous.RelationToLeft(current.end) && previous.RelationToRight(newCurrent.end))
                    return true;
                if (previous.RelationToRight(newCurrent.end) && current.RelationToRight(newCurrent.end))
                    return true;
                if (previous.RelationToLeft(current.end) && current.RelationToLeft(newCurrent.end))
                    return true;
                return false;
            }*/
            private static int EdgeIndexWhere_StartVectorIndexIs(ROSpanVec2D source, Span<EdgeIndexed> subPolygon, int startVectorIndex, int searchFromIndex = 0)
            {
                int foundEdgeIndex = subPolygon.IndexOf(edge => edge.start == startVectorIndex, searchFromIndex);
                if (foundEdgeIndex == -1)
                    throw new Exception($"Cannot find edges starting with startVectorIndex {startVectorIndex}");
                for (int i = searchFromIndex; i < subPolygon.Length; i++)
                {
                    if (i != foundEdgeIndex && subPolygon[i].start == startVectorIndex) // in normal polygon this should never happen, because every outer edge vertice should have only one edge to in and one from it.
                    {

                        if (subPolygon[foundEdgeIndex].RelationToEdge(source, source[subPolygon[i].end]) < 0)
                            foundEdgeIndex = i;
                        // RelationToEdge bigger than zero if point is to the left along the edge
                        // if new edge end is to the left from old one then it describes bigger polygon 
                    }
                }
                return foundEdgeIndex;
            }

            private static int StartVectorIndexWhere_MinXWhereMinY(ROSpanVec2D source, Span<EdgeIndexed> subPolygon) //not tested
            {
                //no null or empty check
                int minIndex = subPolygon[0].start;
                for (int i = 1; i < subPolygon.Length; i++)
                {
                    minIndex = Vec2DUtil.IndexMinXWhereMinY(source, minIndex, subPolygon[i].start);
                }
                return minIndex;
            }
        }
    }

}