using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

using DoubleEngine;
using CollectionLike;

namespace DoubleEngine_xUnit.CollectionLike.ExtensionsTests
{
    public class Span_SubsetOf_Tests
    {
        private readonly ITestOutputHelper _output;
        public Span_SubsetOf_Tests(ITestOutputHelper output)
        {
            _output = output;
        }
        [Theory]
        [InlineData(9, 20)]
        [InlineData(10, 20)]
        [InlineData(11, 20)]
        [InlineData(15, 30)]
        [InlineData(20, 20)]
        [InlineData(21, 20)]
        [InlineData(30, 20)]
        [InlineData(31, 20)]
        public void Span_SubsetOf_Test(int subsetSize, int supersetSize) //TODO Refactor
        {
            var subsetArray = TestGenerators.RandArray(subsetSize, 0, 10).Distinct().ToArray();
            var supersetArray = TestGenerators.RandArray(supersetSize, 0, 10).Distinct().ToArray();
            var hashSubset = new HashSet<int>(subsetArray);
            var hashSuperset = new HashSet<int>(supersetArray);
            bool subsetAsSpan = subsetArray.AsReadOnlySpan().SubsetOf(supersetArray.AsReadOnlySpan(), (x, y) => x == y);
            bool subsetAsHashSet = hashSubset.IsSubsetOf(hashSuperset);
            _output.WriteLine($"{subsetAsSpan} as span, {subsetAsHashSet} as hashset");
            subsetAsSpan.Should().Be(hashSubset.IsSubsetOf(hashSuperset));

        }
    }
}
