using CollectionLike.Enumerables;
using CollectionLike.Pooled;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DoubleEngine
{
    public partial record IndexedPolyVec2D
    {
        public static class TrianglesBuilder
        {
            public readonly ref struct Triangulator
            {
                public readonly ROSpanVec2D vertices;
                public readonly ROSpanInt allCorners;
                public readonly List<int> poly;
                public readonly List<List<int>> holes;
                public Triangulator(ROSpanVec2D vertices, ROSpanInt allCorners, List<int> poly, List<List<int>> holes)
                {
                    this.vertices = vertices;
                    this.allCorners = allCorners;
                    this.poly = poly;
                    this.holes = holes;
                }
            }
            
            public static bool TryTriangulateSliver(Sliver sliver, ref List<IndexedTri> resultTriangles, ROSpanVec2D vertices, ROSpanInt allCorners)
            {
                if (resultTriangles == null)
                {
                    //Debug.Log("result Triangles empty, initializing");
                    resultTriangles = new List<IndexedTri>();
                }
                //Debug.Log($"result triangles before sliver triangulation count {resultTriangles.Count}");
                List<int> poly = sliver._poly.Corners.CreateNewListFromSpanElements();
                var holes = sliver.HolesCornersToAsNewLists();// (sliver._holes);
                var triangulator = new Triangulator(vertices, allCorners, poly, holes);
                if (TriangulatePolyCorners(ref poly, ref resultTriangles, triangulator))
                    return true;
                for (int i = 0; i < holes.Count; i++)
                {
                    var holeCorners = holes[i];
                    TriangulatePolyCorners(ref holeCorners, ref resultTriangles, triangulator);
                    //Debug.Log(allCorners.Length);
                    //Debug.Log(resultTriangles.Count);
                }
                bool result = false;
                while (holes.Count > 0)
                {
                    bool join = TryJoinHole(poly, holes, resultTriangles, vertices, allCorners);

                    if (join == false)
                        return false; //throw new Exception("Cannot join hole to polygon, possible vertices inside Sliver, that not belong to it");
                    result = TriangulatePolyCorners(ref poly, ref resultTriangles, triangulator);
                }
                return result;
            }
            
            public static bool TriangulatePolyCorners(
                [NotNull] ref List<int> corners, 
                [AllowNull] ref List<IndexedTri> triangulated,
                Triangulator triangulator
                )
            {
                if (triangulated == null)
                    triangulated = new List<IndexedTri>();
                //current is good enough, change if knife like triangles will be problem (first change attempt failed)
                while (FindFirstTriangleForTriangulation(corners, triangulator.vertices, out IndexedTri iTri, triangulator.allCorners, triangulator.poly, triangulator.holes))
                {
                    corners.Remove(iTri.v1);
                    triangulated.Add(iTri);
                }
                if (corners.Count > 2)// if fully triangulated corners should contain only 2 last vertices
                    return false;
                return true;
            }
            
            private static bool FindFirstTriangleForTriangulation(
                List<int> corners, 
                ROSpanVec2D vertices, 
                out IndexedTri tri, 
                ROSpanInt allCorners,
                List<int> poly,
                List<List<int>> holes
                )
            {
                if (corners.Count < 3)
                {
                    tri = new();
                    return false;
                }
                tri = new IndexedTri(corners.Last(), corners[0], corners[1]);
                if (tri.CanBeTriangulated(vertices, allCorners) && 
                    NotIntersectWithDifferentVerticesEdgesOfSliver(new EdgeIndexed(corners.Last(), corners[1]), poly, holes, vertices) )
                    return true;
                for (int i = 0; i < corners.Count - 3; i++)
                {
                    tri = new IndexedTri(corners[i], corners[i + 1], corners[i + 2]);
                    if (tri.CanBeTriangulated(vertices, allCorners) && 
                        NotIntersectWithDifferentVerticesEdgesOfSliver(new EdgeIndexed(corners[i], corners[i+2]), poly, holes, vertices))
                        return true;
                }
                return false;
            }
            

            internal static bool TryJoinHole(
                List<int> poly, 
                List<List<int>> holes, 
                List<IndexedTri> triangulated, 
                ROSpanVec2D vertices, 
                ROSpanInt allCorners)
            {
                for (int i = 0; i < holes.Count; i++)
                    if (//listOfHoles[i].Count>3 && 
                        JoinHoleToOuterPoly(ref poly, holes[i], ref triangulated, vertices, allCorners, holes))
                    {
                        holes.RemoveAt(i);
                        return true;
                    }
                return false;
            }
            private static bool JoinHoleToOuterPoly(
    ref List<int> poly,
    List<int> hole,
    ref List<IndexedTri> triangulated,
    ROSpanVec2D vertices, 
    ROSpanInt allCorners,
    List<List<int>> holes
                )
            {
                int polyEdgeStart = poly.Last();
                for (int pI = 0; pI < poly.Count; pI++)
                {
                    int polyEdgeEnd = poly[pI];
                    int holeEdgeStart = hole.Last();
                    for (int h = 0; h < hole.Count; h++)
                    {
                        int holeEdgeEnd = hole[h];
                        var tri1 = new IndexedTri(polyEdgeStart, polyEdgeEnd, holeEdgeStart);
                        var tri2 = new IndexedTri(holeEdgeStart, holeEdgeEnd, polyEdgeStart);
                        if (tri1.CanBeTriangulated(vertices, allCorners) && tri2.CanBeTriangulated(vertices, allCorners))
                        {
                            EdgeIndexed A = new EdgeIndexed(holeEdgeStart, polyEdgeEnd);
                            EdgeIndexed B = new EdgeIndexed(polyEdgeStart, holeEdgeEnd);
                            if ((!CollisionDiscrete2D.EdgeIntersection(A.ToEdgeVec2D(vertices), B.ToEdgeVec2D(vertices), out _, out _)) &&
                                NotIntersectWithDifferentVerticesEdgesOfSliver(A, poly, holes, vertices) &&
                                NotIntersectWithDifferentVerticesEdgesOfSliver(B, poly, holes, vertices))
                            {
                                //if(NotIntersectWithDifferentVerticesTriangles(A, triangulated, vertices) &&
                                //    NotIntersectWithDifferentVerticesTriangles(B, triangulated, vertices))
                                //    {
                                            triangulated.Add(tri1);
                                            triangulated.Add(tri2);
                                //    }
                                /// 
                                //check 2 triangles (polyEdgeStart, polyEdgeEnd, holeEdgeStart) and (holeEdgeStart, holeEdgeEnd, polyEdgeStart)
                                //join polys if everything ok:
                                // 1) both triangles should not have vertices inside
                                // 2) both triangles should be clockwise
                                // 3) (holeEdgeStart, polyEdgeEnd) (as A) should not intersect any edge of iEPoly that not have holeEdgeStart or polyEdgeEnd vertices
                                // 4) (polyEdgeStart, holeEdgeEnd) (as B) should not intersect any edge of iEPoly that not have polyEdgeStart or holeEdgeEnd vertices
                                // 5) A should not intersect B
                                /// and here we found place to insert hole into poly

                                hole.RotateLeftInplace(h);
                                poly.InsertRange(pI, hole);
                                return true;
                            }
                        }


                        holeEdgeStart = holeEdgeEnd;
                    }
                    polyEdgeStart = polyEdgeEnd;
                }
                return false;
            }

            private static bool NotIntersectWithDifferentVerticesEdgesOfSliver(EdgeIndexed toCheck, List<int> poly, List<List<int>> holes, ROSpanVec2D vertices)
            {
                if (CornersIntersectWithDifferentVerticesEdges(toCheck, poly, vertices))
                    return false;
                foreach (List<int> hole in holes)
                    if (CornersIntersectWithDifferentVerticesEdges(toCheck, hole, vertices))
                        return false;
                return true;
            }

            private static bool CornersIntersectWithDifferentVerticesEdges(EdgeIndexed toCheck, List<int> corners, ROSpanVec2D vertices)
            {
                foreach (var edge in corners.IndexedEdgesEnumerableFromCorners())
                {
                    if (toCheck.start == edge.start || toCheck.start == edge.end || toCheck.end == edge.start || toCheck.end == edge.end)
                    {

                    }
                    else if (CollisionDiscrete2D.EdgeIntersection(toCheck.ToEdgeVec2D(vertices), edge.ToEdgeVec2D(vertices), out _, out _))
                        return true;
                }
                return false;
            }
        }
    }
}
