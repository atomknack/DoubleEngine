using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DoubleEngine_xUnit.Helpers
{
    public class MethodCaller_Tests
    {
        private readonly ITestOutputHelper _output;
        public MethodCaller_Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void MethodCaller_MethodBaseShouldBeEqualItself_Test()
        {
            MethodBaseEquality_RealTestBody();
        }

        private void MethodBaseEquality_RealTestBody()
        {
            var methodBase = DoubleEngine.Helpers.GetCaller.GetMethodCaller();
            methodBase.Should().NotBeNull();
            methodBase.DeclaringType.FullName.Should().Be("DoubleEngine_xUnit.Helpers.MethodCaller_Tests");
            methodBase.Name.Should().Be("MethodCaller_MethodBaseShouldBeEqualItself_Test");

            var methodBase2 = DoubleEngine.Helpers.GetCaller.GetMethodCaller();
            methodBase.Should().Be(methodBase2);
            (methodBase == methodBase2).Should().BeTrue();
        }

        [Fact]
        public void MethodCallerAsStrings_TestAndPrint()
        {
            ThisMethodShoudBeCaller();
        }
        internal void ThisMethodShoudBeCaller()
        {
            CallerGetMethodCallerAsStrings_RealTestBody();
            CallerGetMethodCaller_RealTestBody();

        }
        internal void CallerGetMethodCallerAsStrings_RealTestBody()
        {
            (string fullName, string methodName) =
                DoubleEngine.Helpers.GetCaller.GetMethodCallerAsStrings();
            fullName.Should().NotBeNull();
            methodName.Should().NotBeNull();
            _output.WriteLine(fullName);
            _output.WriteLine(methodName);
            fullName.Should().Be("DoubleEngine_xUnit.Helpers.MethodCaller_Tests");
            methodName.Should().Be("ThisMethodShoudBeCaller");
        }
        internal void CallerGetMethodCaller_RealTestBody()
        {
               var methodBase = DoubleEngine.Helpers.GetCaller.GetMethodCaller();
            methodBase.Should().NotBeNull();
            methodBase.DeclaringType.FullName.Should().Be("DoubleEngine_xUnit.Helpers.MethodCaller_Tests");
            methodBase.Name.Should().Be("ThisMethodShoudBeCaller");
        }

    }
}
