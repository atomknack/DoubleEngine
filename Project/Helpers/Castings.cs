using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DoubleEngine;

public static class Castings
{
    public static ReadOnlySpan<int> CastToInt_ReadOnlySpan(this Span<IndexedTri> tris) =>
        CastToInt_ReadOnlySpan((ReadOnlySpan<IndexedTri>)tris);
    public static Span<int> CastToInt_WritableSpan(this Span<IndexedTri> tris) =>
        MemoryMarshal.Cast<IndexedTri, int>(tris);

    public static ReadOnlySpan<int> CastToInt_ReadOnlySpan(this ReadOnlySpan<IndexedTri> tris) =>
        MemoryMarshal.Cast<IndexedTri, int>(tris);
    public static ReadOnlySpan<IndexedTri> CastToIndexedTri_ReadOnlySpan(this Span<int> triangles) =>
        CastToIndexedTri_ReadOnlySpan((ReadOnlySpan<int>)triangles);
    public static ReadOnlySpan<IndexedTri> CastToIndexedTri_ReadOnlySpan(this ReadOnlySpan<int> triangles)
    {
        if (triangles.Length % 3 != 0)
            throw new ArgumentException("triangles should divisible by 3");
        return MemoryMarshal.Cast<int, IndexedTri>(triangles);
    }
    public static ReadOnlySpan<Vec3D> CastToVec3D_ReadOnlySpan(this ReadOnlySpan<double> verticesAsMemoryBlock)
    {
        if (verticesAsMemoryBlock.Length % 3 != 0)
            throw new ArgumentException("verticesAsMemoryBlock should divisible by 3");
        return MemoryMarshal.Cast<double, Vec3D>(verticesAsMemoryBlock);
    }

    // MemoryMarshal
    // public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span)
    //     where TFrom : struct
    //     where TTo : struct
}
