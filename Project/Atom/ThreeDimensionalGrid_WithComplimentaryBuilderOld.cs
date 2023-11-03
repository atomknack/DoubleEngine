#if TESTING
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public sealed class ThreeDimensionalGrid_WithComplimentaryBuilderOld : IThreeDimensionalGrid
    {
        private IThreeDimensionalGrid grid;
        private ThreeDimensionalCell[] cellBuffer;

        public static ThreeDimensionalGrid_WithComplimentaryBuilderOld Create(int lengthX, int lengthY, int lengthZ)
        {
            ThreeDimensionalGrid_WithComplimentaryBuilderOld created = new ThreeDimensionalGrid_WithComplimentaryBuilderOld();
            created.grid = ThreeDimensionalGridBase.Create(lengthX, lengthY, lengthZ);
            created.cellBuffer = new ThreeDimensionalCell[6];
            return created;
        }
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell) => grid.UpdateCell(x, y, z, cell);
        public ThreeDimensionalCell GetCell(int x, int y, int z) => grid.GetCell(x, y, z);
        public Vec3I GetDimensions() => grid.GetDimensions();
        public MeshFragmentVec3D BuildMesh()
        {
            (int xSize, int ySize, int zSize) = grid.GetDimensions();
            using MeshBuilderVec3D builder = new MeshBuilderVec3D();
            for (int yi = 0; yi < ySize; ++yi)
                for (int xi = 0; xi < xSize; ++xi)
                    for (int zi = 0; zi < zSize; ++zi)
                        AddExistingCellToMeshBuilder(xi, yi, zi, builder);
            return builder.BuildFragment();
        }

        /*        internal ThreeDimensionalCell GetCellAndNeighbours(int x, int y, int z, Span<ThreeDimensionalCell> bufferForReturnedNeighbours)
        {
            //public enum SideEnum zNegative = 0,xNegative = 1,yNegative = 2,zPositive = 3,xPositive = 4,yPositive = 5
            bufferForReturnedNeighbours[(int)Grid6Sides.SideEnum.zNegative] = GetCell(x, y, z - 1);
        bufferForReturnedNeighbours[(int)Grid6Sides.SideEnum.xNegative] = GetCell(x - 1, y, z);
        bufferForReturnedNeighbours[(int)Grid6Sides.SideEnum.yNegative] = GetCell(x, y - 1, z);
        bufferForReturnedNeighbours[(int)Grid6Sides.SideEnum.zPositive] = GetCell(x, y, z + 1);
        bufferForReturnedNeighbours[(int)Grid6Sides.SideEnum.xPositive] = GetCell(x + 1, y, z);
        bufferForReturnedNeighbours[(int)Grid6Sides.SideEnum.yPositive] = GetCell(x, y + 1, z);
            return GetCell(x, y, z);
    }
        */

    private void AddExistingCellToMeshBuilder(int x, int y, int z, MeshBuilderVec3D builder)
        {
            ThreeDimensionalCell cell = grid.GetCellAndNeighbours(x, y, z, cellBuffer);
            DeshelledCubeMesh mesh = ThreeDimensionalCellMeshes.GetDeshelled(cell.meshID);
            Grid6SidesCached orientation = Grid6SidesCached.GetCached(cell.grid6SidesCachedIndex);
            Vec3D translation = new Vec3D(x, y, z);
            if (orientation._invertedY)
                builder.AddMeshFragment(mesh.core, orientation.Scale, orientation._rotation, translation);
            else
                builder.AddMeshFragment(mesh.core, orientation._rotation, translation);

            //builder.AddMeshFragment(mesh.core.Scaled(new Vec3D(1f, orientation._invertedY ? -1f : 1f, 1f)), orientation._rotation, translation);
            FlatNode flatNode;
            ThreeDimensionalCell complimentary;
            DeshelledCubeMesh complimentaryDeshelled;
            FlatNode complimentaryNode;
            MeshFragmentVec3D sideMesh3D;

            
            flatNode = mesh.ZNegativeFlatNode(orientation);
            complimentary = cellBuffer[(int)CubeSides.Enum.zNegative];
            complimentaryDeshelled = ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID);
            complimentaryNode = complimentaryDeshelled.ZPositiveComplementFlatNode(Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));
            sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id,flatNode.flatTransform,complimentaryNode.id,complimentaryNode.flatTransform);
            builder.AddMeshFragment(sideMesh3D, AngleMethods.ZNegToZNeg, translation);

            flatNode = mesh.XNegativeFlatNode(orientation);
            complimentary = cellBuffer[(int)CubeSides.Enum.xNegative];
            complimentaryDeshelled = ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID);
            complimentaryNode = complimentaryDeshelled.XPositiveComplementFlatNode(Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));
            sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id, flatNode.flatTransform, complimentaryNode.id, complimentaryNode.flatTransform);
            builder.AddMeshFragment(sideMesh3D, AngleMethods.ZNegToXNeg, translation);

            flatNode = mesh.YNegativeFlatNode(orientation);
            complimentary = cellBuffer[(int)CubeSides.Enum.yNegative];
            complimentaryDeshelled = ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID);
            complimentaryNode = complimentaryDeshelled.YPositiveComplementFlatNode(Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));
            sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id, flatNode.flatTransform, complimentaryNode.id, complimentaryNode.flatTransform);
            builder.AddMeshFragment(sideMesh3D, AngleMethods.ZNegToYNeg, translation);

            flatNode = mesh.ZPositiveFlatNode(orientation);
            complimentary = cellBuffer[(int)CubeSides.Enum.zPositive];
            complimentaryDeshelled = ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID);
            complimentaryNode = complimentaryDeshelled.ZNegativeComplementFlatNode(Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));
            sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id, flatNode.flatTransform, complimentaryNode.id, complimentaryNode.flatTransform);
            builder.AddMeshFragment(sideMesh3D, AngleMethods.ZNegToZPos, translation);

            flatNode = mesh.XPositiveFlatNode(orientation);
            complimentary = cellBuffer[(int)CubeSides.Enum.xPositive];
            complimentaryDeshelled = ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID);
            complimentaryNode = complimentaryDeshelled.XNegativeComplementFlatNode(Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));
            sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id, flatNode.flatTransform, complimentaryNode.id, complimentaryNode.flatTransform);
            builder.AddMeshFragment(sideMesh3D, AngleMethods.ZNegToXPos, translation);

            flatNode = mesh.YPositiveFlatNode(orientation);
            complimentary = cellBuffer[(int)CubeSides.Enum.yPositive];
            complimentaryDeshelled = ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID);
            complimentaryNode = complimentaryDeshelled.YNegativeComplementFlatNode(Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));
            sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id, flatNode.flatTransform, complimentaryNode.id, complimentaryNode.flatTransform);
            builder.AddMeshFragment(sideMesh3D, AngleMethods.ZNegToYPos, translation);
        }
        // FlatNodesComplimentaryNotOptimizedMatcher
        /*         internal static MeshFragmentVec3D GetFlatnodeComplimentaryAs3D(
        int id,
        FlatNodeTransform transform,
            int complimentaryId,
            FlatNodeTransform complimentaryTransform)
            */
    }
}
#endif