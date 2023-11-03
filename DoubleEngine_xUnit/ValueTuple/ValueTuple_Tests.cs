using Xunit.Abstractions;
using CollectionLike.Comparers;
using DjvuNet.Tests.Xunit;
using DoubleEngine.Atom;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace DoubleEngine_xUnit.ValueTuple;

public class ValueTuple_Tests
{
    [Fact]
    public void TwoFlatNodeTransforms_Equality()
    {
        (FlatNodeTransform a, FlatNodeTransform b) defDef = (FlatNodeTransform.Default, FlatNodeTransform.Default);
        (FlatNodeTransform a, FlatNodeTransform b) defInvY = (FlatNodeTransform.Default, FlatNodeTransform.InvertedY);
        (FlatNodeTransform ee, FlatNodeTransform eee) defInvY2 = (FlatNodeTransform.Default, FlatNodeTransform.InvertedY);

        defInvY.Should().Be((FlatNodeTransform.Default, FlatNodeTransform.InvertedY));
        (defInvY == (FlatNodeTransform.Default, FlatNodeTransform.InvertedY)).Should().BeTrue();
        defInvY.Should().Be(defInvY2);
        (defInvY == defInvY2).Should().BeTrue();
        defDef.Should().NotBe((FlatNodeTransform.Default, FlatNodeTransform.InvertedY));
        (defDef == (FlatNodeTransform.Default, FlatNodeTransform.InvertedY)).Should().BeFalse();
    }
}
