using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public sealed class ThreeDimensionalGridOffsetter : IThreeDimensionalGrid
    {
        private IThreeDimensionalGrid _grid;
        private Vec3I _inOffset;
        private int _inOffsetX;
        private int _inOffsetY;
        private int _inOffsetZ;
        private Vec3I _outOffset;
        private int _outOffsetX;
        private int _outOffsetY;
        private int _outOffsetZ;

        public static ThreeDimensionalGridOffsetter Create(IThreeDimensionalGrid grid) => 
            new ThreeDimensionalGridOffsetter(grid);
        public static ThreeDimensionalGridOffsetter CreateClone(ThreeDimensionalGridOffsetter offsetter)
        {
            var clone = Create(offsetter._grid);
            clone.SetInOffset(offsetter._inOffset);
            clone.SetOutOffset(offsetter._outOffset);
            return clone;
        }

        public void Clear()
        {
            _grid.Clear();
        }
        IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> IThreeDimensionalGrid.GetAllMeaningfullCells() => 
            GetAllMeaningfullCells();
        public IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> GetAllMeaningfullCells()
        {
            foreach ((var pos, var cell) in _grid.GetAllMeaningfullCells())
            {
                //Debug.Log($"pos:{pos}, cell:{cell}");
                yield return (pos + _outOffset, cell);
            }
        }

        public void SetOffset(Vec3I offset)
        {
            SetInOffset(offset);
            SetOutOffset(-offset);
        }

        public void SetInOffset(Vec3I offset) 
        { 
            _inOffset = offset;
            (_inOffsetX, _inOffsetY, _inOffsetZ) = offset; 
        }
        public Vec3I GetInOffset() => _inOffset;
        public void SetOutOffset(Vec3I offset) 
        { 
            _outOffset = offset;
            (_outOffsetX, _outOffsetY, _outOffsetZ) = offset; 
        }
        public Vec3I GetOutOffset() => _outOffset;


        public ThreeDimensionalCell GetCell(int x, int y, int z) =>
            _grid.GetCell(x+ _outOffsetX, y+ _outOffsetY, z+ _outOffsetZ);

        public Vec3I GetDimensions() =>
            _grid.GetDimensions();

        public Vec3I ProjectCoordinateInsideGrid(Vec3I coord) => coord + _inOffset;
        public Vec3I ProjectCoordinateFromGridToOutside(Vec3I coord) => coord + _outOffset;
        public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell)
        {
            _grid.UpdateCell(x+ _inOffsetX, y+ _inOffsetY, z+ _inOffsetZ, cell);
        }

        internal ThreeDimensionalGridOffsetter(IThreeDimensionalGrid grid)
        {
            _grid = grid;
            _inOffset = Vec3I.zero;
            _inOffsetX = 0;
            _inOffsetY = 0;
            _inOffsetZ = 0;
            _outOffset = Vec3I.zero;
            _outOffsetX= 0;
            _outOffsetY= 0;
            _outOffsetZ= 0;
        }
    }
}
