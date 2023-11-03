using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DoubleEngine;
using DoubleEngine.Atom;
using DoubleEngine.Atom.GridLoader;

namespace DoubleEngine.Atom
{
    public static class GridLoaders
    {
        public static readonly string Extension_BG10 = ".bg10";
        public static readonly string Extension_BG10TEXT = ".bg10text";
        public static readonly string Extension_BG33 = ".bg33";

        private static readonly string[] s_canSaveToExtensions = {Extension_BG10, Extension_BG10TEXT, Extension_BG33 };
        private static readonly string[] s_canLoadFromExtensions = {Extension_BG10, Extension_BG10TEXT, Extension_BG33 };

        public static readonly IEnumerable<string> CanSaveToExtensions = s_canSaveToExtensions;
        public static readonly IEnumerable<string> CanLoadFromExtensions = s_canLoadFromExtensions;


        public static void LoadGrid(IThreeDimensionalGridElementsProvider grid, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            switch (extension)
            {
                case string s when s.Equals(Extension_BG33, StringComparison.InvariantCultureIgnoreCase):
                    GridLoaderBG33.LoadGrid(grid, fileName);
                    break;
                case string s when s.Equals(Extension_BG10, StringComparison.InvariantCultureIgnoreCase):
                    LoadGridFromBinaryFile(grid, fileName);
                    break;
                case string s when s.Equals(Extension_BG10TEXT, StringComparison.InvariantCultureIgnoreCase):
                    LoadGridFromTextFile(grid, fileName);
                    break;
                default: 
                    throw new ArgumentException($"Not found suitable loader for extension:{extension} of path:{fileName}");
                    //break;
            }
        }

        public static void SaveGrid(IThreeDimensionalGridElementsProvider grid, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            switch (extension)
            {
                case string s when s.Equals(Extension_BG33, StringComparison.InvariantCultureIgnoreCase):
                    GridLoaderBG33.SaveGrid(grid, fileName);
                    break;
                case string s when s.Equals(Extension_BG10, StringComparison.InvariantCultureIgnoreCase):
                    SaveGridToBinaryFile(grid, fileName);
                    break;
                case string s when s.Equals(Extension_BG10TEXT, StringComparison.InvariantCultureIgnoreCase):
                    SaveGridToTextFile(grid, fileName);
                    break;
                default:
                    throw new ArgumentException($"Not found suitable saver for extension:{extension} of path:{fileName}");
                    //break;
            }
        }


        public static void LoadGridFromBinaryFile(IThreeDimensionalGridElementsProvider grid, string filename)
            => GridLoaderBinary.LoadGrid(grid, filename);
        public static void LoadGridFromTextFile(IThreeDimensionalGridElementsProvider grid, string filename)
            => GridLoaderTextStream.LoadGrid(grid, filename);


        public static void SaveGridToBinaryFile(IThreeDimensionalGridElementsProvider grid, string filename)
            => GridLoaderBinary.SaveGrid(grid, filename);
        public static void SaveGridToTextFile(IThreeDimensionalGridElementsProvider grid, string filename)
            => GridLoaderTextStream.SaveGrid(grid, filename);
    }
}
