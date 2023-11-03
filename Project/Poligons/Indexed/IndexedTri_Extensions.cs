using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static partial class IndexedTri_Extensions
    {
        public static Vec3D Normal(this IndexedTri tri, ReadOnlySpan<Vec3D> vertices) =>
            Vec3D.NormalFromTriangle(vertices[tri.v0], vertices[tri.v1], vertices[tri.v2]);
        //public static Vec3D NormalFromTriangle(in Vec3D a, in Vec3D b, in Vec3D c) => Cross(b - a, c - a).Normalized();

        [Obsolete("For testing only")]
        public static TriVec2D[] ToTriVec2DArray(this List<IndexedTri> iTris, Vec2D[] vertices) =>
            ToTriVec2DArray(iTris, vertices.AsSpan());
        public static TriVec2D[] ToTriVec2DArray(this List<IndexedTri> iTris, ROSpanVec2D vertices)
        {
            TriVec2D[] triangles = new TriVec2D[iTris.Count];
            for (int i = 0; i < iTris.Count; i++)
                triangles[i] = new TriVec2D(vertices, iTris[i]);
            return triangles;
        }
        public static int[] ToTriangles(this List<IndexedTri> tris)
        {
            List<int> triangles = new List<int>(tris.Count * 3);
            for (int i = 0; i < tris.Count; i++)
            {
                triangles.Add(tris[i].v0);
                triangles.Add(tris[i].v1);
                triangles.Add(tris[i].v2);
            }
            return triangles.ToArray();
        }

        public static List<IndexedTri> ToIndexedTriList(this ReadOnlySpan<int> triangles)
        {
            if (triangles.Length % 3 != 0)
                throw new ArgumentException("triangles length should be divisible by 3");
            List<IndexedTri> tris = new List<IndexedTri>(triangles.Length/ 3);
            for (int i = 0; i < triangles.Length; i+=3)
            {
                tris.Add(new IndexedTri(triangles[i], triangles[i + 1], triangles[i + 2]));
            }
            return tris;
        }

        public static bool CanBeTriangulated(this IndexedTri iTri, ROSpanVec2D vertices, ROSpanInt allCorners)
        {
            TriVec2D tri = new TriVec2D(vertices, iTri);
            if (tri.IsTriangleClockwise() == false)
                return false;
            for (int i = 0; i < allCorners.Length; i++)
            {
                if (iTri.IsNotCorner(allCorners[i]) && CollisionDiscrete2D.OverlapPoint(tri, vertices[allCorners[i]]))
                    return false;
            }
            return true;
        }
        public static int NumberOfSameVertices(this IndexedTri iTri, IndexedTri other)
        {
            int count = 0;
            if (iTri.v0 == other.v0 || iTri.v0 == other.v1 || iTri.v0 == other.v2)
                count++;
            if (iTri.v1 == other.v0 || iTri.v1 == other.v1 || iTri.v1 == other.v2)
                count++;
            if (iTri.v2 == other.v0 || iTri.v2 == other.v1 || iTri.v2 == other.v2)
                count++;
            return count;
        }
        public static bool HaveCommonOppositeEdge( this IndexedTri iTri, IndexedTri compareTo)
        {
            foreach (IndexedTri other in stackalloc[] { compareTo, compareTo.ShiftOnce(), compareTo.ShiftTwice() })
                if ((iTri.v1 == other.v0 && iTri.v0 == other.v1) ||
                    (iTri.v2 == other.v1 && iTri.v1 == other.v2) ||
                    (iTri.v0 == other.v2 && iTri.v2 == other.v0))
                    return true;
            return false;
        }

        /*[Obsolete(" Use CanBeTriangulated(this IndexedTri iTri, ROSpanVec2D vertices, ROSpanInt allCorners) instead")]
public static bool CanBeTriangulated(this IndexedTri iTri, ROSpanVec2D vertices)
{
    TriVec2D tri = new TriVec2D(vertices, iTri);
    if (tri.IsTriangleClockwise() == false)
        return false;
    for (int pointIndex = 0; pointIndex < vertices.Length; pointIndex++)
    {
        if (iTri.IsNotCorner(pointIndex) && CollisionDiscrete2D.OverlapPoint(tri, vertices[pointIndex]))
            return false;
    }
    return true;
}*/
    }
}
