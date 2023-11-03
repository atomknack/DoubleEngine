using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    [Obsolete("not tested")]
    public class ThreeDimensionalGridNaiveCounter : IThreeDimensionalGrid
    {
        internal int _maxX;
        internal int _maxY;
        internal int _maxZ;
        internal int _updateCallsCount;

        public static ThreeDimensionalGridNaiveCounter Create() =>
            new ThreeDimensionalGridNaiveCounter();
        public int GetUpdateCallsCount() => _updateCallsCount;
        public Vec3I GetDimensions() => new Vec3I(_maxX + 1, _maxY + 1, _maxZ + 1);
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell)
        {
            ++_updateCallsCount;
            _maxX = Math.Max(x, _maxX);
            _maxY = Math.Max(y, _maxY);
            _maxZ = Math.Max(z, _maxZ);
        }
        void IThreeDimensionalGrid.Clear()
        {
            (this as ThreeDimensionalGridNaiveCounter).Clear();
        }
        public void Clear()
        {
            _updateCallsCount = 0;
            _maxX = -1;
            _maxY = -1;
            _maxZ = -1;
        }
        private ThreeDimensionalGridNaiveCounter()
        {
            Clear();
        }

        public MeshFragmentVec3D BuildMesh()
        {
            throw new NotImplementedException();
        }

        public ThreeDimensionalCell GetCell(int x, int y, int z)
        {
            throw new NotImplementedException();
        }
    }
}
