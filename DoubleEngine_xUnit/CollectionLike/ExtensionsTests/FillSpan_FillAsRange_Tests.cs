using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using FluentAssertions_Extensions;
using NUnit.Framework;

namespace DoubleEngine_xUnit.CollectionLike.ExtensionsTests
{

    public class FillSpan_FillAsRange_Tests
    {

        [Fact]
        public static void TestSpanRangeFillDefault()
        {
            int count = 100;
            int[] range = Enumerable.Range(0, count).ToArray();
            ReadOnlySpan<int> rangeSpan = range.AsSpan();

            ReadOnlySpan<int> span = stackalloc int[count].FillAsRange();

            span.ShouldEqual(rangeSpan);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(100, 100)]
        [InlineData(29, 5)]
        public static void TestSpanRangeFill(int start, int count)
        {
            int[] range = Enumerable.Range(start, count).ToArray();
            ReadOnlySpan<int> rangeSpan = range.AsSpan();

            ReadOnlySpan<int> span = stackalloc int[count].FillAsRange(start);

            span.ShouldEqual(rangeSpan);
        }

        [Fact]
        public static void SpanFromNullLengthIsZero()
        {
            new Span<int>(null).Length.Should().Be(0);
            new ReadOnlySpan<int>(null).Length.Should().Be(0);
        }
    }
}
