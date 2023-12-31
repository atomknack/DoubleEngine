﻿//----------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool. Changes will be lost if the code is regenerated.
// </auto-generated>
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Collections.Pooled;

namespace DoubleEngine;

public static partial class VectorUtil
{
    // update tested in Enumerables.AssembleIndices_Tests // [Obsolete("Not tested, need testing")]
    public static T[] AssembleIndices<T>(this T[] vertices, int[] indicesToAssemble)
    {
        T[] result = new T[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static T[] AssembleIndices<T>(this ReadOnlySpan<T> vertices, int[] indicesToAssemble)
    {
        T[] result = new T[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
// generated
    public static T[] AssembleIndices<T>(this ReadOnlySpan<T> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        T[] result = new T[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer<T>(this ReadOnlySpan<T> vertices, ReadOnlySpan<int> indicesToAssemble, Span<T> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static T[] AssembleIndices<T>(this ReadOnlySpan<T> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        T[] result = new T[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer<T>(this ReadOnlySpan<T> vertices, IReadOnlyList<int> indicesToAssemble, Span<T> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static T[] AssembleIndices<T>(this IReadOnlyList<T> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        T[] result = new T[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer<T>(this IReadOnlyList<T> vertices, ReadOnlySpan<int> indicesToAssemble, Span<T> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static T[] AssembleIndices<T>(this IReadOnlyList<T> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        T[] result = new T[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer<T>(this IReadOnlyList<T> vertices, IReadOnlyList<int> indicesToAssemble, Span<T> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static void AppendAssembledIndices<T>(this IList<T> receiver, ReadOnlySpan<T> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices<T>(this IList<T> receiver, IList<T> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices<T>(this IList<T> receiver, ReadOnlySpan<T> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices<T>(this IList<T> receiver, IReadOnlyList<T> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices<T>(this PooledList<T> receiver, ReadOnlySpan<T> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices<T>(this PooledList<T> receiver, IList<T> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices<T>(this PooledList<T> receiver, ReadOnlySpan<T> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices<T>(this PooledList<T> receiver, IReadOnlyList<T> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    // update tested in Enumerables.AssembleIndices_Tests // [Obsolete("Not tested, need testing")]
    public static Vec2D[] AssembleIndices(this Vec2D[] vertices, int[] indicesToAssemble)
    {
        Vec2D[] result = new Vec2D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static Vec2D[] AssembleIndices(this ReadOnlySpan<Vec2D> vertices, int[] indicesToAssemble)
    {
        Vec2D[] result = new Vec2D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
// generated
    public static Vec2D[] AssembleIndices(this ReadOnlySpan<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        Vec2D[] result = new Vec2D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this ReadOnlySpan<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble, Span<Vec2D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec2D[] AssembleIndices(this ReadOnlySpan<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        Vec2D[] result = new Vec2D[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this ReadOnlySpan<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble, Span<Vec2D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec2D[] AssembleIndices(this IReadOnlyList<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        Vec2D[] result = new Vec2D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this IReadOnlyList<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble, Span<Vec2D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec2D[] AssembleIndices(this IReadOnlyList<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        Vec2D[] result = new Vec2D[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this IReadOnlyList<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble, Span<Vec2D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static void AppendAssembledIndices(this IList<Vec2D> receiver, ReadOnlySpan<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec2D> receiver, IList<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec2D> receiver, ReadOnlySpan<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec2D> receiver, IReadOnlyList<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec2D> receiver, ReadOnlySpan<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec2D> receiver, IList<Vec2D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec2D> receiver, ReadOnlySpan<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec2D> receiver, IReadOnlyList<Vec2D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    // update tested in Enumerables.AssembleIndices_Tests // [Obsolete("Not tested, need testing")]
    public static Vec3D[] AssembleIndices(this Vec3D[] vertices, int[] indicesToAssemble)
    {
        Vec3D[] result = new Vec3D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static Vec3D[] AssembleIndices(this ReadOnlySpan<Vec3D> vertices, int[] indicesToAssemble)
    {
        Vec3D[] result = new Vec3D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
// generated
    public static Vec3D[] AssembleIndices(this ReadOnlySpan<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        Vec3D[] result = new Vec3D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this ReadOnlySpan<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble, Span<Vec3D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec3D[] AssembleIndices(this ReadOnlySpan<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        Vec3D[] result = new Vec3D[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this ReadOnlySpan<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble, Span<Vec3D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec3D[] AssembleIndices(this IReadOnlyList<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        Vec3D[] result = new Vec3D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this IReadOnlyList<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble, Span<Vec3D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec3D[] AssembleIndices(this IReadOnlyList<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        Vec3D[] result = new Vec3D[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this IReadOnlyList<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble, Span<Vec3D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static void AppendAssembledIndices(this IList<Vec3D> receiver, ReadOnlySpan<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec3D> receiver, IList<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec3D> receiver, ReadOnlySpan<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec3D> receiver, IReadOnlyList<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec3D> receiver, ReadOnlySpan<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec3D> receiver, IList<Vec3D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec3D> receiver, ReadOnlySpan<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec3D> receiver, IReadOnlyList<Vec3D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    // update tested in Enumerables.AssembleIndices_Tests // [Obsolete("Not tested, need testing")]
    public static Vec4D[] AssembleIndices(this Vec4D[] vertices, int[] indicesToAssemble)
    {
        Vec4D[] result = new Vec4D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static Vec4D[] AssembleIndices(this ReadOnlySpan<Vec4D> vertices, int[] indicesToAssemble)
    {
        Vec4D[] result = new Vec4D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
// generated
    public static Vec4D[] AssembleIndices(this ReadOnlySpan<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        Vec4D[] result = new Vec4D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this ReadOnlySpan<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble, Span<Vec4D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec4D[] AssembleIndices(this ReadOnlySpan<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        Vec4D[] result = new Vec4D[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this ReadOnlySpan<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble, Span<Vec4D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec4D[] AssembleIndices(this IReadOnlyList<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        Vec4D[] result = new Vec4D[indicesToAssemble.Length];
        for (var i = 0; i < indicesToAssemble.Length; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this IReadOnlyList<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble, Span<Vec4D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static Vec4D[] AssembleIndices(this IReadOnlyList<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        Vec4D[] result = new Vec4D[indicesToAssemble.Count];
        for (var i = 0; i < indicesToAssemble.Count; i++)
            result[i] = vertices[indicesToAssemble[i]];
        return result;
    }
    public static void AssembleIndicesToBuffer(this IReadOnlyList<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble, Span<Vec4D> buffer)
    {
        for (var i = 0; i < indicesToAssemble.Count; i++)
            buffer[i] = vertices[indicesToAssemble[i]];
    }
    public static void AppendAssembledIndices(this IList<Vec4D> receiver, ReadOnlySpan<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec4D> receiver, IList<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec4D> receiver, ReadOnlySpan<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this IList<Vec4D> receiver, IReadOnlyList<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec4D> receiver, ReadOnlySpan<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec4D> receiver, IList<Vec4D> vertices, ReadOnlySpan<int> indicesToAssemble)
    {
        for (var i = 0; i < indicesToAssemble.Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec4D> receiver, ReadOnlySpan<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
    public static void AppendAssembledIndices(this PooledList<Vec4D> receiver, IReadOnlyList<Vec4D> vertices, IReadOnlyList<int> indicesToAssemble)
    {
        int Length = indicesToAssemble.Count;
        for (var i = 0; i < Length; i++)
            receiver.Add( vertices[indicesToAssemble[i]] );
    }
}
