using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using FluentAssertions_Extensions;
using NUnit.Framework;

namespace DoubleEngine_xUnit.Enumerables;

public class Span_Tests
{
    [Fact]
    public static void SpanFromNullLengthIsZero()
    {
        new Span<int>(null).Length.Should().Be(0);
        new ReadOnlySpan<int>(null).Length.Should().Be(0);
    }
}
