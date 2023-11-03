/* replaced with t4 codegeneration

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DoubleEngine
{
    public static partial class GenericUtil
    {
        public static T[] AssembleIndices<T>(this T[] items, int[] indicesToAssemble)
        {
            T[] result = new T[indicesToAssemble.Length];
            for (var i = 0; i < indicesToAssemble.Length; i++)
                result[i] = items[indicesToAssemble[i]];
            return result;
        }


        public static T[] AssembleIndices<T>(this ReadOnlySpan<T> vertices, int[] indicesToAssemble)
        {
            T[] result = new T[indicesToAssemble.Length];
            for (var i = 0; i < indicesToAssemble.Length; i++)
                result[i] = vertices[indicesToAssemble[i]];
            return result;
        }
        public static T[] AssembleIndices<T>(this ImmutableArray<T> vertices, ImmutableArray<int> indicesToAssemble)
        {
            T[] result = new T[indicesToAssemble.Length];
            for (var i = 0; i < indicesToAssemble.Length; i++)
                result[i] = vertices[indicesToAssemble[i]];
            return result;
        }
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
        public static void AssembleIndicesToBuffer<T>(this IReadOnlyList<T> vertices, ReadOnlySpan<int> indicesToAssemble, Span<T> buffer)
        {
            for (var i = 0; i < indicesToAssemble.Length; i++)
                buffer[i] = vertices[indicesToAssemble[i]];
        }
        public static void AssembleIndicesToBuffer<T>(this IReadOnlyList<T> vertices, IReadOnlyList<int> indicesToAssemble, Span<T> buffer)
        {
            int Length = indicesToAssemble.Count;
            for (var i = 0; i < Length; i++)
                buffer[i] = vertices[indicesToAssemble[i]];
        }
        //append to reciever
        public static void AppendAssembledIndices<T>(this IList<T> receiver, ReadOnlySpan<T> vertices, ReadOnlySpan<int> indicesToAssemble)
        {
            for (var i = 0; i < indicesToAssemble.Length; i++)
                receiver.Add(vertices[indicesToAssemble[i]]);
        }
        public static void AppendAssembledIndices<T>(this IList<T> receiver, IReadOnlyList<T> vertices, ReadOnlySpan<int> indicesToAssemble)
        {
            for (var i = 0; i < indicesToAssemble.Length; i++)
                receiver.Add(vertices[indicesToAssemble[i]]);
        }
        public static void AppendAssembledIndices<T>(this IList<T> receiver, ReadOnlySpan<T> vertices, IReadOnlyList<int> indicesToAssemble)
        {
            int Length = indicesToAssemble.Count;
            for (var i = 0; i < Length; i++)
                receiver.Add(vertices[indicesToAssemble[i]]);
        }
        public static void AppendAssembledIndices<T>(this IList<T> receiver, IReadOnlyList<T> vertices, IReadOnlyList<int> indicesToAssemble)
        {
            int Length = indicesToAssemble.Count;
            for (var i = 0; i < Length; i++)
                receiver.Add(vertices[indicesToAssemble[i]]);
        }
    }
}
*/