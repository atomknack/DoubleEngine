using System;
using System.Collections.Generic;

namespace DoubleEngine.Atom
{
    public interface IThreeDimensionalGrid: IThreeDimensionalGridElementsProvider
    {
        //MeshFragmentVec3D BuildMesh();
        new public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell);
        public ThreeDimensionalCell GetCell(int x, int y, int z);
        public Vec3I GetDimensions();
        new public virtual void Clear()
        {
            (int xLength, int yLength, int zLength) = this.GetDimensions();
            for(int xi=0;xi<xLength;++xi)
                for(int yi=0;yi<yLength;++yi)
                    for(int zi=0;zi<zLength;++zi)
                        UpdateCell(xi,yi,zi, ThreeDimensionalCell.Empty);
        }

        public virtual IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> Transform(int x, int y, int z, byte rotation)
        {
            (int xSize, int ySize, int zSize) = this.GetDimensions();
            var cached = Grid6SidesCached.GetCached(rotation);
            var matrix = MatrixD4x4.FromRotation(cached._rotation)*MatrixD4x4.FromScale(cached.Scale);
            for(var xi=0;xi<xSize;++xi)
                for(var yi=0;yi<ySize;++yi)
                    for(var zi=0;zi<zSize;++zi)
                    {
                        var cell = this.GetCell(xi,yi,zi);
                        if (cell.meshID != 0)
                        {
                            var vecD = matrix.MultiplyPoint3x4(new Vec3D(xi,yi,zi));
                            var vecI = new Vec3I(
                                x+Convert.ToInt32(vecD.x),//(int)Math.Round(vecD.x),
                                y+Convert.ToInt32(vecD.y),//(int)Math.Round(vecD.y),
                                z+Convert.ToInt32(vecD.z));//(int)Math.Round(vecD.z));
                            yield return (vecI, 
                                ThreeDimensionalCell.Create(
                                    cell.meshID,
                                    Grid6SidesCached.TransformedBy(cell.grid6SidesCachedIndex, rotation),
                                    cell.material));
                        }
                    }

        }

        internal ThreeDimensionalCell GetCellAndNeighbours(int x, int y, int z, Span<ThreeDimensionalCell> bufferForReturnedNeighbours)
        {
            //public enum SideEnum zNegative = 0,xNegative = 1,yNegative = 2,zPositive = 3,xPositive = 4,yPositive = 5
            bufferForReturnedNeighbours[(int)CubeSides.Enum.zNegative] = GetCell(x, y, z - 1);
            bufferForReturnedNeighbours[(int)CubeSides.Enum.xNegative] = GetCell(x - 1, y, z);
            bufferForReturnedNeighbours[(int)CubeSides.Enum.yNegative] = GetCell(x, y - 1, z);
            bufferForReturnedNeighbours[(int)CubeSides.Enum.zPositive] = GetCell(x, y, z + 1);
            bufferForReturnedNeighbours[(int)CubeSides.Enum.xPositive] = GetCell(x + 1, y, z);
            bufferForReturnedNeighbours[(int)CubeSides.Enum.yPositive] = GetCell(x, y + 1, z);
            return GetCell(x, y, z);
        }

        public virtual IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> GetAllMeaningfullCells()
        {
            (int xLength, int yLength, int zLength) = this.GetDimensions();
            for (int xi = 0; xi < xLength; ++xi)
                for (int yi = 0; yi < yLength; ++yi)
                    for (int zi = 0; zi < zLength; ++zi)
                    {
                        var cell = this.GetCell(xi, yi, zi);
                        if (cell.meshID > 0)
                            yield return (new Vec3I(xi, yi, zi), cell);
                    }

        }
        public static long CountNonEmpty(IThreeDimensionalGrid grid)
        {
            long count = 0;
            foreach (var placed in grid.GetAllMeaningfullCells()) 
            {
                if (placed.cell.meshID!=0)
                    count++;
            }

            return count;
        }

        IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> IThreeDimensionalGridElementsProvider.GetAllMeaningfullCells() =>
            GetAllMeaningfullCells();
        void IThreeDimensionalGridElementsProvider.UpdateCell(int x, int y, int z, DoubleEngine.Atom.ThreeDimensionalCell cell) =>
            UpdateCell(x, y, z, cell);
        void IThreeDimensionalGridElementsProvider.Clear() =>
            Clear();
    }
}