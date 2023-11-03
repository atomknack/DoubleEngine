using System;
using System.Collections.Generic;
using System.Text;
using DoubleEngine.Atom;
using CollectionLike.Comparers.SetsWithCustomComparer;

namespace DoubleEngine
{
    public static partial class CollisionDiscrete2D
    {
        public static bool EdgeIntersection(EdgeVec2D a, EdgeVec2D b, out Vec2D resultStart, out Vec2D? resultEnd) =>
            CoincidentIntersector2D.Intersection(a, b, out resultStart, out  resultEnd);
        public static Vec2DCloseEnoughSet SingleEdges2DIntersections(
            ROSpanVec2D vertices1,
            ROSpanInt triangles1,
            ROSpanVec2D vertices2,
            ROSpanInt triangles2)
        {
            EdgeIndexed[] mesh1Edges = EdgeIndexed.SingleEdgesFromTriangles(triangles1);
            //foreach (var e in mesh1Edges) Debug.Log($"{e.a} {e.b} edge mesh1");
            EdgeIndexed[] mesh2Edges = EdgeIndexed.SingleEdgesFromTriangles(triangles2);
            //foreach (var e in mesh2Edges) Debug.Log($"{e.a} {e.b} edge mesh1");

            Vec2DCloseEnoughSet result = Vec2DCloseEnoughSet.NewHashSet();

            foreach (var edge1 in mesh1Edges)
                foreach (var edge2 in mesh2Edges)
                    //if (EdgesIntersectAndNotParallel_NoSameVerticeCheck(    vertices1[edge1.start], vertices1[edge1.end], 
                    //                                                        vertices2[edge2.start], vertices2[edge2.end], out Vector2 intersection))
                    if (EdgesIntersectAndNotParallel_NoSameVerticeCheck(vertices1[edge1.start], vertices1[edge1.end],
                                                                            vertices2[edge2.start], vertices2[edge2.end], out Vec2D intersection))

                        result.Add(intersection);
            return result;
        }
        public static Vec2DCloseEnoughSet AnyEdges2DIntersections(
            ROSpanVec2D vertices1,
            ReadOnlySpan<int> triangles1,
            ROSpanVec2D vertices2,
            ReadOnlySpan<int> triangles2)
        {
            HashSet<(int start, int end)> mesh1Edges = new();
            AddEdgesToSet(mesh1Edges, triangles1);
            HashSet<(int start, int end)> mesh2Edges = new();
            AddEdgesToSet(mesh2Edges, triangles2);
            Vec2DCloseEnoughSet result = Vec2DCloseEnoughSet.NewHashSet();

            foreach (var edge1 in mesh1Edges)
                foreach (var edge2 in mesh2Edges)
                    //if (EdgesIntersectAndNotParallel_NoSameVerticeCheck(vertices1[edge1.start], vertices1[edge1.end], 
                    //                                                    vertices2[edge2.start], vertices2[edge2.end], out Vector2 intersection))
                    if (EdgesIntersectAndNotParallel_NoSameVerticeCheck(vertices1[edge1.start], vertices1[edge1.end],
                                                                            vertices2[edge2.start], vertices2[edge2.end], out Vec2D intersection))
                        result.Add(intersection);
            return result;
            void AddEdgesToSet(HashSet<(int a, int b)> edges, ReadOnlySpan<int> triangles)
            {
                for (var i = 0; i < triangles.Length; i += 3)
                {
                    if (!IsEdgeInSet(edges, triangles[i], triangles[i + 1]))
                        edges.Add((i, i + 1));
                    if (!IsEdgeInSet(edges, triangles[i + 1], triangles[i + 2]))
                        edges.Add((i + 1, i + 2));
                    if (!IsEdgeInSet(edges, triangles[i + 2], triangles[i]))
                        edges.Add((i + 2, i));
                }
                bool IsEdgeInSet(HashSet<(int a, int b)> edges, int a, int b) => edges.Contains((a, b)) || edges.Contains((b, a));
            }

        }

