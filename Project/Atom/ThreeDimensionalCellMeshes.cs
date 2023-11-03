using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom;


public static partial class ThreeDimensionalCellMeshes
{
    private static IntactCubeMesh[] _intactMeshes;
    private static DeshelledCubeMesh[] _deshelledMeshes;

    public static ReadOnlySpan<IntactCubeMesh> IntactMeshes => _intactMeshes;
    public static ReadOnlySpan<DeshelledCubeMesh> DeshelledCubeMeshes => _deshelledMeshes;

    public static short GetCount() => 
        (short)_deshelledMeshes.Length;

    public static DeshelledCubeMesh GetDeshelled(short index) => _deshelledMeshes[index];

    //flatTransform.Transform(applyTransformation)
    public static int AddCellMesh(IntactCubeMesh intact)
    {
        int newMeshIndex = _intactMeshes.Length;
        IntactCubeMesh[] newIntactMeshes = new IntactCubeMesh[_intactMeshes.Length+1];
        Array.Copy(_intactMeshes, newIntactMeshes, _intactMeshes.Length);
        newIntactMeshes[newMeshIndex] = intact;
        _intactMeshes = newIntactMeshes;
        _deshelledMeshes = DeshelledCubeMeshesFromIntact(_intactMeshes);
        return newMeshIndex;
    }

    private static DeshelledCubeMesh[] DeshelledCubeMeshesFromIntact(ReadOnlySpan<IntactCubeMesh> intacts)
    {
        DeshelledCubeMesh[] result = new DeshelledCubeMesh[intacts.Length];
        for (int i = 0; i < intacts.Length; ++i)
        {
            //Debug.Log(i);
            result[i] = DeshelledCubeMesh.Create(intacts[i]);
        }
        return result;
    }

    static ThreeDimensionalCellMeshes()
    {
        //StaticConstructorInitIntactMeshesFromJson();
        ReloadInternal();
    }
    /// <summary>
    /// Call directly only internally, Use Helpers if need to reload.
    /// </summary>
    public static void ReloadInternal()
    {
        //try
        //{
            _intactMeshes = ReloadIntactsFromJson(Loaders.EngineLoader.LoadTDCellMeshes());
        //}
        //catch
        //{
        //    _intactMeshes = SetIntactMeshesForEmptySpaceAndFullyFilledCube(_intactMeshes);
        //}

        RecalculateAllFieldsBasedOnIntact();
    }
    /*
public static void StaticConstructorInitIntactMeshesFromJson(string filename = null)
{
try
{
    if (filename == null)
        _intactMeshes = LoadIntactsFromJsonFile();
    else
        _intactMeshes = LoadIntactsFromJsonFile(filename);
}
catch
{
    _intactMeshes = SetIntactMeshesForEmptySpaceAndFullyFilledCube(_intactMeshes);
}

RecalculateAllFieldsBasedOnIntact();
}*/


    private static void RecalculateAllFieldsBasedOnIntact()
    {
        _deshelledMeshes = DeshelledCubeMeshesFromIntact(_intactMeshes);
    }



    public static short ValidateMeshId(short meshId)
    {
        int count = GetCount();
        if (meshId >= count)
            throw new Exception($"mesh id {meshId} should be bigger than -1 and less than {count}");
        return meshId;
    }

    public static short EmptySpaceId => 0;
    public static short FullCubeId => 1;
}
