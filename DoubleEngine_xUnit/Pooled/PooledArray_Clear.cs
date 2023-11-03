using Collections.Pooled;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using NUnit.Framework.Internal;
using Xunit.Abstractions;
using CollectionLike.Pooled;

namespace DoubleEngine_xUnit.Pooled;

public partial class PooledArray_Tests
{

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(9)]
    [InlineData(99)]
    [InlineData(199)]
    [InlineData(301)]
    public static void Clear_Test(int count)
    {
        int[] randArray = TestGenerators.RandArray(count, -1000, 1000);
        PooledArrayStruct<int> pooled = new PooledArrayStruct<int>(count);
        randArray.AsReadOnlySpan().CopyTo(pooled.AsSpan());
        pooled.AsReadOnlySpan().ToArray().Should().Equal(randArray);
        pooled.Clear();
        ReadOnlySpan<int> pooledSpan = pooled.AsReadOnlySpan();
        pooledSpan.Length.Should().Be(count);
        if (count != 0)
            pooledSpan.ToArray().Should().OnlyContain(x=>x==0);
        
    }
}
