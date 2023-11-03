using CollectionLike;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentHelpers
{
    public static partial class Extensions
    {
        public static void ISpanableShouldBeEqual<T>(this ISpanable<T> spanable, T[] expectedValues)
        {
            expectedValues.Should().NotBeNull();
            Span<T> span = spanable.AsSpan();
            ReadOnlySpan<T> readonlySpan = spanable.AsReadOnlySpan();
            span.Length.Should().Be(expectedValues.Length);
            readonlySpan.Length.Should().Be(expectedValues.Length);
            span.ToArray().Should().Equal(expectedValues);
            readonlySpan.ToArray().Should().Equal(expectedValues);
        }
        public static void ISpanableFillAndTest<T>(this ISpanable<T> spanable, T fillValue, int expectedSpanLength)
        {
            Span<T> span = spanable.AsSpan();
            ReadOnlySpan<T> readonlySpan = spanable.AsReadOnlySpan();

            span.Length.Should().Be(expectedSpanLength);
            readonlySpan.Length.Should().Be(expectedSpanLength);

            span.Fill(fillValue);

            if (expectedSpanLength != 0)
            {
                span.ToArray().Should().NotBeEmpty().And.OnlyContain(x => x.Equals(fillValue));
                readonlySpan.ToArray().Should().NotBeEmpty().And.OnlyContain(x => x.Equals(fillValue));
            }
        }
    }
}
