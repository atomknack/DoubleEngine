using Collections.Pooled;
using DoubleEngine;
using CollectionLike;
using CollectionLike.Pooled;
using FluentAssertions;
using NUnit.Framework.Internal;
using Xunit.Abstractions;

namespace DoubleEngine_xUnit.Pooled;

public partial class PooledList_Extension_Tests
{

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(9, 0)]
    [InlineData(9, 1)]
    [InlineData(9, 4)]
    [InlineData(9, 8)]
    [InlineData(9, 9)]
    [InlineData(9, 10)]
    [InlineData(20, 10)]
    [InlineData(99, 90)]
    [InlineData(199, 0)]
    [InlineData(199, 90)]
    [InlineData(199, 198)]
    [InlineData(199, 199)]
    [InlineData(199, 200)]
    public static void ClearListStartingFrom_Test(int count, int clearStartingFrom)
    {
        clearStartingFrom.Should().BeGreaterThanOrEqualTo(0);
        int[] randArray = TestGenerators.RandArray(count, -1000, 1000);
        PooledList<int> pooled = new PooledList<int>(randArray);
        pooled.Count.Should().Be(count);
        pooled.Clear(clearStartingFrom);
        if (clearStartingFrom < count)
        {
            pooled.Count.Should().Be(clearStartingFrom);
            pooled.AsReadOnlySpan().ToArray().Should().Equal(
                randArray.AsSpan().Slice(0, clearStartingFrom).ToArray());
        }
        else
        {
            pooled.Count.Should().Be(count);
            pooled.AsSpan().ToArray().Should().Equal(randArray);
        }
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(1, -10)]
    [InlineData(9, -7)]
    [InlineData(9, -9)]
    [InlineData(9, -100)]
    [InlineData(199, -1)]
    public static void ClearListStartingFrom_Negative_Throw_Test(int count, int clearStartingFrom)
    {
        int[] randArray = TestGenerators.RandArray(count, -1000, 1000);
        PooledList<int> pooled = new PooledList<int>(randArray);
        pooled.Count.Should().Be(count);
        Action act = ()=>pooled.Clear(clearStartingFrom);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
