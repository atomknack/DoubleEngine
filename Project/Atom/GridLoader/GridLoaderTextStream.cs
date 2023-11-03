using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using static DoubleEngine.Atom.GridLoader;

namespace DoubleEngine.Atom
{
    public sealed class GridLoaderTextStream : GridLoaderBase, IGridStreamLoader
    {
        private static readonly string s_putCommandAsString = "put";
        private static readonly string s_modelCommandAsString = "model";
        private static readonly string s_orientationCommandAsString = "orientation";
        private static readonly string s_materialCommandAsString = "material";
        StreamReader _reader;
        StreamWriter _writer;
        ThreeDimensionalCell _brush;
        string _lastReadString;
        string[] _splitted;

        public static void LoadGrid(IThreeDimensionalGridElementsProvider grid, string filepath)
        {
            var loader = new GridLoaderTextStream();
            using var stream = File.Open(filepath, FileMode.Open);
            loader.LoadGridFromStream(grid, stream);
        }

        public static void SaveGrid(IThreeDimensionalGridElementsProvider grid, string path)
        {
            var loader = new GridLoaderTextStream();
            using var stream = File.Open(path, FileMode.OpenOrCreate);
            loader.SaveGridToStream(grid, stream);
        }

        internal GridLoaderTextStream()
        {
            _reader = null;
            _writer = null;
        }
        public void LoadGridFromStream(IThreeDimensionalGridElementsProvider grid, Stream stream)
        {
            _brush = ThreeDimensionalCell.Empty;
            _grid = grid;
            using (_reader = new StreamReader(stream))
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
            using (_writer = new StreamWriter(stream))
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
            _lastReadString = _reader.ReadLine();
            if(_lastReadString == null)
            {
                command = GridLoaderCommand.DoNothing;
                return false;
            }
            _splitted = _lastReadString.Split(' ');
            string first = _splitted[0];
            switch (first)
            {
                case string c when c.Equals(s_putCommandAsString, StringComparison.InvariantCultureIgnoreCase):
                    command = GridLoaderCommand.PutInInt16Coords;
                    break;
                case string c when c.Equals(s_modelCommandAsString, StringComparison.InvariantCultureIgnoreCase):
                    command = GridLoaderCommand.SetModel;
                    break;
                case string c when c.Equals(s_orientationCommandAsString, StringComparison.InvariantCultureIgnoreCase):
                    command = GridLoaderCommand.SetOrientation;
                    break;
                case string c when c.Equals(s_materialCommandAsString, StringComparison.InvariantCultureIgnoreCase):
                    command = GridLoaderCommand.SetMaterial;
                    break;
                default:
                    command = GridLoaderCommand.DoNothing;
                    break;
            }
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
            _brush = ThreeDimensionalCell.Create(short.Parse(_splitted[1]), _brush.grid6SidesCachedIndex, _brush.material);
        }

        internal override void ReadCommandParameters_PutInIntCoords()
        {
            _grid.UpdateCell(short.Parse(_splitted[1]), short.Parse(_splitted[2]), short.Parse(_splitted[3]), _brush);
        }

        internal override void ReadCommandParameters_SetMaterial()
        {
            byte material = byte.Parse(_splitted[1]);
            _brush = ThreeDimensionalCell.Create(_brush.meshID, _brush.grid6SidesCachedIndex, material);
        }

        internal override void ReadCommandParameters_SetOrientation()
        {
            var sipr3d = ScaleInversionPerpendicularRotation3.FromInt(int.Parse(_splitted[1]));
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
            _writer.WriteLine($"{s_putCommandAsString} {vec3I.x} {vec3I.y} {vec3I.z}");
        }

        internal override void WriteCommand_SetMaterial(byte material)
        {
            _writer.WriteLine($"{s_materialCommandAsString} {material}");
        }

        internal override void WriteCommand_SetModel(short modelId)
        {
            _writer.WriteLine($"{s_modelCommandAsString} {modelId}");
        }

        internal override void WriteCommand_SetOrientation(ScaleInversionPerpendicularRotation3 sipr3d)
        {
            _writer.WriteLine($"{s_orientationCommandAsString} {ScaleInversionPerpendicularRotation3.ToInt(sipr3d)}");
        }
    }
}
