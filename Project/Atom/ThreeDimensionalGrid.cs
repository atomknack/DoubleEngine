using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public sealed class ThreeDimensionalGrid: IThreeDimensionalGrid
    {
        internal ThreeDimensionalGridBase _grid;

        public static ThreeDimensionalGrid Create(int lengthX, int lengthY, int lengthZ)
        {
            ThreeDimensionalGrid created = new ThreeDimensionalGrid(lengthX, lengthY, lengthZ);
            return created;
        }

        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell) => _grid.UpdateCell(x, y, z, cell);
        public ThreeDimensionalCell GetCell(int x, int y, int z) => _grid.GetCell(x, y, z);
        public Vec3I GetDimensions() => _grid.GetDimensions();

        internal ThreeDimensionalGrid(int lengthX, int lengthY, int lengthZ)
        {
            _grid = ThreeDimensionalGridBase.Create(lengthX, lengthY, lengthZ);
        }
    }
}