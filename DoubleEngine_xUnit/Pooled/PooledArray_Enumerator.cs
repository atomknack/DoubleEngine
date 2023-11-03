using Collections.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using DoubleEngine_xUnit.Helpers;
using FluentHelpers;
using CollectionLike.Pooled;

namespace DoubleEngine_xUnit.Pooled;

public partial class PooledArray_Tests
{
    [Fact]
    public void Enumerator_shouldBeStruct()
    {
        PooledArrayStruct<int> pooled = new PooledArrayStruct<int>(20);
        pooled.GetEnumerator().GetType().IsValueType.Should().BeTrue();
    }
    [Fact]
    public void Enumerator_TestIEnumerator_TestRevindTestAgain_Test()
    {
        int expectedSize = 42;
        CreateRandomArrayAndFillPooledFromIt(expectedSize, out int[] randArray, out PooledArrayStruct<int> pooled);

        IEnumerator<int> enumerator = pooled.GetEnumerator();
        enumerator.TestIEnumerator_TestRevindTestAgain(expectedSize, randArray);
    }
    [Fact]
    public void Enumerator_MoveNext_Test()
    {
        int expectedSize = 37;
        CreateRandomArrayAndFillPooledFromIt(expectedSize, out int[] randArray, out PooledArrayStruct<int> pooled);

        IEnumerator<int> enumerator = pooled.GetEnumerator();
        enumerator.TestIEnumerator_MoveNext(expectedSize, randArray);
    }
    [Fact]
    public void Enumerator_ResetInMiddle_Test()
    {
        int expectedSize = 37;
        CreateRandomArrayAndFillPooledFromIt(expectedSize, out int[] randArray, out PooledArrayStruct<int> pooled);

        IEnumerator<int> enumerator = pooled.GetEnumerator();
        enumerator.TestIEnumerator_ResetInMiddle(expectedSize, randArray);
    }

    [Fact]
    public void Enumerator_foreach_Test()
    {
        int expectedSize = 37;
        CreateRandomArrayAndFillPooledFromIt(expectedSize, out int[] randArray, out PooledArrayStruct<int> pooled);

        int k = -1;
        foreach (var value in pooled)
            value.Should().Be(randArray[++k]);
    }

    private static void CreateRandomArrayAndFillPooledFromIt(int expectedSize, out int[] randArray, out PooledArrayStruct<int> pooled)
    {
        randArray = TestGenerators.RandArray(expectedSize, -10, 30);
        pooled = new PooledArrayStruct<int>(expectedSize);
        for (int i = 0; i < randArray.Length; i++)
            pooled[i] = randArray[i];
    }
}
