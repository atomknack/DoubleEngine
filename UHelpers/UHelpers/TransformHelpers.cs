using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DoubleEngine.UHelpers;

public static class TransformHelpers
{
    public static void UpdateFromTRS3D(this Transform transform, TRS3D trs)
    {
        transform.position = trs.translation.ToVector3();
        transform.rotation = trs.rotation.ToQuaternion();
        transform.localScale = trs.scale.ToVector3();
    }
}
