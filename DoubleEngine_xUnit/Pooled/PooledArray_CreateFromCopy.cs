using Collections.Pooled;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using FluentAssertions_Extensions;
using NUnit.Framework.Internal;
using Xunit.Abstractions;
using CollectionLike.Pooled;

namespace DoubleEngine_xUnit.Pooled;

public partial class PooledArray_Tests
{

    [Fact]
    public static void CreateAsCopyFromSpan_Test()
    {
        (int zero, int one, int two) = (79345, -237653, 23423);
        PooledArrayStruct<int> pooled = new PooledArrayStruct<int>(3);
        pooled[0] = zero;
        pooled[1] = one;
        pooled[2] = two;
        PooledArrayStruct<int> copy = PooledArrayStruct<int>.CreateAsCopyFromSpan(pooled.AsReadOnlySpan());

        (copy.AsReadOnlySpan() == pooled.AsReadOnlySpan()).Should().BeFalse();
        copy.AsReadOnlySpan().SequenceEqual(pooled.AsReadOnlySpan()).Should().BeTrue();
        copy.AsReadOnlySpan().ShouldEqual(pooled.AsReadOnlySpan());
    }
}
