using System;

namespace DoubleEngine.Atom;

public partial class ThreeDimensionalChunker
{
    public MeshVolatileFragmentWithMaterials BuildFullMeshDecimated()
    {
        (var builder, var offset) = MakeTemporaryBuilderAndBuild();
        MeshVolatileFragmentWithMaterials violatileMesh = DecimatorColored.DecimateAndReturnViolatileFragment(builder);
        violatileMesh.Translate(Vec3D.FromVec3I(offset));
        builder.Dispose();
        return violatileMesh;
    }

    public MeshVolatileFragmentWithMaterials BuildFullMeshNotDecimated()
    {
        (var builder, var offset) = MakeTemporaryBuilderAndBuild();
        MeshVolatileFragmentWithMaterials violatileMesh = MeshVolatileFragmentWithMaterials.CreateByCopyingFrom(builder);
        violatileMesh.Translate(Vec3D.FromVec3I(offset));
        builder.Dispose();
        return violatileMesh;
    }

    private (ThreeDimensionalBuilder builder, Vec3I offset) MakeTemporaryBuilderAndBuild()
    {
        var offsetter = ThreeDimensionalGridOffsetter.CreateClone(_offsetter);
        var tempOutOffset = offsetter.GetOutOffset();
        offsetter.SetOutOffset(Vec3I.zero);
        ThreeDimensionalBuilder builder = ThreeDimensionalBuilder.Create(_sizeX, _sizeY, _sizeZ);
        builder.Build(offsetter);
        return (builder, tempOutOffset);
    }

}
