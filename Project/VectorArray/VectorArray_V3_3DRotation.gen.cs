﻿//----------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool. Changes will be lost if the code is regenerated.
// </auto-generated>
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleEngine
{
    public static partial class VectorArray
    {
    //[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double Vec3D Math.Abs(a.x) + Math.Abs(a.y) + Math.Abs(a.z)

    /*
        public static Vector3[] Rotated(this Vector3[] vectors, Quaternion rotation) => 
            vectors.Select(v => v.Rotated(rotation)).ToArray();
        public static void RotateNormalsInPlace(this Vector3[] normals, Quaternion rotation) => 
            normals.RotateInPlaceBy(rotation);
        public static void RotateInPlaceBy(this Vector3[] vectors, Quaternion rotation)
        {
            for (int i = 0; i < vectors.Length; ++i)
                vectors[i] = vectors[i].Rotated(rotation);
        }
        */

// need testing
        public static Vec3D[] Rotated(this Vec3D[] vectors, QuaternionD rotation) => 
            rotation.Rotate(vectors);
// need testing
        public static void RotateNormalsInPlace(this Vec3D[] normals, QuaternionD rotation) => 
            rotation.RotateInPlace(normals);
// need testing
        public static void RotateInPlaceBy(this Vec3D[] vectors, QuaternionD rotation) =>
            rotation.RotateInPlace(vectors);

    }
}