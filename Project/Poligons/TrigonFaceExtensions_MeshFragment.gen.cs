
//this file was generated

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DoubleEngine
{

public static class TrigonExtensions_MeshFragment
    {


    //    public static bool IsTriangleClockwise(this TriVec2D face)
    //        => Vector2dUtil.IsTriangleClockwise(face.v0, face.v1, face.v2);
    /*
    public static IEnumerable<TriVec3D> Tris(this MeshFragmentVec3D mesh)
    {
        for (int i = 0; i < mesh.triangles.Length; i += 3)
            yield return new TriVec3D(mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i + 2]]);
    }
    */
    public static IEnumerable<TriVec3D> Tris(this IEnumerable<MeshFragmentVec3D> meshes)
    {
        foreach (var mesh in meshes)
            foreach (var tri in mesh.Tris())//(var face in Faces(mesh))
                yield return tri;
    }

    public static IEnumerable<TriVec2D> ConvertFaces_Vec3DToVec2D(this IEnumerable<TriVec3D> faces)
    {
        foreach (var face in faces)
            yield return face.ToTriVec2D();
    }

    [Obsolete("Need testing")]
    public static TriVec3D[] FacesArray(this MeshFragmentVec3D mesh)
    {
        Vec3D[] vs = mesh.vertices;
        int[] tris = mesh.triangles;
            TriVec3D[] result = new TriVec3D[tris.Length / 3];
        for ((int f, int tr) = (0,0); f < result.Length; f++, tr+=3)
            result[f] = new TriVec3D( vs[tris[tr]],  vs[tris[tr+1]], vs[tris[tr+2]]);
        return result;
    } 


    }
}