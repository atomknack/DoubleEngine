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
        public static void TestIEnumerator_TestRevindTestAgain(this IEnumerator<int> enumerator, int expectedSize, int[] expectedValues)
        {
            TestIEnumerator_MoveNext(enumerator, expectedSize, expectedValues);
            enumerator.Reset();
            TestIEnumerator_MoveNext(enumerator, expectedSize, expectedValues);
        }
        public static void TestIEnumerator_ResetInMiddle(this IEnumerator<int> enumerator, int expectedSize, int[] expectedValues)
        {
            expectedSize.Should().BeGreaterThan(2);
            enumerator.MoveNext();
            enumerator.MoveNext();
            expectedValues[1].Should().Be(enumerator.Current);
            enumerator.Reset();
            TestIEnumerator_MoveNext(enumerator, expectedSize, expectedValues);
        }

        public static void TestIEnumerator_MoveNext(this IEnumerator<int> enumerator, int expectedSize, int[] expectedValues)
        {
            int k = 0;
            while (enumerator.MoveNext())
            {
                int value = enumerator.Current;
                value.Should().Be(expectedValues[k++]);
            }
            k.Should().Be(expectedSize);
        }
    }
}
