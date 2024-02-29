using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DoubleEngine_xUnit.Round
{
    public class Round_Tests
    {
        private readonly ITestOutputHelper _output;
        public Round_Tests(ITestOutputHelper output) => _output = output;

        [Fact]

        public void DefaultMathRound_EqualTo_ConvertToInt32_And_MathfRoundToInt()
        {
            System.Random randGen = new();
            for (int i = 0; i < 1000; i++)
            {
                double rand = (randGen.NextDouble() * randGen.Next(0, 100)) - randGen.Next(100);
                float randSingle = (float)rand;
                double rounded = Math.Round((double)randSingle);
                rounded.Should().Be((int)Math.Round(randSingle));
            }
        }

        [Theory]
        ////
        [InlineData(1.5, 2)]
        [InlineData(2.5, 2)]
        [InlineData(1.499999, 1)]
        [InlineData(2.500001, 3)]
        ////
        [InlineData(0.1, 0)]
        [InlineData(0.6, 1)]
        public void Round_zero(double toRound, int rounded)
        {
            _output.WriteLine($"{toRound}; {rounded}");
            double roundedAsDouble = rounded;
            roundedAsDouble.Should().Be((double)Convert.ToInt32(toRound));//, $"troublesome value: {toRound}");
            roundedAsDouble.Should().Be(MathU.Round(toRound, 0));//, $"troublesome value: {toRound}");
        }
        [Theory]
        ////
        [InlineData(1.5, 1.5)]
        [InlineData(2.5, 2.5)]
        [InlineData(1.499999, 1.5)]
        [InlineData(2.500001, 2.5)]
        ////
        [InlineData(0.1, 0.1)]
        [InlineData(0.6, 0.6)]
        public void Round_one(double toRound, double rounded)
        {
            _output.WriteLine($"{toRound}; {rounded}");
            double roundedAsDouble = rounded;
            roundedAsDouble.Should().Be(MathU.Round(toRound, 1));//, $"troublesome value: {toRound}");
        }

    }
}
