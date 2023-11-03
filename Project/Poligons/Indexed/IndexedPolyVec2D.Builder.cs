using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace DoubleEngine
{
    public partial record IndexedPolyVec2D
    {
        internal static class Builder
        {
            internal static List<Sliver> GetListOfSlivers(List<Subpoly> sortedPolys, List<Subpoly> allHoles, Vec2D[] immutableVertices)
            {
                List<Subpoly>[] holesForEachPoly = new List<Subpoly>[sortedPolys.Count];
                for (int i = 0; i < holesForEachPoly.Length; i++)
                    holesForEachPoly[i] = new List<Subpoly>();
                foreach (Subpoly hole in allHoles)
                {
                    holesForEachPoly[GetPolyIndexOfHole(sortedPolys, hole, immutableVertices)].Add(hole);
                }

                List<Sliver> sliversList = new List<Sliver>(sortedPolys.Count);
                for (int i = 0; i < sortedPolys.Count; i++)
                    sliversList.Add(new Sliver(sortedPolys[i], holesForEachPoly[i]));
                return sliversList;
            }

            private static int GetPolyIndexOfHole(List<Subpoly> sortedPolys, Subpoly hole, Vec2D[] immutableVertices)
            {
                Vec2D holeFirstCorner = immutableVertices[hole.Corners[0]];
                for (int r = sortedPolys.Count - 1; r >= 0; r--)
                {
                    Vec2D[] compareToPoly = sortedPolys[r].CornersVec2D(immutableVertices);
                    if (CollisionDiscrete2D.PointInsidePolygon(compareToPoly, holeFirstCorner))//hole.CornerVec2D(0, immutableVertices)))
                        return r;
                }
                throw new Exception("Incorrect Poly - every hole should be in some poly");
            }

            
            internal static (List<Subpoly> sortedPolys, List<Subpoly> holes) SplitSortedPolysAndHoles(List<Subpoly> subpolys, ROSpanVec2D vertices)
            {
                List<Subpoly> polys = new List<Subpoly>();
                List<Subpoly> holes = new List<Subpoly>();

                for (int i = 0; i < subpolys.Count; i++)
                {
                    Subpoly subpoly = subpolys[i];
                    if (subpoly.Corners.IsPolyClockwise(vertices))
                    {
                        //Debug.Log($"subpoly{i} is poly with {subpoly.Corners.Length} corners");
                        AddSorted(polys, subpoly, vertices); //sort and add to polys;
                    }
                    else
                    {
                        //Debug.Log($"subpoly{i} is HOLE with {subpoly.Corners.Length} corners");
                        holes.Add(subpoly);
                    }
                    //Debug.Log(CornersToString(subpoly.CornersVec2D(vertices)));
                }
                return (polys, holes);
                //string CornersToString(ReadOnlySpan<Vec2D> corners) => String.Join(", ", corners.ToArray());
            }

            private static void AddSorted(List<Subpoly> polys, Subpoly toAdd, ROSpanVec2D immutableVertices)
            {
                Vec2D toAddFirstCorner = immutableVertices[toAdd.Corners[0]];
                for (int i = polys.Count - 1; i >= 0; i--)
                {
                    Vec2D[] compareToPoly = polys[i].CornersVec2D(immutableVertices);
                    if (CollisionDiscrete2D.PointInsidePolygon(compareToPoly, toAddFirstCorner))//CornerVec2D(0, immutableVertices)))
                    {
                        polys.Insert(i + 1, toAdd);
                        return;
                    }
                }
                polys.Insert(0, toAdd);
            }
        }

    }
}
