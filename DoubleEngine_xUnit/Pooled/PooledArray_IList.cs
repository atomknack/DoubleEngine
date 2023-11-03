using Collections.Pooled;
using CollectionLike;
using FluentAssertions;
using System.Collections;
using CollectionLike.Pooled;

namespace DoubleEngine_xUnit.Pooled;

public partial class PooledArray_Tests
{
    public class TestData_PooledArrayFill : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 42, -10, 10 };
            yield return new object[] { 142, -10, 10 };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private static void CreateRandomFilledExpectedAndEmptyPooled(
        int arraySize, int minValue, int maxValue, out int[] expected, out PooledArrayStruct<int> pooled)
    {
        expected = TestGenerators.RandArray(arraySize, minValue, maxValue);
        pooled = new PooledArrayStruct<int>(arraySize);
        expected.Should().NotBeNull();
        if (arraySize > 0)
        {
            expected.Should().NotBeEmpty();
        }
    }

    /*
    [Fact]
    public static void Clear_ThrowsNotImplemented_Test()
    {
        PooledArrayStruct<int> pooled = new PooledArrayStruct<int>(10);
        Action clearCall = () => pooled.Clear();
        clearCall.Should().ThrowExactly<NotImplementedException>();
        Action clearCallAsIList = () => ((IList<int>)pooled).Clear();
        clearCallAsIList.Should().ThrowExactly<NotImplementedException>();
    }
    */

    [Theory]
    [InlineData(0, -10, 10)]
    [ClassData(typeof(TestData_PooledArrayFill))]
    public static void Contains_Test(int arraySize, int minValue, int maxValue)
    {
        CreateRandomFilledExpectedAndEmptyPooled(arraySize, minValue, maxValue, out int[] expected, out PooledArrayStruct<int> pooled);
        expected.CopyTo(pooled.AsSpan());
        int numTries = 1000;
        int possibleValueExpansion = 100;
        for (int i = 0; i < numTries; i++)
        {
            int randItem = rand.Next(minValue - possibleValueExpansion, maxValue + possibleValueExpansion);
            if (expected.Contains(randItem))
                pooled.Contains(randItem).Should().BeTrue();
            else
                pooled.Contains(randItem).Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(0, -10, 10)]
    [ClassData(typeof(TestData_PooledArrayFill))]
    public static void ToArray_Test(int arraySize, int minValue, int maxValue)
    {
        CreateRandomFilledExpectedAndEmptyPooled(arraySize, minValue, maxValue, out int[] expected, out PooledArrayStruct<int> pooled);
        pooled.Count.Should().Be(arraySize);
        expected.CopyTo(pooled.AsSpan());
        var actual = pooled.ToArray();
        actual.Should().NotBeNull().And.HaveCount(arraySize);
        actual.Should().Equal(expected);
    }



    [Theory]
    [InlineData(0, -10, 10)]
    [ClassData(typeof(TestData_PooledArrayFill))]
    public static void CopyTo_FromZero_Test(int arraySize, int minValue, int maxValue)
    {
        CreateRandomFilledExpectedAndEmptyPooled(arraySize, minValue, maxValue, out int[] expected, out PooledArrayStruct<int> pooled);
        pooled.Count.Should().Be(arraySize);
        expected.CopyTo(pooled.AsSpan());
        var actual = new int[arraySize];
        pooled.CopyTo(actual, 0);
        actual.Should().Equal(expected);
    }
    [Theory]
    [InlineData(0, -10, 10)]
    [ClassData(typeof(TestData_PooledArrayFill))]
    public static void CopyTo_FromRandom_Test(int arraySize, int minValue, int maxValue)
    {
        CreateRandomFilledExpectedAndEmptyPooled(arraySize, minValue, maxValue, out int[] expected, out PooledArrayStruct<int> pooled);
        pooled.Count.Should().Be(arraySize);
        expected.CopyTo(pooled.AsSpan());
        pooled.Should().Equal(expected);
        int offset = rand.Next(1, 100);
        int[] expectedCopy = new int[arraySize + offset];
        int fillValue = -3;
        expectedCopy.AsSpan().Fill(fillValue);
        expected.CopyTo(expectedCopy, offset);
        int[] pooledCopy = new int[arraySize + offset];
        pooledCopy.AsSpan().Fill(fillValue);
        pooled.CopyTo(pooledCopy, offset);
        pooledCopy.Should().Equal(expectedCopy);
    }


    [Theory]
    [InlineData(0, -10, 10)]
    [ClassData(typeof(TestData_PooledArrayFill))]
    public static void Throws_IndexOutOfRangeException(int arraySize, int minValue, int maxValue)
    {
#pragma warning disable CS0251 // Indexing an array with a negative index
        var expected = TestGenerators.RandArray(arraySize, minValue, maxValue);
        int valueFromOutsideBounds = 0;
        PooledArrayStruct<int> pooled = new PooledArrayStruct<int>(expected.Length);
        Action indexlessThanZero = () => valueFromOutsideBounds = expected[-1];
        indexlessThanZero.Should().ThrowExactly<IndexOutOfRangeException>();
        Action indexOfLength = () => valueFromOutsideBounds = expected[arraySize];
        indexOfLength.Should().ThrowExactly<IndexOutOfRangeException>();
        valueFromOutsideBounds = 1;
        indexlessThanZero = () => expected[-1] = valueFromOutsideBounds;
        indexlessThanZero.Should().ThrowExactly<IndexOutOfRangeException>();
        indexOfLength = () => expected[arraySize] = valueFromOutsideBounds;
        indexOfLength.Should().ThrowExactly<IndexOutOfRangeException>();
    }
    [Theory]
    [InlineData(0, -10, 10)]
    [ClassData(typeof(TestData_PooledArrayFill))]
    public static void Throws_IndexOutOfRangeException_As_IList(int arraySize, int minValue, int maxValue)
    {
        var expected = TestGenerators.RandArray(arraySize, minValue, maxValue);
        int valueFromOutsideBounds = 0;
        IList<int> pooled = new PooledArrayStruct<int>(expected.Length);
        Action indexlessThanZero = () => valueFromOutsideBounds = expected[-1];
        indexlessThanZero.Should().ThrowExactly<IndexOutOfRangeException>();
        Action indexOfLength = () => valueFromOutsideBounds = expected[arraySize];
        indexOfLength.Should().ThrowExactly<IndexOutOfRangeException>();
        valueFromOutsideBounds = 1;
        indexlessThanZero = () => expected[-1] = valueFromOutsideBounds;
        indexlessThanZero.Should().ThrowExactly<IndexOutOfRangeException>();
        indexOfLength = () => expected[arraySize] = valueFromOutsideBounds;
        indexOfLength.Should().ThrowExactly<IndexOutOfRangeException>();
#pragma warning restore CS0251 // Indexing an array with a negative index
    }

    [Theory]
    [ClassData(typeof(TestData_PooledArrayFill))]
    public static void Index_Test(int arraySize, short minValue, short maxValue)
    {
        short[] expected = TestGenerators.RandShortArray(arraySize, minValue, maxValue);
        PooledArrayStruct<short> pooled = new PooledArrayStruct<short>(expected.Length);
        pooled.Count.Should().Be(arraySize);
        short filling = 9;
        pooled.Fill(filling);
        pooled.Should().OnlyContain(x=>x == filling);
        short[] newArray = new short[arraySize];
        for (int i = 0; i < arraySize; i++)
        {
            pooled[i].Should().Be(filling);
            pooled[i] = (short)expected[i];
            newArray[i] = pooled[i];
        }
        var fromPooled = pooled.AsReadOnlySpan().ToArray();
        fromPooled.Should().Equal(expected);
        newArray.Should().Equal(expected);
    }
}
