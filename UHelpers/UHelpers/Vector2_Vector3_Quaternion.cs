using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DoubleEngine.UHelpers;

public static partial class VectorUtil
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Normalized(this Vector2 v) => v.normalized;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Normalized(this Vector3 v) => v.normalized;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Rotate(this Quaternion rotation, Vector3 v) => rotation*v;


}
