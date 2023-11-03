using DoubleEngine.Atom.Multithreading;
using System.Collections.Generic;


namespace DoubleEngine.Atom;

public partial class ThreeDimensionalChunker : AbstractOverlord<SpaceCell, Vec3I, BuildedChunk>, IThreeDimensionalGrid
{
    public partial class Worker : AbstractWorker {}

    private readonly int _chunkDimension;
    private readonly ThreeDimensionalGridOffsetter _offsetter;

    private readonly int _sizeX;
    private readonly int _sizeY;
    private readonly int _sizeZ;
    public void UpdateCell(int x, int y, int z, ThreeDimensionalCell cell)
    {
        AddInput(new SpaceCell(new Vec3I(x, y, z), cell));
    }
    IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> IThreeDimensionalGrid.GetAllMeaningfullCells() =>
        GetAllMeaningfullCells();
    IEnumerable<(Vec3I pos, ThreeDimensionalCell cell)> GetAllMeaningfullCells() => 
        _offsetter.GetAllMeaningfullCells();
    public ThreeDimensionalCell GetCell(int x, int y, int z) => _offsetter.GetCell(x, y, z);

    public Vec3I GetDimensions() => _offsetter.GetDimensions();

    public ThreeDimensionalChunker(int chunkDimension, IThreeDimensionalGrid grid, Vec3I offset) : base(7, 30)
    {
        _chunkDimension = chunkDimension;
        _offsetter = ThreeDimensionalGridOffsetter.Create(grid);
        _offsetter.SetOffset(offset);
        var dimensions = _offsetter.GetDimensions();
        _sizeX = dimensions.x;
        _sizeY = dimensions.y;
        _sizeZ = dimensions.z;
    }

    public override AbstractWorker CreateSubordinateWorker() => new Worker(this);

    protected override void AddWork()
    {
        var corners = Vec3I.Corners;
        var currentChunk = Vec3I.zero;
        bool haveChunk = false;
        while (TryGetInputForWork(out SpaceCell cell))
        {
            var coords = cell.coords;
            _offsetter.UpdateCell(coords.x, coords.y, coords.z, cell.cell);
            ///Debug.Log($"{((IThreeDimensionalGrid)_offsetter).GetAllMeaningfullCells().Count()} _offsetter meaningfullCells");
            foreach (var corner in corners)
            {
                if (TryGetChunkCoordFromWordCoord(coords + corner, out Vec3I newChunk))
                {
                    if (haveChunk)
                    {
                        if (currentChunk != newChunk)
                        {
                            currentChunk = newChunk;
                            AddWorkItemToBeProcessed(currentChunk, true);
                        }
                    }
                    else
                    {
                        haveChunk = true;
                        currentChunk = newChunk;
                        AddWorkItemToBeProcessed(currentChunk, true);
                    }
                }
            }

        }
    }
    internal bool TryGetChunkCoordFromWordCoord(Vec3I cellCoord, out Vec3I chunkLocalStartCoord)
    {
        Vec3I coord = _offsetter.ProjectCoordinateInsideGrid(cellCoord);
        if (coord.x < 0 || coord.y < 0 || coord.z < 0 || coord.x >= _sizeX || coord.y >= _sizeY || coord.z >= _sizeZ)
        {
            chunkLocalStartCoord = Vec3I.zero;
            return false;
        }
        chunkLocalStartCoord = new Vec3I(
            (coord.x / _chunkDimension)*_chunkDimension, 
            (coord.y / _chunkDimension)*_chunkDimension, 
            (coord.z / _chunkDimension)*_chunkDimension
            );
        //chunkStartCoord = _offsetter.ProjectCoordinateFromGridToOutside(chunkStartCoord);
        //Debug.Log(chunkLocalStartCoord);
        return true;
    }
}
