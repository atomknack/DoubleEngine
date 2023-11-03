using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public interface IMeshFragmentWithMaterials<Vec> : IMeshFragment<Vec>
    {
        public ReadOnlySpan<MaterialByte> FaceMaterials { get; }
    }
}
