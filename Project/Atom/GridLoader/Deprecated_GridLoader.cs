#if TESTING
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoubleEngine.Atom
{
    [Obsolete("Deprecated, do not use")]
    public partial struct ForTestingOnly_OldDeprecatedGridLoader
    {
        public enum Command : byte
        {
            DoNothing = 0,
            //StopLoading = 1,
            PutInIntCoords = 16,
            SetModel = 17,
            SetOrientation = 18,
            SetMaterial = 19
        }

        BinaryWriter _writer;
        BinaryReader _reader;
        IThreeDimensionalGrid _grid;
        ThreeDimensionalCell _brush;

        internal void WriteCell(Int16 x, Int16 y, Int16 z,ThreeDimensionalCell cell)
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
                WriteCommand_SetOrientation(Grid6SidesCached.GetCached( cell.grid6SidesCachedIndex)._orientation );
                _brush = ThreeDimensionalCell.Create(_brush.meshID, cell.grid6SidesCachedIndex, _brush.material);
            }
            if(cell.material != _brush.material)
            {
                WriteCommand_SetMaterial(cell.material);
                _brush = ThreeDimensionalCell.Create(_brush.meshID, _brush.grid6SidesCachedIndex, cell.material);
            }
            WriteCommand_PutInIntCoords(x, y, z);
        }
        internal void WriteCommand_PutInIntCoords(Int16 x, Int16 y, Int16 z)
        {
            _writer.Write((Byte)Command.PutInIntCoords);
            _writer.Write(x);
            _writer.Write(y);
            _writer.Write(z);
        }
        internal void ReadCommandParameters_PutInIntCoords()
        {
            _grid.UpdateCell(_reader.ReadInt16(), _reader.ReadInt16(), _reader.ReadInt16(), _brush);
        }
        internal void WriteCommand_SetModel(Int16 modelId)
        {
            _writer.Write((Byte)Command.SetModel);
            _writer.Write(modelId);
        }
        internal void ReadCommandParamenters_SetModel()
        {
            _brush = ThreeDimensionalCell.Create(_reader.ReadInt16(), _brush.grid6SidesCachedIndex,_brush.material);
        }
        internal void WriteCommand_SetOrientation(ScaleInversionPerpendicularRotation3 sipr3d)
        {
            _writer.Write((Byte)Command.SetOrientation);
            _writer.Write(ScaleInversionPerpendicularRotation3.ToInt(sipr3d));
        }
        internal void ReadCommandParameters_SetOrientation()
        {
            var sipr3d = ScaleInversionPerpendicularRotation3.FromInt(_reader.ReadInt32());
            _brush = ThreeDimensionalCell.Create(_brush.meshID,sipr3d,_brush.material);
        }
        internal void WriteCommand_SetMaterial(byte material)
        {
            _writer.Write((Byte)Command.SetMaterial);
            _writer.Write(material);
        }
        internal void ReadCommandParameters_SetMaterial()
        {
            byte material = _reader.ReadByte();
            _brush = ThreeDimensionalCell.Create(_brush.meshID,_brush.grid6SidesCachedIndex,material);
        }

        internal void LoadCommand()
        {
            Command command = FindCommand();
            switch (command)
            {
                case Command.DoNothing:
                    return;
                case Command.PutInIntCoords:
                    ReadCommandParameters_PutInIntCoords();
                    return;
                case Command.SetModel:
                    ReadCommandParamenters_SetModel();
                    return;
                case Command.SetOrientation:
                    ReadCommandParameters_SetOrientation();
                    return;
                case Command.SetMaterial:
                    ReadCommandParameters_SetMaterial();
                    return;
            }
        }

        internal Command FindCommand() => (Command)_reader.ReadByte();

        public static void SaveGrid(IThreeDimensionalGrid grid, string path)
        {
            var stream = File.Open(path, FileMode.OpenOrCreate);
            SaveGrid(grid, stream);
        }

        public static void SaveGrid(IThreeDimensionalGrid grid, FileStream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                ForTestingOnly_OldDeprecatedGridLoader.SaveGridTo(grid, writer);
            }
        }

        public static void SaveGridTo(IThreeDimensionalGrid grid, BinaryWriter writer)
        {
            ForTestingOnly_OldDeprecatedGridLoader loader = new ForTestingOnly_OldDeprecatedGridLoader(writer, grid);
            loader.SaveGridToWriter();
        }
        public void SaveGridToWriter()
        {
            WriteCommand_SetModel(ThreeDimensionalCell.Empty.meshID);
            WriteCommand_SetOrientation(Grid6SidesCached.GetCached(ThreeDimensionalCell.Empty.grid6SidesCachedIndex)._orientation);
            WriteCommand_SetMaterial(ThreeDimensionalCell.Empty.material);
            Vec3I size = _grid.GetDimensions();
            for (short xi = 0; xi < size.x; ++xi)
                for (short yi = 0; yi < size.y; ++yi)
                    for (short zi = 0; zi < size.z; ++zi)
                        WriteCell(xi, yi, zi, _grid.GetCell(xi, yi, zi));
        }

        public static void LoadGrid(IThreeDimensionalGrid grid, string filepath)
        {
            var stream = File.Open(filepath, FileMode.Open);
            LoadGrid(grid, stream);
        }

        public static void LoadGrid(IThreeDimensionalGrid grid, FileStream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                grid.Clear();
                UpdateGridFrom(grid, reader);
            }
        }

        public static void UpdateGridFrom(IThreeDimensionalGrid grid, BinaryReader reader)
        {
            ForTestingOnly_OldDeprecatedGridLoader loader = new ForTestingOnly_OldDeprecatedGridLoader(reader, grid);
            while (loader._reader.PeekChar() != -1)
                loader.LoadCommand();
        }
        private ForTestingOnly_OldDeprecatedGridLoader(BinaryReader reader, IThreeDimensionalGrid grid)
        {
            _writer = null;
            _reader = reader;
            _grid = grid;
            _brush = ThreeDimensionalCell.Empty;
        }
        private ForTestingOnly_OldDeprecatedGridLoader(BinaryWriter writer, IThreeDimensionalGrid grid)
        {
            _writer = writer;
            _reader = null;
            _grid = grid;
            _brush = ThreeDimensionalCell.Empty;
        }

    }
}
#endif