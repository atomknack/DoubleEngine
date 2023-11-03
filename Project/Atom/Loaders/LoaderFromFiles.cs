using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoubleEngine.Atom.Loaders
{
    //[Obsolete("Wip, not tested")]
    public sealed class LoaderFromFiles : IEngineLoader
    {
        private string _path;
        internal string FlatNodesPath => _path + @"flatnodesData.json_";
        internal string TDCellMeshesPath => _path + @"cellMeshesData.json_";

        public LoaderFromFiles(string path = null)
        {
            if (path is null)
                path = @".\";
            _path = path;
        }

        public string LoadFlatNodes()
        {
            try
            {
                return File.ReadAllText(FlatNodesPath);
            }
            catch
            {
                //Debug.Log("Cannot Load FlatNodes file");
                throw;
            }
        }

            public string LoadTDCellMeshes()
        {
            try
            {
                return File.ReadAllText(TDCellMeshesPath);
            }
            catch
            {
                //Debug.Log("Cannot Load ThreeDimensionalCellMeshes file");
                throw;
            }
        }
    }
}
