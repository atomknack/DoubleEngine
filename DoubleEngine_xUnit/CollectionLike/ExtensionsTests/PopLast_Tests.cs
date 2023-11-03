using DjvuNet.Tests.Xunit;
using DoubleEngine_xUnit.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collections.Pooled;
using CollectionLike;

namespace DoubleEngine_xUnit.CollectionLike.ExtensionsTests
{
    public class PopLast_Tests
    {
        static int[] numOfElements = new[] { 0, 1, 2, 9, 99, 113, 260 };
        public static IEnumerable<object[]> NumOfElements => numOfElements.WrapAs1Parameter();

        [DjvuTheory]
        [MemberData(nameof(NumOfElements))]
        public static void PopLast_PooledListInt_Test(int count)
        {
            int[] filled = TestGenerators.RandArray(count, -10, 1000);
            using PooledList<int> list = new PooledList<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(filled[i]);
            }
            for (int i = 0; i < count; i++)
            {
                int expectedCount = count - i;
                list.Count.Should().Be(expectedCount);
                list.ToArray().Should().Equal(filled.AsSpan(0, expectedCount).ToArray());
                int ShouldBePoppedIndex = expectedCount - 1;
                if (expectedCount > 0)
                {
                    list.PopLast().Should().Be(filled[ShouldBePoppedIndex]);
                }
                else
                {
                    list.Count.Should().Be(0);
                }
            }
            list.Count.Should().Be(0);
        }

        [Fact]
        public static void TryPopLast_FromNull_Test()
        {
            int popped = -89;
            PooledList<int> listNull = null;
            listNull.TryPopLast(out popped).Should().BeFalse();
            popped.Should().Be(0);
        }
            [Fact]
        public static void TryPopLast_Test()
        {
            int popped = 0;
            using PooledList<int> list = new PooledList<int>();
            list.TryPopLast(out popped).Should().BeFalse();
            list.Add(78);
            list.Add(42);
            list.TryPopLast(out popped).Should().BeTrue();
            popped.Should().Be(42);
            list.TryPopLast(out popped).Should().BeTrue();
            popped.Should().Be(78);
            list.TryPopLast(out popped).Should().BeFalse();
            list.Add(-9);
            list.TryPopLast(out popped).Should().BeTrue();
            popped.Should().Be(-9);
            list.TryPopLast(out popped).Should().BeFalse();
            list.Count.Should().Be(0);
        }
    }
}
