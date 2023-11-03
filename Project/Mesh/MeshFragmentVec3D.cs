using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine;

public static partial class MeshFragmentVec3D_Extentions
{
    public static MeshFragmentVec3D MultiplyedByMatrixAs3x4PointNoScaleChecks(this MeshFragmentVec3D mesh, MatrixD4x4 m) 
        => MeshFragmentVec3D.CreateMeshFragment(mesh.Vertices.MultipliedByMatrixAsPoint3x4(m), mesh.Faces);

}
