using DoubleEngine.Atom;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//MeshUtil located at DoubleEngine.MeshUtil
namespace DoubleEngine.UHelpers;

public static class UMeshUtil
{
    public static Mesh ToNewUnityMesh(this IMeshFragment<Vec3D> m)
    {
        Mesh mesh = new Mesh();
        UpdateUnityMesh(m, mesh);
        return mesh;
    }
    public static Mesh ToNewUnityMesh(this IMeshFragmentWithMaterials<Vec3D> m, ReadOnlySpan<Color32> albedos = default(ReadOnlySpan<Color32>))
    {
        Mesh mesh = new Mesh();
        UpdateUnityMesh(m, mesh, albedos);
        return mesh;
    }
    public static void UpdateUnityMesh(this IMeshFragment<Vec3D> m, Mesh unityMesh)
    {
        if (unityMesh == null)
            throw new ArgumentNullException(nameof(unityMesh));
        unityMesh.Clear();
        unityMesh.vertices = m.Vertices.ToArrayVector3();
        unityMesh.triangles = m.Triangles.ToArray();
    }
    public static void UpdateUnityMesh(this IMeshFragmentWithMaterials<Vec3D> mesh, Mesh unityMesh, ReadOnlySpan<Color32> albedos = default(ReadOnlySpan<Color32>))
    {
        if (unityMesh == null)
            throw new ArgumentNullException(nameof(unityMesh));
        //Debug.Log(albedos.Length);
        if (albedos.Length == 0)
            albedos = UMaterials.UnsafeAlbedos();
        //Debug.Log(albedos.Length);
        //Debug.Log(JsonConvert.SerializeObject(materials));
        //Debug.Log(String.Join(',', faceMaterials));
        unityMesh.Clear();

        var vertices = mesh.Vertices;
        var triangles = mesh.Triangles;
        if (triangles.Length > 65000)
        {
            unityMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }

        var faces = mesh.Faces;
        var faceMaterials = mesh.FaceMaterials;
        int numUnityVertices = triangles.Length;
        Vector3[] unityVertices = new Vector3[numUnityVertices];
        Vector3[] unityNormals = new Vector3[numUnityVertices];
        Color32[] unityVerticeColors = new Color32[numUnityVertices];
        int[] unityTriangles = new int[triangles.Length];
        int last = 0;
        for (int i = 0; i < faces.Length; ++i)
        {
            unityTriangles[last] = last;
            unityTriangles[last + 1] = last + 1;
            unityTriangles[last + 2] = last + 2;
            var face = faces[i];
            unityVertices[last] = vertices[face.v0].ToVector3();
            unityVertices[last + 1] = vertices[face.v1].ToVector3();
            unityVertices[last + 2] = vertices[face.v2].ToVector3();
            var normal = faces[i].Normal(vertices).ToVector3();
            unityNormals[last] = normal;
            unityNormals[last + 1] = normal;
            unityNormals[last + 2] = normal;
            var facecolor = albedos[faceMaterials[i]];
            unityVerticeColors[last] = facecolor;
            unityVerticeColors[last + 1] = facecolor;
            unityVerticeColors[last + 2] = facecolor;
            last += 3;
        }
        unityMesh.SetVertices(unityVertices, 0, numUnityVertices);// unityMesh.vertices = unityVertices;
        unityMesh.SetNormals(unityNormals, 0, numUnityVertices);//unityMesh.normals = normals;
        unityMesh.SetColors(unityVerticeColors, 0, numUnityVertices);//unityMesh.colors32 = verticeColors;
        unityMesh.SetTriangles(unityTriangles, 0, triangles.Length, 0);//unityMesh.triangles = unityTriangles;
        //Debug.Log(JsonConvert.SerializeObject(colors));
    }
}
