﻿//----------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a Conversion_VectorLike_To.tt. 
//     Changes will be lost if the code is regenerated.
// </auto-generated>
//----------------------------------------------------------------------------------------

using UnityEngine;

namespace DoubleEngine.UHelpers;

public static partial class Conversion
{
    public static Vec2D ToVec2D(this Vector2 from) => 
        new Vec2D( from.x, from.y );
    public static Vector2 ToVector2(this Vec2D from) => 
        new Vector2( (float)from.x, (float)from.y );

    public static Vec3D ToVec3D(this Vector3 from) => 
        new Vec3D( from.x, from.y, from.z );
    public static Vector3 ToVector3(this Vec3D from) => 
        new Vector3( (float)from.x, (float)from.y, (float)from.z );

    public static Vec4D ToVec4D(this Vector4 from) => 
        new Vec4D( from.x, from.y, from.z, from.w );
    public static Vector4 ToVector4(this Vec4D from) => 
        new Vector4( (float)from.x, (float)from.y, (float)from.z, (float)from.w );

    public static QuaternionD ToQuaternionD(this Quaternion from) => 
        new QuaternionD( from.x, from.y, from.z, from.w );
    public static Quaternion ToQuaternion(this QuaternionD from) => 
        new Quaternion( (float)from.x, (float)from.y, (float)from.z, (float)from.w );

    public static Vector2Int ToVector2Int(this Vec2I from) => 
        new Vector2Int( from.x, from.y );
    public static Vec2I ToVec2I(this Vector2Int from) => 
        new Vec2I( from.x, from.y );

    public static Vector3Int ToVector3Int(this Vec3I from) => 
        new Vector3Int( from.x, from.y, from.z );
    public static Vec3I ToVec3I(this Vector3Int from) => 
        new Vec3I( from.x, from.y, from.z );

}
