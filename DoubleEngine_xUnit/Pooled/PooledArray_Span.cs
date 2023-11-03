using CollectionLike.Pooled;
using Collections.Pooled;
using FluentAssertions;
using FluentHelpers;

namespace DoubleEngine_xUnit.Pooled;

public partial class PooledArray_Tests
{
    [Fact]
    public void PooledArray_AsSpan()
    {
        int testLength = 20;
        PooledArrayStruct<int> pooled = new(testLength);
        int fillValue = 8;
        pooled.ISpanableFillAndTest(fillValue, testLength);
        pooled.Should().NotBeNullOrEmpty().And.OnlyContain(x => x == fillValue);

    }

}
