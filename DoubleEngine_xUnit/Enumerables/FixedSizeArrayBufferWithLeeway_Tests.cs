using CollectionLike.Enumerables;
using FluentAssertions;
using FluentHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit.Enumerables
{
    public class FixedSizeArrayBufferWithLeeway_Tests
    {
        [Fact]
        public void Clear_Test()
        {
            int expectedSize = 7;
            int overfill = 20;

            int[] randArray = TestGenerators.RandArray(expectedSize + 100 + overfill, -10, 30);
            FixedSizeArrayBufferWithLeeway<int> buffer = new(expectedSize);
            AddFromRandArrayAndTestAsISpanable(expectedSize, randArray, buffer);
            buffer.Clear();
            AddFromRandArrayWithOverfillAndTestAsISpanable(expectedSize, overfill, randArray, buffer);
            buffer.Clear();
            buffer.Count.Should().Be(0);
        }
        [Fact]
        public void CreateAndAddUntilFullAnd20More()
        {
            int expectedSize = 7;
            int overfill = 20;

            int[] randArray = TestGenerators.RandArray(expectedSize + 100, -10, 30);
            FixedSizeArrayBufferWithLeeway<int> buffer = new(expectedSize);
            AddFromRandArrayWithOverfillAndTestAsISpanable(expectedSize, overfill, randArray, buffer);
        }

        [Fact] 
        public void CreateAndAddUntilFull()
        {
            int expectedSize = 18;

            int[] randArray = TestGenerators.RandArray(expectedSize + 100, -10, 30);
            FixedSizeArrayBufferWithLeeway<int> buffer = new(expectedSize);
            AddFromRandArrayAndTestAsISpanable(expectedSize, randArray, buffer);
        }

        [Fact]
        public void LeewaySize_Test()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            FixedSizeArrayBufferWithLeeway<int>.TESTING_GetADDITIONALLEEWAY().Should().BeInRange(30, 1000, "less than 30 is cheapish, more than 1000 is too big to waste");
#pragma warning restore CS0618 // Type or member is obsolete
        }
        [Fact]
        public void FixedSizeArrayBufferWithLeeway_ISpanableFill()
        {
            FixedSizeArrayBufferWithLeeway<short> buffer = new FixedSizeArrayBufferWithLeeway<short>(14);
            buffer.Count.Should().Be(0);
            short[] testValues = new short[] { 18, 0, -10, 34 };
            for (int i = 0; i < testValues.Length; i++)
                buffer.Add(testValues[i]);
            buffer.AsSpan().ToArray().Should().Equal(testValues);
            buffer.ISpanableFillAndTest<short>(85, 4);
        }

        private static void AddFromRandArrayWithOverfillAndTestAsISpanable(int expectedSize, int overfill, int[] randArray, FixedSizeArrayBufferWithLeeway<int> buffer)
        {
            AddFromRandArrayAndTestAsISpanable(expectedSize, randArray, buffer);
            for (int k = 0; k < overfill; k++)
                buffer.Add(randArray[expectedSize + k]);
            buffer.ISpanableShouldBeEqual(randArray.AsSpan(0, expectedSize + overfill).ToArray());
        }

        private static void AddFromRandArrayAndTestAsISpanable(int expectedSize, int[] randArray, FixedSizeArrayBufferWithLeeway<int> buffer)
        {
            buffer.Count.Should().Be(0);
            int i = -1;
            while (buffer.CanAddMore())
                buffer.Add(randArray[++i]);
            buffer.ISpanableShouldBeEqual(randArray.AsSpan(0, expectedSize).ToArray());
        }
    }
}
