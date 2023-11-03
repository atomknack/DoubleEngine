using System;
using System.IO;

namespace DoubleEngine.Atom
{
    public abstract partial class GridLoaderBase
    {
        protected internal IThreeDimensionalGridElementsProvider _grid;

        internal virtual void SaveGridCells()
        {
            foreach (var cellElement in _grid.GetAllMeaningfullCells())
            {
                WriteCell(cellElement.pos.ToVec3short(), cellElement.cell);
            }
        }

        internal virtual void UpdateGridCells()
        {
            while (FindCommand(out var command))
                LoadCommand(command);
        }

        internal abstract bool FindCommand(out GridLoaderCommand command);
        internal abstract void LoadCommand(GridLoaderCommand command);
        internal abstract void ReadCommandParamenters_SetModel();
        internal abstract void ReadCommandParameters_PutInIntCoords();
        internal abstract void ReadCommandParameters_SetMaterial();
        internal abstract void ReadCommandParameters_SetOrientation();

        internal abstract void WriteCell(Vec3short vec3I, ThreeDimensionalCell threeDimensionalCell);
        internal abstract void WriteCommand_PutInCoords(Vec3short vec3I);
        internal abstract void WriteCommand_SetMaterial(byte material);
        internal abstract void WriteCommand_SetModel(Int16 modelId);
        internal abstract void WriteCommand_SetOrientation(ScaleInversionPerpendicularRotation3 sipr3d);

        /*
public void SaveGrid(IThreeDimensionalGrid grid)
{
    StartSavingGrid(grid);
    SaveGridCells();
    FinishSavingGrid();
}

public virtual void UpdateGrid(IThreeDimensionalGrid grid)
{
    StartUpdatingGrid(grid);
    UpdateGridCells();
    FinishUpdatingGrid();
}

internal virtual void StartSavingGrid(IThreeDimensionalGrid grid)
{
    _grid = grid;
}*/

        /*
internal virtual void FinishSavingGrid()
{
    _grid = null;
}

internal virtual void StartUpdatingGrid(IThreeDimensionalGrid grid)
{
    _grid = grid;
}
internal virtual void FinishUpdatingGrid()
{
    _grid = null;
}*/
    }
}