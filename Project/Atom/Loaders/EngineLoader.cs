using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom.Loaders
{
    public static class EngineLoader
    {
        internal static readonly LoaderFromFiles DefaultLoader;

        private static IEngineLoader s_loader = null;
        private static void SetDefaultLoader()
        {
            s_loader = DefaultLoader;
        }
        public static void SetLoader(IEngineLoader loader)
        {
            s_loader = loader;
        }
        internal static string LoadFlatNodes() => s_loader.LoadFlatNodes();
        internal static string LoadTDCellMeshes() => s_loader.LoadTDCellMeshes();
        static EngineLoader()
        {
            DefaultLoader = new LoaderFromFiles();
            if (s_loader ==  null)
                SetDefaultLoader();
        }
    }
}
