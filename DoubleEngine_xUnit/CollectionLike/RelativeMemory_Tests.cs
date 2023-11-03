using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using FluentAssertions_Extensions;
using NUnit.Framework;
using CollectionLike.Enumerables;

namespace DoubleEngine_xUnit.CollectionLike;

public class RelativeMemory_Tests
{
    public static readonly int[] s_increasing;
    public static readonly int[] s_decreasing;
    public static readonly int[] s_randomItems;

    static RelativeMemory_Tests()
    {
        Random systemRandom = new Random();
        int testArrayLength = 1000;
        s_increasing = new int[testArrayLength];
        s_decreasing = new int[testArrayLength];
        s_randomItems = new int[testArrayLength];
        s_increasing.AsSpan().FillAsRange();
        for (int i = 0; i < s_increasing.Length; i++)
        {
            s_decreasing[i] = s_increasing[s_increasing.IndexOfLast() - i];
            s_randomItems[i] = systemRandom.Next();
        }
    }

    [Fact]
    public static void Preconditions_Test()
    {
        for (int i = 0; i < s_increasing.Length; i++)
        {
            s_increasing[i].Should().Be(i);
            s_decreasing[i].Should().Be( s_increasing.Length - 1 - i);
            //s_randomItems[i] = systemRandom.Next();
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 1)]
    [InlineData(0, 100)]
    [InlineData(9, 8)]
    [InlineData(10, 0)]
    public static void RelativeMemory_Posive_Creation_Tests(int start, int count)
    {
        Action act = () => RelativeMemory.Create(start, count);
        act.Should().NotThrow();
    }
    [Theory]
    [InlineData(0, -100)]
    [InlineData(-1, 0)]
    [InlineData(-10, 10)]
    [InlineData(-8, -9)]
    [InlineData(-9, -8)]
    public static void RelativeMemory_Negative_Creation_ShouldFail_Tests(int start, int count)
    {
        Action act = () => RelativeMemory.Create(start, count);
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }



    [Theory]
    [InlineData(0, 100)]
    [InlineData(100, 100)]
    [InlineData(29, 5)]
    public static void LikeASpan_Test(int start, int count)
    {
        LikeASpan(s_increasing, start, count);
        LikeASpan(s_decreasing, start, count);
        LikeASpan(s_randomItems, start, count);
    }

    public static void LikeASpan<T>(T[] items, int start, int count)
    {
        Span<T> expectedSpan = items.AsSpan(start, count);
        RelativeMemory relative = RelativeMemory.Create(start, count);
        ReadOnlySpan<T> readOnlyRelativeSpan = relative.GetReadOnlySpan(items);
        readOnlyRelativeSpan.ShouldEqual(expectedSpan);
        Span<T> relativeSpan = relative.GetSpan(items.AsSpan());
        relativeSpan.ShouldEqual(expectedSpan);
    }

    [Theory]
    [InlineData(0, 100, 0)]
    [InlineData(0, 100, 4)]
    [InlineData(100, 100, 0)]
    [InlineData(100, 100, 20)]
    [InlineData(29, 5, 0)]
    [InlineData(29, 5, 1)]
    [InlineData(100, 400, 0)]
    [InlineData(100, 400, 50)]
    public static void SliceStartIsLikeASpan_Test(int start, int count, int sliceStart)
    {
        SliceStartIsLikeSpanSlice(s_increasing, start, count, sliceStart);
        SliceStartIsLikeSpanSlice(s_decreasing, start, count, sliceStart);
        SliceStartIsLikeSpanSlice(s_randomItems, start, count, sliceStart);
    }

    public static void SliceStartIsLikeSpanSlice<T>(T[] items, int start, int count, int sliceStart)
    {
        Span<T> expectedSpan = items.AsSpan(start, count).Slice(sliceStart);
        RelativeMemory relative = RelativeMemory.Create(start, count).Slice(sliceStart);
        ReadOnlySpan<T> readOnlyRelativeSpan = relative.GetReadOnlySpan(items);
        readOnlyRelativeSpan.ShouldEqual(expectedSpan);
        Span<T> relativeSpan = relative.GetSpan(items.AsSpan());
        relativeSpan.ShouldEqual(expectedSpan);
    }

    [Theory]
    [InlineData(0, 100, 0, 10)]
    [InlineData(0, 100, 4, 10)]
    [InlineData(100, 100, 0, 20)]
    [InlineData(100, 100, 20, 20)]
    [InlineData(29, 5, 0, 4)]
    [InlineData(29, 5, 1, 4)]
    [InlineData(100, 400, 0, 100)]
    [InlineData(100, 400, 50, 100)]
    public static void SliceStartCountIsLikeASpan_Test(int start, int count, int sliceStart, int sliceCount)
    {
        SliceStartCountIsLikeSpanSlice(s_increasing, start, count, sliceStart, sliceCount);
        SliceStartCountIsLikeSpanSlice(s_decreasing, start, count, sliceStart, sliceCount);
        SliceStartCountIsLikeSpanSlice(s_randomItems, start, count, sliceStart, sliceCount);
    }

    public static void SliceStartCountIsLikeSpanSlice<T>(T[] items, int start, int count, int sliceStart, int sliceCount)
    {
        Span<T> expectedSpan = items.AsSpan(start, count).Slice(sliceStart, sliceCount);
        RelativeMemory relative = RelativeMemory.Create(start, count).Slice(sliceStart, sliceCount);
        ReadOnlySpan<T> readOnlyRelativeSpan = relative.GetReadOnlySpan(items);
        readOnlyRelativeSpan.ShouldEqual(expectedSpan);
        Span<T> relativeSpan = relative.GetSpan(items.AsSpan());
        relativeSpan.ShouldEqual(expectedSpan);
    }
}
