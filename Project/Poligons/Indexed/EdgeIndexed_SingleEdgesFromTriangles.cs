using CollectionLike.Pooled;
using Collections.Pooled;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DoubleEngine
{
    public readonly partial struct EdgeIndexed
    {
        public static EdgeIndexed[] SingleEdgesFromTriangles(ReadOnlySpan<int> triangles)
        {
            using PooledDictionary<EdgeIndexed, int> edgesCount = EdgesCountDirectionIgnore(triangles);
            //var onlyKeys = edgesCount.Where(keyvaluePair => keyvaluePair.Value == 1).Select(keyvaluePair => keyvaluePair.Key); // make single lined
            return SingleEdgesFromCountingDictionary(edgesCount).ToArray(); // todo make single lined
        }
        public static IEnumerable<EdgeIndexed> SingleEdgesFromCountingDictionary(PooledDictionary<EdgeIndexed, int> edgesCount) =>
            edgesCount.Where(keyvaluePair => keyvaluePair.Value == 1).Select(keyvaluePair => keyvaluePair.Key);

        public static PooledDictionary<EdgeIndexed,int> EdgesCountDirectionIgnore(ReadOnlySpan<int> triangles)
        {
            PooledDictionary<EdgeIndexed,int> edgesCount = Expendables.CreateDictionary<EdgeIndexed,int>(triangles.Length);
            AddTriangleEdgesToCountingDictionary(edgesCount, triangles);
            return edgesCount;
        }

        public static void AddTriangleEdgesToCountingDictionary(PooledDictionary<EdgeIndexed, int> edgesCount, ReadOnlySpan<int> triangles)
        {
            for (var i = 0; i < triangles.Length; i += 3)
            {
                AddEdge(triangles[i], triangles[i + 1]);
                AddEdge(triangles[i + 1], triangles[i + 2]);
                AddEdge(triangles[i + 2], triangles[i]);
            }

            void AddEdge(int start, int end)
            {
                var edge = new EdgeIndexed(start, end);
                if (edgesCount.ContainsKey(edge))
                {
                    edgesCount[edge]++;
                    return;
                }
                var edgeBackwards = edge.Backwards();
                if (edgesCount.ContainsKey(edgeBackwards))
                {
                    edgesCount[edgeBackwards]++;
                    return;
                }
                edgesCount.Add(edge, 1);
            }
        }

        public static IEnumerable<(EdgeIndexed, MaterialByte)> SingleEdgesWithMaterialsFromCountingDictionary(
            PooledDictionary<(EdgeIndexed, MaterialByte), int> edgesCount) =>
            edgesCount.Where(keyvaluePair => keyvaluePair.Value == 1).Select(keyvaluePair => keyvaluePair.Key);
        public static PooledDictionary<(EdgeIndexed, MaterialByte), int> EdgesCountWithMaterialsDirectionIgnore(
            ReadOnlySpan<IndexedTri> faces,
            ReadOnlySpan<MaterialByte> facesMaterials)
        {
            PooledDictionary<(EdgeIndexed, MaterialByte), int> edgesCount = 
                Expendables.CreateDictionary<(EdgeIndexed, MaterialByte), int>(faces.Length*3);
            AddTriangleEdgesWithMaterialsToCountingDictionary(edgesCount, faces, facesMaterials);
            return edgesCount;
        }
        public static void AddTriangleEdgesWithMaterialsToCountingDictionary(
            PooledDictionary<(EdgeIndexed, MaterialByte), int> edgesCount, 
            ReadOnlySpan<IndexedTri> faces, 
            ReadOnlySpan<MaterialByte> facesMaterials)
        {
            for (var i = 0; i < faces.Length; ++i)
            {
                var face = faces[i];
                var material = facesMaterials[i];
                AddEdge(face.v0, face.v1, material);
                AddEdge(face.v1, face.v2, material);
                AddEdge(face.v2, face.v0, material);
            }

            void AddEdge(int start, int end, MaterialByte material)
            {
                var edge = new EdgeIndexed(start, end);
                if (edgesCount.ContainsKey((edge, material)))
                {
                    edgesCount[(edge, material)]++;
                    return;
                }
                var edgeBackwards = edge.Backwards();
                if (edgesCount.ContainsKey((edgeBackwards, material)))
                {
                    edgesCount[(edgeBackwards, material)]++;
                    return;
                }
                edgesCount.Add((edge, material), 1);
            }
        }
    }
}
