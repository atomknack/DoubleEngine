using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using FluentAssertions_Extensions;
using NUnit.Framework;

namespace DoubleEngine_xUnit.CollectionLike.ExtensionsTests;

public class Span_Count_Tests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(9)]
    [InlineData(49)]
    [InlineData(50)]
    [InlineData(51)]
    [InlineData(100)]
    [InlineData(211)]
    [InlineData(1000)]
    public static void Span_Count_Test(int count)
    {
        int first50 = count < 50 ? count : 50;
        int[] array = TestGenerators.RandArray(count, -100, 100);
        //TestContext.WriteLine(String.Join(',',array.Take(50)));
        for (int i = 0; i < first50; i++)
            {
            int countInArray = array.Count(x => x == array[i]);
            countInArray.Should().Be( array.AsReadOnlySpan().Count(x => x == array[i]));
            countInArray.Should().Be( array.AsSpan().Count(x => x == array[i]));
            countInArray.Should().Be( array.AsSpan().Count(array[i]));
            countInArray.Should().Be( array.AsReadOnlySpan().Count(array[i]));
            //TestContext.Write($"{array.AsReadOnlySpan().Count(x => x == array[i])},");
            }
    }
}
