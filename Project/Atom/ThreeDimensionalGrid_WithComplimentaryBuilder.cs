#if TESTING
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public class ThreeDimensionalGrid_WithComplimentaryBuilder : IThreeDimensionalGrid
    {
        internal IThreeDimensionalGrid _grid;
        protected ThreeDimensionalCell[] _cellBuffer;
        protected (int x, int y, int z) _current;
        protected Vec3D _currentVec3D;
        protected IMeshBuilder3D _builder;//MeshBuilderVec3D _builder;

        public static ThreeDimensionalGrid_WithComplimentaryBuilder Create(int lengthX, int lengthY, int lengthZ)
        {
            ThreeDimensionalGrid_WithComplimentaryBuilder created = new ThreeDimensionalGrid_WithComplimentaryBuilder();
            created._grid = ThreeDimensionalGridBase.Create(lengthX, lengthY, lengthZ);
            created._cellBuffer = new ThreeDimensionalCell[6];
            return created;
        }
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell) => _grid.UpdateCell(x, y, z, cell);
        public ThreeDimensionalCell GetCell(int x, int y, int z) => _grid.GetCell(x, y, z);
        public Vec3I GetDimensions() => _grid.GetDimensions();
        public virtual MeshFragmentVec3D BuildMesh()
        {
            (int xSize, int ySize, int zSize) = _grid.GetDimensions();
            _builder = new MeshBufferBuilder();//_builder = new MeshBuilderVec3D();
            for (int yi = 0; yi < ySize; ++yi)
                for (int xi = 0; xi < xSize; ++xi)
                    for (int zi = 0; zi < zSize; ++zi)
                    {
                        ChangeCellForBuild(xi, yi, zi);
                        AddCellToMeshBuilder();
                    }
            return Build();
        }

        protected virtual MeshFragmentVec3D Build()
        {
            return _builder.BuildFragment();
        }

        protected virtual void ChangeCellForBuild(int xi, int yi, int zi)
        {
            _current = (xi, yi, zi);
            _currentVec3D = new Vec3D(xi, yi, zi);
        }

        protected virtual void AddCellMeshFragment(MeshFragmentVec3D fragment, Vec3D Scale, QuaternionD rotation)
        {
            _builder.AddMeshFragment(fragment, Scale, rotation, _currentVec3D);
        }
        protected virtual void AddCellMeshFragment(MeshFragmentVec3D fragment, QuaternionD rotation)
        {
            _builder.AddMeshFragment(fragment, rotation, _currentVec3D);
        }

        protected virtual void AddCellToMeshBuilder()
        {
            ThreeDimensionalCell cell = _grid.GetCellAndNeighbours(_current.x, _current.y, _current.z, _cellBuffer);
            DeshelledCubeMesh mesh = ThreeDimensionalCellMeshes.GetDeshelled(cell.meshID);
            Grid6SidesCached orientation = Grid6SidesCached.GetCached(cell.grid6SidesCachedIndex);
            if (orientation._invertedY)
                AddCellMeshFragment(mesh.core, orientation.Scale, orientation._rotation);
            else
                AddCellMeshFragment(mesh.core, orientation._rotation);

            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.zNegative);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.xNegative);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.yNegative);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.zPositive);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.xPositive);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.yPositive);
        }

        protected void AddCombinedSideToBuilder(DeshelledCubeMesh mesh, Grid6SidesCached orientation, CubeSides.Enum side)
        {
            int sideIndex = (int)side;
            FlatNode flatNode = ThreeDimensionalGridBase.SideFlatNode[sideIndex](mesh, orientation);//mesh.ZNegativeFlatNode(orientation);
            ThreeDimensionalCell complimentary = _cellBuffer[sideIndex];//cellBuffer[(int)Grid6Sides.SideEnum.zNegative];
            FlatNode complimentaryNode = ThreeDimensionalGridBase.SideComplementAsNeighbourFlatNode[sideIndex](
                ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID), 
                Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));//complimentaryDeshelled.ZPositiveComplementFlatNode(Grid6SidesCached.GetCached(complimentary.Grid6SidesCachedIndex));
            MeshFragmentVec3D sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id, flatNode.flatTransform, complimentaryNode.id, complimentaryNode.flatTransform);
            AddCellMeshFragment(sideMesh3D, CubeSides.FromZNeg[sideIndex]);
        }
    }
}
#endif