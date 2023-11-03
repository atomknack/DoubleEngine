using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    internal class ThreeDimensionalGridBase : IThreeDimensionalGrid
    {
        internal static readonly Func<DeshelledCubeMesh, Grid6SidesCached, FlatNode>[] SideFlatNode =
        {
            (deshelled, orientation) => deshelled.ZNegativeFlatNode(orientation),
            (deshelled, orientation) => deshelled.XNegativeFlatNode(orientation),
            (deshelled, orientation) => deshelled.YNegativeFlatNode(orientation),
            (deshelled, orientation) => deshelled.ZPositiveFlatNode(orientation),
            (deshelled, orientation) => deshelled.XPositiveFlatNode(orientation),
            (deshelled, orientation) => deshelled.YPositiveFlatNode(orientation),
        };
        internal static readonly Func<DeshelledCubeMesh, Grid6SidesCached, FlatNode>[] SideComplementAsNeighbourFlatNode =
        {
            (deshelled, orientation) => deshelled.ZPositiveComplementFlatNode(orientation),
            (deshelled, orientation) => deshelled.XPositiveComplementFlatNode(orientation),
            (deshelled, orientation) => deshelled.YPositiveComplementFlatNode(orientation),
            (deshelled, orientation) => deshelled.ZNegativeComplementFlatNode(orientation),
            (deshelled, orientation) => deshelled.XNegativeComplementFlatNode(orientation),
            (deshelled, orientation) => deshelled.YNegativeComplementFlatNode(orientation),
        };

        internal ThreeDimensionalCell[][,] _cells;
        internal int _yLength;
        internal int _lastY;
        internal int _xLength;
        internal int _lastX;
        internal int _zLength;
        internal int _lastZ;

        //public ReadOnlySpan2D<ThreeDimensionalGridCell>[,] GetYLayer(int y) => _cells[y];
        public static ThreeDimensionalGridBase Create(int lengthX, int lengthY, int lengthZ) =>
            new ThreeDimensionalGridBase(lengthX, lengthY, lengthZ);
        public Vec3I GetDimensions() => new Vec3I(_xLength, _yLength, _zLength);
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell)
        {
            if (x >= 0 && y >= 0 && z >= 0 && x <= _lastX && y <= _lastY && z <= _lastZ)
                SetCellNoCheck(x, y, z, cell);
            else
            {
                throw new Exception($"Cannot update cell({x},{y},{z}) - is outside grid (0-{_lastX},0-{_lastY},0-{_lastZ}");
                //Debug.Log($"Cannot update cell({x},{y},{z}) - is outside grid (0-{_lastX},0-{_lastY},0-{_lastZ}");
            }

        }
        public ThreeDimensionalCell GetCell(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x > _lastX || y > _lastY || z > _lastZ)
                return ThreeDimensionalCell.Empty;
            return GetCellNoCheck(x, y, z);
        }

        private void SetCellNoCheck(int x, int y, int z, ThreeDimensionalCell cell) => _cells[y][x, z] = cell;

        public MeshFragmentVec3D BuildMesh()
        {
            throw new NotImplementedException();
            //return grid.BuildMesh();
        }

        internal ThreeDimensionalCell GetCellNoCheck(int x, int y, int z) => _cells[y][x, z];
        private static ThreeDimensionalCell[][,] CreateCellsArray(int lengthX, int lengthY, int lengthZ)
        {
            ThreeDimensionalCell[][,] result = new ThreeDimensionalCell[lengthY][,];
            for (int i = 0; i < lengthY; ++i)
                result[i] = new ThreeDimensionalCell[lengthX, lengthZ];
            return result;
        }

        private ThreeDimensionalGridBase(int lengthX, int lengthY, int lengthZ)
        {
            _xLength = lengthX;
            _yLength = lengthY;
            _zLength = lengthZ;
            _lastX = _xLength - 1;
            _lastY = _yLength - 1;
            _lastZ = _zLength - 1;
            _cells = CreateCellsArray(_xLength, _yLength, _zLength);
        }
    }
}
