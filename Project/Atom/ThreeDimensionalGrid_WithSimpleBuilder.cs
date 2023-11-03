#if TESTING
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public sealed class ThreeDimensionalGrid_WithSimpleBuilder: IThreeDimensionalGrid
    {
        private ThreeDimensionalGridBase grid;

        public static ThreeDimensionalGrid_WithSimpleBuilder Create(int lengthX, int lengthY, int lengthZ)
        {
            ThreeDimensionalGrid_WithSimpleBuilder created = new ThreeDimensionalGrid_WithSimpleBuilder();
            created.grid = ThreeDimensionalGridBase.Create(lengthX, lengthY, lengthZ);
            return created;
        }
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell) => grid.UpdateCell(x, y, z, cell);
        public ThreeDimensionalCell GetCell(int x, int y, int z) => grid.GetCell(x, y, z);
        public Vec3I GetDimensions() => grid.GetDimensions();
        public MeshFragmentVec3D BuildMesh()
        {
            using MeshBuilderVec3D builder = new MeshBuilderVec3D();
            for (int yi = 0; yi < grid._yLength; ++yi)
                for (int xi = 0; xi < grid._xLength; ++xi)
                    for (int zi = 0; zi < grid._zLength; ++zi)
                        AddExistingCellToMeshBuilder(xi, yi, zi, builder);
            return builder.BuildFragment();
        }

        private void AddExistingCellToMeshBuilder(int x, int y, int z, MeshBuilderVec3D builder)
        {
            ThreeDimensionalCell cell = grid.GetCellNoCheck(x, y, z);
            DeshelledCubeMesh mesh = ThreeDimensionalCellMeshes.GetDeshelled(cell.meshID);
            Grid6SidesCached orientation = Grid6SidesCached.GetCached(cell.grid6SidesCachedIndex);
            Vec3D translation = new Vec3D(x, y, z);
            if (orientation._invertedY)
                builder.AddMeshFragment(mesh.core, orientation.Scale, orientation._rotation, translation);
            else
                builder.AddMeshFragment(mesh.core, orientation._rotation, translation);

            builder.AddMeshFragment(mesh.core.Scaled(new Vec3D(1f, orientation._invertedY ? -1f : 1f, 1f)), orientation._rotation, translation);
            builder.AddMeshFragment(mesh.ZNegativeFlatNode(orientation).Transformed, AngleMethods.ZNegToZNeg, translation);
            builder.AddMeshFragment(mesh.XNegativeFlatNode(orientation).Transformed, AngleMethods.ZNegToXNeg, translation);
            builder.AddMeshFragment(mesh.YNegativeFlatNode(orientation).Transformed, AngleMethods.ZNegToYNeg, translation);
            builder.AddMeshFragment(mesh.ZPositiveFlatNode(orientation).Transformed, AngleMethods.ZNegToZPos, translation);
            builder.AddMeshFragment(mesh.YPositiveFlatNode(orientation).Transformed, AngleMethods.ZNegToYPos, translation);
            builder.AddMeshFragment(mesh.XPositiveFlatNode(orientation).Transformed, AngleMethods.ZNegToXPos, translation);
        }
    }
}
#endif