/*
#if TESTING
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    [Obsolete("use ThreeDimensionalBuilder instead")]
    public sealed class ThreeDimensionalGrid_WithLayeredBuilder: IThreeDimensionalGrid
    {
        private ThreeDimensionalGridBase _grid;
        private InternalLayeredBuilderWithMaterials _builder;

        public static ThreeDimensionalGrid_WithLayeredBuilder Create(int lengthX, int lengthY, int lengthZ)
        {
            ThreeDimensionalGrid_WithLayeredBuilder created = new ThreeDimensionalGrid_WithLayeredBuilder();
            created._grid = ThreeDimensionalGridBase.Create(lengthX, lengthY, lengthZ);
            created._builder = InternalLayeredBuilderWithMaterials.Create(lengthX, lengthY, lengthZ);
            return created;
        }
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell) => _grid.UpdateCell(x, y, z, cell);
        public ThreeDimensionalCell GetCell(int x, int y, int z) => _grid.GetCell(x, y, z);
        public Vec3I GetDimensions() => _grid.GetDimensions();
        public MeshFragmentVec3D BuildMesh()
        {
            return _builder.Build(_grid);
        }
        public MeshFragmentVec3DWithMaterials BuildMeshWithMaterials()
        {
            _builder.Build(_grid);
            return _builder.BuildWithMaterials();
        }
    }
}
#endif
*/