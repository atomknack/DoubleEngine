using CollectionLike.Pooled;
using Collections.Pooled;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DoubleEngine
{

    public static partial class MeshUtil
    {
        //public static (List<TVertice>, List<int>)
        //  old_testingOnly_RemoveUnusedFacesAndVertices<TVertice>(
        //  IReadOnlyList<int> _triangles,
        //  IReadOnlyList<TVertice> _vertexes,
        //  IEnumerable<int> newMeshFaceIdexes) where TVertice : struct

        //public static (List<TVertice>, List<int>) old_testingOnly_RemoveUnusedFacesAndVertices<TVertice>(ROSpanInt triangles, ReadOnlySpan<TVertice> vertexes, IEnumerable<int> newMeshFaceIdexes) where TVertice : struct


        public static (TVertice[], int[]) RemoveUnusedVerticesAndFacesAndReturnNewArraysTriangles<TVertice>(
            ReadOnlySpan<TVertice> vertices, ReadOnlySpan<IndexedTri> faces, IEnumerable<int> usedFaceIndexes) where TVertice : struct
        {
            (var pooledVertices, var pooledTriangles) = RemoveUnusedVerticesAndFacesAndReturnPooled(vertices, faces, usedFaceIndexes);
            using (pooledVertices)
            {
                using (pooledTriangles)
                {
                    return (pooledVertices.ToArray(), pooledTriangles.AsReadOnlySpan().CastToInt_ReadOnlySpan().ToArray());
                }
            }
        }

        public static (PooledArrayStruct<TVertice>, PooledArrayStruct<IndexedTri>) RemoveUnusedVerticesAndReturnPooledArrays<TVertice>(
            ReadOnlySpan<TVertice> vertices, ReadOnlySpan<IndexedTri> faces) where TVertice : struct
        {
            var resultFaces = Expendables.CreateArray<IndexedTri>(faces.Length);
            var resultVertices = UnusedVerticeRemover.RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_Span(
                resultFaces.AsReadOnlySpan(), resultFaces.AsSpan(), vertices);
            return (resultVertices, resultFaces);
        }
        public static (PooledArrayStruct<TVertice>, PooledList<IndexedTri>) RemoveUnusedVerticesAndFacesAndReturnPooled<TVertice>(
    ReadOnlySpan<TVertice> vertices, ReadOnlySpan<IndexedTri> faces, IEnumerable<int> usedFaceIndexes) where TVertice : struct
        {

            var resultFaces = RemoveUnusedFaces(faces, usedFaceIndexes);
            var resultVertices = UnusedVerticeRemover.RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_Span(
                resultFaces.AsReadOnlySpan(), resultFaces.AsSpan(), vertices);
            return (resultVertices, resultFaces);
        }

        public static void AddRemappedNonDegenerateFacesToBuffers(ReadOnlySpan<int> newVerticeIndices, ReadOnlySpan<IndexedTri> faces, ReadOnlySpan<byte> faceMaterials, PooledList<IndexedTri> newFacesBuffer, PooledList<byte> newFacesMaterialsBuffer)
        {
            for (int i = 0; i < faces.Length; ++i)
            {
                var face = faces[i];
                IndexedTri newFace = new IndexedTri(newVerticeIndices[face.v0], newVerticeIndices[face.v1], newVerticeIndices[face.v2]);
                if (newFace.NotDegenerate())
                {
                    newFacesBuffer.Add(newFace);
                    newFacesMaterialsBuffer.Add(faceMaterials[i]);
                }
            }
        }
        public static void AddRemappedNonDegenerateFacesToBuffer(ReadOnlySpan<int> newVerticeIndices, ReadOnlySpan<IndexedTri> faces, PooledList<IndexedTri> newFacesBuffer)
        {
            for (int i = 0; i < faces.Length; ++i)
            {
                var face = faces[i];
                IndexedTri newFace = new IndexedTri(newVerticeIndices[face.v0], newVerticeIndices[face.v1], newVerticeIndices[face.v2]);
                if (newFace.NotDegenerate())
                {
                    newFacesBuffer.Add(newFace);
                }
            }
        }

        public static PooledSet<int> SelectVertices<TVertice>(this ReadOnlySpan<TVertice> vertices, Func<TVertice, bool> s) where TVertice : struct
        {
            if (vertices == null)
                throw new ArgumentNullException("vertices[] is null");
            PooledSet<int> set = Expendables.CreateSet<int>(vertices.Length / 4);
            for (var i = 0; i < vertices.Length; i++)
            {
                if (s(vertices[i]))
                    set.Add(i);
            }
            return set;
        }

        public static PooledList<int> SelectFacesWhereAllVerticesInSet(this ReadOnlySpan<int> triangles, PooledSet<int> verticesIndexes)
        {
            if (triangles == null)
                throw new ArgumentNullException("triangles[] is null");
            if (verticesIndexes == null)
                throw new ArgumentNullException("verticesIndexes[] is null");
            PooledList<int> list = Expendables.CreateList<int>(triangles.Length / 3);
            var f = 0;
            for (var i = 0; i < triangles.Length; i += 3)
            {
                if (verticesIndexes.Contains(triangles[i]) && verticesIndexes.Contains(triangles[i + 1]) && verticesIndexes.Contains(triangles[i + 2]))
                {
                    list.Add(f);
                    //list.Add(triangles[i]);
                    //list.Add(triangles[i + 1]);
                    //list.Add(triangles[i + 2]);
                }
                f++;
            }
            return list;
        }
    }

}


