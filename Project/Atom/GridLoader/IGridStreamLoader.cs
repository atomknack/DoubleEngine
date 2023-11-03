using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoubleEngine.Atom
{
    internal interface IGridStreamLoader
    {
        void SaveGridToStream(IThreeDimensionalGridElementsProvider grid, Stream stream);
        void LoadGridFromStream(IThreeDimensionalGridElementsProvider grid, Stream stream);
    }
}
