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
        internal static EdgeIndexed CreateIncreasingEdge(int start, int end)
        {
            if (start<end)
                return new EdgeIndexed(start, end);
            return new EdgeIndexed(end, start);
        }
        internal static bool IsEdgeIncreasing(EdgeIndexed edge) =>
            IsEdgeIncreasing(edge.start, edge.end);
        internal static bool IsEdgeIncreasing(int start, int end) =>
            start < end;

        public ref struct MultiMaterialEdges
        {
            private PooledSet<EdgeIndexed> _multiMaterialEdges;

            public EdgeIndexed[] TESTING_EdgesAsArray() => _multiMaterialEdges.ToArray();


            public bool IsEdgeMultiMaterial(int start, int end)=>
                _multiMaterialEdges.Contains(CreateIncreasingEdge(start,end));
            public MultiMaterialEdges(ReadOnlySpan<IndexedTri> faces, ReadOnlySpan<MaterialByte> faceMaterials)
            {
                _multiMaterialEdges = Expendables.CreateSet<EdgeIndexed>(faces.Length * 3);
                using var pooledDict = new PooledDictionary<EdgeIndexed, MaterialByte>(faces.Length * 3);
                for(int i = 0; i < faces.Length; i++)
                {
                    IndexedTri face = faces[i];
                    MaterialByte material = faceMaterials[i];
                    AddEdge(pooledDict, face.v0, face.v1, material);
                    AddEdge(pooledDict, face.v1, face.v2, material);
                    AddEdge(pooledDict, face.v2, face.v0, material);
                }
            }
            private readonly void AddEdge(PooledDictionary<EdgeIndexed, byte> pooledDict, int start, int end, MaterialByte faceMaterial)
            {
                EdgeIndexed edge = CreateIncreasingEdge(start, end);
                if (pooledDict.TryGetValue(edge, out MaterialByte material))
                {
                    if (material != faceMaterial)
                        _multiMaterialEdges.Add(edge);
                    return;
                }
                pooledDict.Add(edge, faceMaterial);
            }

            public void Dispose()
            {
                _multiMaterialEdges.Dispose();
                //throw new NotImplementedException();
            }
        }
    }
}
