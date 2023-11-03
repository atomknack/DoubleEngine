using System;
using System.Collections.Generic;

namespace DoubleEngine.Atom
{
    public interface IThreeDimensionalGridElementsProvider
    {
        public IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> GetAllMeaningfullCells();
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell);
        public void Clear();
    }
}
