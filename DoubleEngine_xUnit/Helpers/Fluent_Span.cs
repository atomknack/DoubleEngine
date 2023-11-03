using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions
{
    public static partial class Extensions
    {
        public static ReadOnlySpanAssertions<T> Should<T>(this ReadOnlySpan<T> span) => new(span);

        public static ReadOnlySpanAssertions<T> Should<T>(this Span<T> span) => new(span);
    }

    public readonly ref struct ReadOnlySpanAssertions<T>
    {
        private readonly ReadOnlySpan<T> span;

        public ReadOnlySpanAssertions(ReadOnlySpan<T> span)
        {
            this.span = span;
        }

        public SpanAndConstraint<T> NotBeEmpty()
        {
            if (span.IsEmpty)
            {
                throw new Exception("Did not expect span to be empty");
            }

            return new(this);
        }

        public SpanAndWhichConstraint<T> ContainSingle()
        {
            if (span.Length != 1)
            {
                throw new Exception("Expected span to contain a single element");
            }

            return new(this, span[0]);
        }
    }

    public readonly ref struct SpanAndConstraint<T>
    {
        public ReadOnlySpanAssertions<T> And { get; }

        public SpanAndConstraint(ReadOnlySpanAssertions<T> parentConstraint)
        {
            And = parentConstraint;
        }
    }

    public readonly ref struct SpanAndWhichConstraint<T>
    {
        public ReadOnlySpanAssertions<T> And { get; }

        public T Which { get; }

        public SpanAndWhichConstraint(ReadOnlySpanAssertions<T> parentConstraint, T whichValue)
        {
            And = parentConstraint;
            Which = whichValue;
        }
    }
}
