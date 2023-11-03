using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine;

public static partial class VectorArray
{
    [Obsolete("Not tested")]
    public static Vec3D[] MultipliedByMatrixAsPoint3x4(this ReadOnlySpan<Vec3D> vectors, in MatrixD4x4 m)
    {
        Vec3D[] result = new Vec3D[vectors.Length];
        for(int i = 0; i < vectors.Length; i++)
            result[i] = m.MultiplyPoint3x4(vectors[i]);
        return result;
    }
}
