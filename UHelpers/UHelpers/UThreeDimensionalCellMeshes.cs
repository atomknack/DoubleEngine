using DoubleEngine.Atom;
using System;
using UnityEngine;

namespace DoubleEngine.UHelpers;

public static class UThreeDimensionalCellMeshes
{
    private static Mesh[] _unityMeshes;
    public static Mesh GetUnityMesh(short index) 
    {
        if (_unityMeshes == null || _unityMeshes.Length <= index)
            RecalculateAllFieldsBasedOnIntact();
        return _unityMeshes[index];
    }


    private static Mesh[] UnityMeshesFromIntact(ReadOnlySpan<IntactCubeMesh> intacts)
    {
        Mesh[] result = new Mesh[intacts.Length];
        for (int i = 0; i < intacts.Length; ++i)
            result[i] = ((ISerializableCubeMesh)intacts[i]).Mesh.ToNewUnityMesh();
        return result;
    }

    public static void Reload()
    {
        ThreeDimensionalCellMeshes.ReloadInternal();
        RecalculateAllFieldsBasedOnIntact();
    }
    static UThreeDimensionalCellMeshes()
    {
        RecalculateAllFieldsBasedOnIntact();
    }

    private static void RecalculateAllFieldsBasedOnIntact()
    {
        _unityMeshes = UnityMeshesFromIntact(ThreeDimensionalCellMeshes.IntactMeshes);
    }

}
