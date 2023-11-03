using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static partial class MeshFragmentVec2D_Extentions
    {
        public static MeshFragmentVec3D To3D(this MeshFragmentVec2D m, double newZ) =>
            new MeshFragmentVec3D(m.vertices.ConvertXYtoXYZArray(newZ), m.triangles, m._joinedVertices);
    }
}
