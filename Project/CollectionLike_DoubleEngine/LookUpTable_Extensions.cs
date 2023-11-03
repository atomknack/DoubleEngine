using System;

namespace CollectionLike.Pooled;

public static class LookUpTable_Extensions
{
    /*[Obsolete("need testing")]
    public static void AddIndividualNormals(this LookUpTable<Vec3D> normals, ReadOnlySpan<IndexedTri> faces, ReadOnlySpan<Vec3D> vertices)
    {
        foreach (var face in faces)
        {
            var normal = face.Normal(vertices);
            normals.AddItem(face.v0, normal);
            normals.AddItem(face.v1, normal);
            normals.AddItem(face.v2, normal);
        }
    }*/
    [Obsolete("need testing")]
    public static void AddVerticeMaterial(this LookUpTable<byte> materials, byte material, ReadOnlySpan<IndexedTri> faces)
    {
        foreach (var face in faces)
        {
            materials.AddItem(face.v0, material);
            materials.AddItem(face.v1, material);
            materials.AddItem(face.v2, material);
        }
    }
}
