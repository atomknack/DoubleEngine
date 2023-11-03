using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static partial class IndexedTri_Extensions
    {
        public static void AddTriangleAndSplitIfNeeded(this IList<IndexedTri> buffer, IndexedTri abc, ReadOnlySpan<int> splitters, ReadOnlySpan<Vec3D> vertices)
        {
            (int a, int b, int c) = abc;
            EdgeVec3D edgeAB = new EdgeVec3D(vertices[a], vertices[b]);
            EdgeVec3D edgeBC = new EdgeVec3D(vertices[b], vertices[c]);
            EdgeVec3D edgeCA = new EdgeVec3D(vertices[c], vertices[a]);
            //(Vec3D center, double sqrRadius) = TriVec3D.CalcSqrSphere(vertices[a], vertices[b], vertices[c]);
            //sqrRadius = sqrRadius + 0.0000001d;
            for (int i = 0; i < splitters.Length; ++i)
            {
                int splitter = splitters[i];
                if (splitter != a && splitter != b && splitter != c)
                {
                    Vec3D splitterPoint = vertices[splitter];
                    //if(center.CloseBySqrDistance(splitterPoint, sqrRadius))
                    //    {
                        if (edgeAB.PointBelongsToEdge(splitterPoint))
                        {
                            AddTriangleAndSplitIfNeeded(buffer, new IndexedTri(a, splitter, c), splitters, vertices);
                            AddTriangleAndSplitIfNeeded(buffer, new IndexedTri(c, splitter, b), splitters, vertices);
                            return;
                        }
                        if (edgeBC.PointBelongsToEdge(splitterPoint))
                        {
                            AddTriangleAndSplitIfNeeded(buffer, new IndexedTri(a, b, splitter), splitters, vertices);
                            AddTriangleAndSplitIfNeeded(buffer, new IndexedTri(splitter, c, a), splitters, vertices);
                            //throw new NotImplementedException();
                            return;
                        }
                        if (edgeCA.PointBelongsToEdge(splitterPoint))
                        {
                            AddTriangleAndSplitIfNeeded(buffer, new IndexedTri(a, b, splitter), splitters, vertices);
                            AddTriangleAndSplitIfNeeded(buffer, new IndexedTri(splitter, b, c), splitters, vertices);
                            //throw new NotImplementedException();
                            return;
                        }
                    //}
                }
            }

            buffer.Add(abc);
        }

        public static void AddTriangleAndSplitIfNeeded(this IList<int> buffer, int a, int b, int c, ReadOnlySpan<int> splitters, ReadOnlySpan<Vec3D> vertices)
        {
            EdgeVec3D edgeAB = new EdgeVec3D(vertices[a], vertices[b]);
            EdgeVec3D edgeBC = new EdgeVec3D(vertices[b], vertices[c]);
            EdgeVec3D edgeCA = new EdgeVec3D(vertices[c], vertices[a]);
            for (int i = 0; i < splitters.Length; ++i)
            {
                int splitter = splitters[i];
                if (splitter != a && splitter != b && splitter != c)
                {
                    Vec3D splitterPoint = vertices[splitter];
                    if (edgeAB.PointBelongsToEdge(splitterPoint))
                    {
                        AddTriangleAndSplitIfNeeded(buffer, a, splitter, c, splitters, vertices);
                        AddTriangleAndSplitIfNeeded(buffer, c, splitter, b, splitters, vertices);
                        return;
                    }
                    if (edgeBC.PointBelongsToEdge(splitterPoint))
                    {
                        AddTriangleAndSplitIfNeeded(buffer, a, b, splitter, splitters, vertices);
                        AddTriangleAndSplitIfNeeded(buffer, splitter, c, a, splitters, vertices);
                        //throw new NotImplementedException();
                        return;
                    }
                    if (edgeCA.PointBelongsToEdge(splitterPoint))
                    {
                        AddTriangleAndSplitIfNeeded(buffer, a, b, splitter, splitters, vertices);
                        AddTriangleAndSplitIfNeeded(buffer, splitter, b, c, splitters, vertices);
                        //throw new NotImplementedException();
                        return;
                    }
                }
            }

            AddTriangle(buffer, a, b, c);
        }

        private static void AddTriangle(IList<int> buffer, int a, int b, int c)
        {
            buffer.Add(a);
            buffer.Add(b);
            buffer.Add(c);
        }

    }
}
