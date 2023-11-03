using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DoubleEngine.UHelpers;

public static partial class UnityExtension
{
    public static Matrix4x4 FromOperationRotateThenTranslate(Quaternion rotation, Vector3 translation) =>
        FromOperationScaleThenRotateThenTranslate(Vector3.one, rotation, translation);
    public static Matrix4x4 FromOperationScaleThenRotateThenTranslate(Vector3 scale, Quaternion rotation, Vector3 translation) =>
        Matrix4x4.TRS(translation,rotation,scale);
}
