using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DoubleEngine.UHelpers
{
    public static partial class UnityExtension
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Scaled(this Vector3 v, Vector3 b) => new Vector3(v.x * b.x, v.y * b.y, v.z * b.z);
        public static Vector3? Scaled(this Vector3? v, Vector3 b) => v == null ? b : v.Value.Scaled(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Rotated(this Vector3 v, Quaternion rotation) => rotation * v;

        //[Obsolete("Not tested enough, maybe should be other way around like TRS.Rotate)")]
        //public static Quaternion Rotated(this Quaternion from, Quaternion rotation) => from * rotation;
        //public static Quaternion? Rotated(this Quaternion? from, Quaternion rotation) => from == null ? rotation : from.Value.Rotated(rotation);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Translated(this Vector3 v, Vector3 translation) => v + translation;
        public static Vector3? Translated(this Vector3? v, Vector3 translation) => v == null ? translation : v.Value.Translated(translation);
    }
}