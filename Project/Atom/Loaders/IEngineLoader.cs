using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom.Loaders
{
    public interface IEngineLoader
    {
        string LoadFlatNodes();
        string LoadTDCellMeshes();
    }
}