/*
//manual Tested, looks working, TODO: Test more, automate tests
public static (int[] remapedVerticeIndexes, int[] newTriangles) CreatePartialMesh(int[] triangles, int[] newMeshFaceIdexes = null)
{
    if (triangles.Length % 3 != 0)
        throw new ArgumentException("Every triangle must contain 3 vertices");
    List<int> newVertices = new List<int>();
    Dictionary<int, int> oldNewVerticesMap = new Dictionary<int, int>();
    int newVerticesCount = 0;
    List<int> newTriangles = new List<int>();
    if (newMeshFaceIdexes != null)
        foreach (int faceIndex in newMeshFaceIdexes)
        {
            AddTriangleVertice(triangles[faceIndex * 3 + 0]);
            AddTriangleVertice(triangles[faceIndex * 3 + 1]);
            AddTriangleVertice(triangles[faceIndex * 3 + 2]);
        }
    else
        for (var vertIndex = 0; vertIndex < triangles.Length; vertIndex++)
            AddTriangleVertice(triangles[vertIndex]);

    return (newVertices.ToArray(), newTriangles.ToArray());

    void AddTriangleVertice(int oldVerticeIndex)
    {
        if (oldNewVerticesMap.ContainsKey(oldVerticeIndex) == false)
        {
            newVertices.Add(oldVerticeIndex);
            oldNewVerticesMap.Add(oldVerticeIndex, newVerticesCount);
            newVerticesCount++;
        }
        newTriangles.Add(oldNewVerticesMap[oldVerticeIndex]);
    }
}

public static PooledArrayStruct<int> RemapTriangles(ReadOnlySpan<int> oldTriangles, ReadOnlySpan<int> newVerticeIndexes)
{
    PooledArrayStruct<int> newTrianglesArr = Expendables.CreateArray<int>(oldTriangles.Length);// new int[oldTriangles.Length];
    Span<int> newTriangles = newTrianglesArr.AsSpan();
    for (int i = 0; i < oldTriangles.Length; i++)
    {
        newTriangles[i] = newVerticeIndexes[oldTriangles[i]];
    }
    return newTrianglesArr;
}
*/
/*
public static int[] SingleEdgesToArray((int start, int end)[] singleEdges)
{
int[] result = new int[singleEdges.Length*2];
for (int i = 0; i < singleEdges.Length; i++)
{
result[(i*2)] = singleEdges[i].start;
result[(i*2)+1] = singleEdges[i].end;
}
return result;
}
public static (int start, int end)[] SingleEdges(this IReadOnlyList<int> triangles)
{
Dictionary<(int start, int end), int> edgesCount = new();

for (var i = 0; i < triangles.Count; i += 3)
{
AddEdge(triangles[i], triangles[i + 1]);
AddEdge(triangles[i+1], triangles[i + 2]);
AddEdge(triangles[i + 2], triangles[i]);
}
//foreach (var keyValue in edgesCount)
//    Debug.Log($"{keyValue.Key.a} {keyValue.Key.b} {keyValue.Value}");
var filtered = edgesCount.Where(keyvaluePair => keyvaluePair.Value == 1); // make single lined
var onlyKeys = filtered.Select(keyvaluePair => keyvaluePair.Key); // make single lined
return onlyKeys.ToArray(); // make single lined

void AddEdge(int start, int end)
{
var edge = (start, end);
if (edgesCount.ContainsKey(edge))
{
edgesCount[edge]++;
return;
}
var edgeBackwards = (end, start);
if (edgesCount.ContainsKey(edgeBackwards))
{
edgesCount[edgeBackwards]++;
return;
}
edgesCount.Add(edge, 1);
}

}*/

//Not Tryed, Not Tested. TODO: Try, Test
/*public static int[] FindFacesWithAllVertices(HashSet<int> verticesIndexes, int[] triangles)
{
    List<int> faces = new List<int>();
    for(var i=0; i < triangles.Length; i += 3)
        if( verticesIndexes.Contains(triangles[i]) && verticesIndexes.Contains(triangles[i+1]) && verticesIndexes.Contains(triangles[i+2]) )
            faces.Add(i);
    return faces.ToArray();
}*/