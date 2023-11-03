using System;
using System.Collections.Generic;
using CollectionLike.Comparers;
using CollectionLike.Comparers.SetsWithCustomComparer;
using Collections.Pooled;

namespace CollectionLike.Pooled;

public static class ExpendablesDoubleEngineSpecific
{
    public static LookUpInt CreateConnectionsLookUp(int verticesCount, PooledDictionary<EdgeIndexed, int> edgesCount)
    {
        LookUpInt connections = new LookUpTable<int>(verticesCount, Expendables.IntComparer, verticesCount * 3);
        foreach (var edge in edgesCount.Keys)
        {
            connections.AddItem(edge.start, edge.end);
            connections.AddItem(edge.end, edge.start);
        }
        return connections;
    }
    public static bool ContainsVec3D_2em5(this PooledList<int> listOfIndices, Vec3D vertice, ReadOnlySpan<Vec3D> vertices) =>
        listOfIndices.Span.ContainsVec3D_2em5(vertice, vertices);

    public static LookUpTable<Vec3D> CreateEmptyLookUpForNormals(ReadOnlySpan<Vec3D> vertices) =>
        CreateEmptyLookUpForNormals(vertices.Length);
    public static LookUpTable<Vec3D> CreateEmptyLookUpForNormals(int verticesLength) =>
        new LookUpTable<Vec3D>(verticesLength, Vec3DComparer_2em5.StaticEquals, verticesLength * 2 + 10);

    //TODO split normals Array creation and filling Span with normals
    public static PooledArrayStruct<Vec3D> CreateFaceNormalsFromMeshFragment3D(IMeshFragment<Vec3D> mesh)
    {
        var vertices = mesh.Vertices;
        var faces = mesh.Faces;
        int facesLength = faces.Length;
        var result  = Expendables.CreateArray<Vec3D>(facesLength);
        for (int i = 0; i < facesLength; ++i)
        {
            result[i] = faces[i].Normal(vertices);
        }
        return result;
    }
    /*
    public static LookUpTable<Vec3D> CreateNormalsLookUpFromMeshFragment3D(MeshFragmentVec3D mesh)
    {
        var result = CreateEmptyLookUpForNormals(mesh.Vertices);
        result.AddIndividualNormals(mesh.Faces, mesh.Vertices);
        return result;
    }*/

public static LookUpTable<Vec3D> CreateVerticeToNormalsLookUp(int verticesLength, ReadOnlySpan<IndexedTri> faces, PooledArrayStruct<Vec3D> facesNormals)
{
    var result = CreateEmptyLookUpForNormals(verticesLength);
    for (int i = 0; i < faces.Length; ++i)
    {
        var normal = facesNormals[i];
        var face = faces[i];
        result.AddItem(face.v0, normal);
        result.AddItem(face.v1, normal);
        result.AddItem(face.v2, normal);
    }
    return result;
}
public static LookUpTable<MaterialByte> CreateVerticeToMaterialsLookUp(int verticesLength, ReadOnlySpan<IndexedTri> faces, ReadOnlySpan<MaterialByte> facesMaterials)
{
    var result = Expendables.CreateEmptyLookUpForMaterials(verticesLength);
    for (int i = 0; i < faces.Length; ++i)
    {
        var material = facesMaterials[i];
        var face = faces[i];
        result.AddItem(face.v0, material);
        result.AddItem(face.v1, material);
        result.AddItem(face.v2, material);
    }
    return result;
}
[Obsolete("need testing")]
public static LookUpTable<int> CreateVerticeToFacesLookUp(int verticeLength, ReadOnlySpan<IndexedTri> faces)
{
    var result = new LookUpTable<int>(verticeLength, Expendables.IntComparer, faces.Length * 3 + 1000);
    for (int i = 0; i < faces.Length; ++i)
    {
        var face = faces[i];
        result.AddItem(face.v0, i);
        result.AddItem(face.v1, i);
        result.AddItem(face.v2, i);
    }
    return result;
}

}