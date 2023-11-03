using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public interface IMeshFragment<Vec>
    {
        public ReadOnlySpan<Vec> Vertices { get; }
        public ReadOnlySpan<int> Triangles { get; }
        public ReadOnlySpan<IndexedTri> Faces { get; }
    }
}
