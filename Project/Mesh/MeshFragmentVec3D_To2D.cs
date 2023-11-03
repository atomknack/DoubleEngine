using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static partial class MeshFragmentVec3D_Extentions
    {
        public static MeshFragmentVec2D To2D(this MeshFragmentVec3D m) =>
            new MeshFragmentVec2D(m.vertices.ConvertXYZtoXYArray(), m.triangles, m._joinedVertices);
    }
}
