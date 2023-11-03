using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using FluentAssertions_Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace DoubleEngine_xUnit.SelfTest;

public static class SpanEquality_SelfTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    [InlineData(40)]
    [InlineData(100)]
    public static void Random_ShouldEqual_Assert_Equal(int len)
    {
        var random = new Random();
        int[] arr1 = TestGenerators.RandArray(len, -10, 10);
        int[] arr2 = TestGenerators.RandArray(len, -10, 10);
        Arrays_Spans_Equality_Self_Test(arr1, arr2);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    [InlineData(40)]
    public static void Positive_ShouldEqual_Assert_Equal(int len)
    {
        var random = new Random(42);
        int[] arr1 = TestGenerators.RandArray(len, -5, 5);
        int[] arr2 = (int[])arr1.Clone();
        Arrays_Spans_Equality_Self_Test(arr1, arr2);
        ArrsEqual_PositiveCase(arr1, arr2);
    }

    private static void Arrays_Spans_Equality_Self_Test(int[] arr1, int[] arr2)
    {
        bool arrsEquals = ArrsEqual(arr1, arr2);
        if (arrsEquals)
            ArrsEqual_PositiveCase(arr1, arr2);
        else
        {
            Action act1 = () => arr1.AsReadOnlySpan().ShouldEqual(arr2.AsReadOnlySpan());
            act1.Should().ThrowExactly<XunitException>();
            Action act2 = () => Assert.AreEqual(arr1.AsReadOnlySpan(), arr2.AsReadOnlySpan());
            act2.Should().ThrowExactly<EqualException>();
            Action act3 = () => arr1.AsSpan().ShouldEqual(arr2.AsSpan());
            act3.Should().ThrowExactly<XunitException>();
            Action act4 = () => Assert.AreEqual(arr1.AsSpan(), arr2.AsSpan());
            act4.Should().ThrowExactly<EqualException>();
        }
    }

    private static void ArrsEqual_PositiveCase(int[] arr1, int[] arr2)
    {
        ReadOnlySpan<int> roSpan1 = arr1;
        ReadOnlySpan<int> roSpan2 = arr2;
        ArrsEqual(arr1, arr2).Should().BeTrue();
        roSpan1.ShouldEqual(roSpan2);
        Assert.AreEqual(roSpan1, roSpan2);

        arr1.AsSpan().ShouldEqual(arr2.AsSpan());
        Assert.AreEqual(arr1.AsSpan(), arr2.AsSpan());
    }

    private static bool ArrsEqual(int[] arr1, int[] arr2)
    {
        if (arr1.Length != arr2.Length)
            return false;
        for(int i = 0; i < arr1.Length; i++)
            if (arr1[i] != arr2[i])
                return false;
        return true;
    }
}