        [Obsolete("try not to use it, find same point beforehand and use directly EdgesIntersectAndNotParallel_NoSameVerticeCheck")]
        public static bool EdgesIntersectAndNotParallel(Vec2D edgeStart, Vec2D edgeEnd, Vec2D otherStart, Vec2D otherEnd, out Vec2D intersectionPoint)
        {
            //float denominator = (otherEnd.y - otherStart.y) * (edgeEnd.x - edgeStart.x) - (otherEnd.x - otherStart.x) * (edgeEnd.y - edgeStart.y);
            Vec2D edgeEndMinusStart = edgeEnd - edgeStart;
            Vec2D otherEndMinusStart = otherEnd - otherStart;
            double denominator = Vec2D.Cross(edgeEndMinusStart, otherEndMinusStart);
            if (Math.Abs(denominator) <= CoincidentIntersector2D.MyEpsilon)//0.00001f)
            {
                intersectionPoint = Vec2D.zero;
                return false;
            }

            if (edgeStart == otherStart || edgeStart.CloseByEach(otherStart, 1e-6f)) { intersectionPoint = edgeStart; return true; }
            if (edgeEnd == otherStart || edgeEnd.CloseByEach(otherStart, 1e-6f)) { intersectionPoint = edgeEnd; return true; }
            if (edgeStart == otherEnd || edgeStart.CloseByEach(otherEnd, 1e-6f)) { intersectionPoint = edgeStart; return true; }
            if (edgeEnd == otherEnd || edgeEnd.CloseByEach(otherEnd, 1e-6f)) { intersectionPoint = edgeEnd; return true; }
            //return EdgesIntersectAndNotParallel_NoSameVerticeCheck(edgeStart, edgeEnd, otherStart, otherEnd, out intersectionPoint);
            Vec2D intersectionVec2D;
            bool intersect = EdgesIntersectAndNotParallel_NoSameVerticeCheck(edgeStart, edgeEnd, otherStart, otherEnd, out intersectionVec2D);
            intersectionPoint = intersectionVec2D;
            return intersect;
        }

        public static bool EdgesIntersectAndNotParallel_NoSameVerticeCheck(Vec2D edgeStart, Vec2D edgeEnd, Vec2D otherStart, Vec2D otherEnd, out Vec2D intersectionPoint)
        {
            Vec2D edgeEndMinusStart = edgeEnd - edgeStart;
            Vec2D otherEndMinusStart = otherEnd - otherStart;
            double denominator = Vec2D.Cross(edgeEndMinusStart, otherEndMinusStart);
            //float denominator = (edgeA2.x - edgeA1.x)* (edgeB2.y - edgeB1.y)  - (edgeA2.y - edgeA1.y) * (edgeB2.x - edgeB1.x);

            if (Math.Abs(denominator) > CoincidentIntersector2D.MyEpsilon) //0.00001f)
            {
                Vec2D edgeStartMinus_otherStart = edgeStart - otherStart;
                double ua = Vec2D.Cross(otherEndMinusStart, edgeStartMinus_otherStart) / denominator;
                //float ua = ( (edgeB2.x - edgeB1.x) * (edgeA1.y - edgeB1.y) - (edgeB2.y - edgeB1.y) * (edgeA1.x - edgeB1.x) ) / denominator;
                double ub = Vec2D.Cross(edgeEndMinusStart, edgeStartMinus_otherStart) / denominator;
                //float ub = ( (edgeA2.x - edgeA1.x) * (edgeA1.y - edgeB1.y) - (edgeA2.y - edgeA1.y) * (edgeA1.x - edgeB1.x) ) / denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersectionPoint = new Vec2D(edgeStart.x + ua * edgeEndMinusStart.x, edgeStart.y + ua * edgeEndMinusStart.y);
                    //intersectionPoint = new Vector2(edgeA1.x + ua * (edgeA2.x - edgeA1.x), edgeA1.y + ua * (edgeA2.y - edgeA1.y));
                    return true;
                }
            }

            intersectionPoint = Vec2D.zero;
            return false;
        }
    }
}
