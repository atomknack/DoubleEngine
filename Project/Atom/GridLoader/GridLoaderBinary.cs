using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using static DoubleEngine.Atom.GridLoader;

namespace DoubleEngine.Atom
{
    public sealed class GridLoaderBinary : GridLoaderBase, IGridStreamLoader
    {
        BinaryReader _reader;
        BinaryWriter _writer;
        ThreeDimensionalCell _brush;

        public static void LoadGrid(IThreeDimensionalGridElementsProvider grid, string filepath)
        {
            var loader = new GridLoaderBinary();
            using var stream = File.Open(filepath, FileMode.Open);
            loader.LoadGridFromStream(grid, stream);
        }

        public static void SaveGrid(IThreeDimensionalGridElementsProvider grid, string path)
        {
            var loader = new GridLoaderBinary();
            using var stream = File.Open(path, FileMode.Create);
            loader.SaveGridToStream(grid, stream);
        }

        internal GridLoaderBinary()
        {
            _reader = null;
            _writer = null;
        }
        public void LoadGridFromStream(IThreeDimensionalGridElementsProvider grid, Stream stream)
        {
            _brush = ThreeDimensionalCell.Empty;
            _grid = grid;
            using (_reader = new BinaryReader(stream))
            {
                _grid.Clear();
                UpdateGridCells();
            }
            _grid = null;
            _reader = null;
        }

        public void SaveGridToStream(IThreeDimensionalGridElementsProvider grid, Stream stream)
        {
            _brush = ThreeDimensionalCell.Empty;
            _grid = grid;
            using (_writer = new BinaryWriter(stream))
            {
                WriteCommand_SetModel(ThreeDimensionalCell.Empty.meshID);
                WriteCommand_SetOrientation(Grid6SidesCached.GetCached(ThreeDimensionalCell.Empty.grid6SidesCachedIndex)._orientation);
                WriteCommand_SetMaterial(ThreeDimensionalCell.Empty.material);
                SaveGridCells();
            }
            _grid = null;
            _writer = null;
        }

        internal override bool FindCommand(out GridLoaderCommand command)
        {
            if(_reader.PeekChar() == -1)
            {
                command = GridLoaderCommand.DoNothing;
                return false;
            }
            command = (GridLoaderCommand)_reader.ReadByte();
            return true;
        }

        internal override void LoadCommand(GridLoaderCommand command)
        {
            switch (command)
            {
                case GridLoaderCommand.DoNothing:
                    return;
                case GridLoaderCommand.PutInInt16Coords:
                    ReadCommandParameters_PutInIntCoords();
                    return;
                case GridLoaderCommand.SetModel:
                    ReadCommandParamenters_SetModel();
                    return;
                case GridLoaderCommand.SetOrientation:
                    ReadCommandParameters_SetOrientation();
                    return;
                case GridLoaderCommand.SetMaterial:
                    ReadCommandParameters_SetMaterial();
                    return;
            }
        }

        internal override void ReadCommandParamenters_SetModel()
        {
            _brush = ThreeDimensionalCell.Create(_reader.ReadInt16(), _brush.grid6SidesCachedIndex, _brush.material);
        }

        internal override void ReadCommandParameters_PutInIntCoords()
        {
            _grid.UpdateCell(_reader.ReadInt16(), _reader.ReadInt16(), _reader.ReadInt16(), _brush);
        }

        internal override void ReadCommandParameters_SetMaterial()
        {
            byte material = _reader.ReadByte();
            _brush = ThreeDimensionalCell.Create(_brush.meshID, _brush.grid6SidesCachedIndex, material);
        }

        internal override void ReadCommandParameters_SetOrientation()
        {
            var sipr3d = ScaleInversionPerpendicularRotation3.FromInt(_reader.ReadInt32());
            _brush = ThreeDimensionalCell.Create(_brush.meshID, sipr3d, _brush.material);
        }

        internal override void WriteCell(Vec3short pos, ThreeDimensionalCell cell)
        {
            if (cell == ThreeDimensionalCell.Empty)
                return;
            if (cell.meshID == 0)
                return;
            if (cell.meshID != _brush.meshID)
            {
                WriteCommand_SetModel(cell.meshID);
                _brush = ThreeDimensionalCell.Create(cell.meshID, _brush.grid6SidesCachedIndex, _brush.material);
            }
            if (cell.grid6SidesCachedIndex != _brush.grid6SidesCachedIndex)
            {
                WriteCommand_SetOrientation(Grid6SidesCached.GetCached(cell.grid6SidesCachedIndex)._orientation);
                _brush = ThreeDimensionalCell.Create(_brush.meshID, cell.grid6SidesCachedIndex, _brush.material);
            }
            if (cell.material != _brush.material)
            {
                WriteCommand_SetMaterial(cell.material);
                _brush = ThreeDimensionalCell.Create(_brush.meshID, _brush.grid6SidesCachedIndex, cell.material);
            }
            WriteCommand_PutInCoords(pos);
        }

        internal override void WriteCommand_PutInCoords(Vec3short vec3I)
        {
            _writer.Write((Byte)GridLoaderCommand.PutInInt16Coords);
            _writer.Write(vec3I.x);
            _writer.Write(vec3I.y);
            _writer.Write(vec3I.z);
        }

        internal override void WriteCommand_SetMaterial(byte material)
        {
            _writer.Write((Byte)GridLoaderCommand.SetMaterial);
            _writer.Write(material);
        }

        internal override void WriteCommand_SetModel(short modelId)
        {
            _writer.Write((Byte)GridLoaderCommand.SetModel);
            _writer.Write(modelId);
        }

        internal override void WriteCommand_SetOrientation(ScaleInversionPerpendicularRotation3 sipr3d)
        {
            _writer.Write((Byte)GridLoaderCommand.SetOrientation);
            _writer.Write(ScaleInversionPerpendicularRotation3.ToInt(sipr3d));
        }
    }
}
