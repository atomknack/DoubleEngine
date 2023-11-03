using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit.Helpers
{
    public class Fluent_TestDocumenting
    {
        [Fact]
        public void BeInRangeTest_JustToBeSure()
        {
            10.Should().BeInRange(10, 20);
            10.Should().BeInRange(2, 10);
            10.Should().NotBeInRange(11, 20);
            10.Should().NotBeInRange(2, 9);
        }
    }
}
