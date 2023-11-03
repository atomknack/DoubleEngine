using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleEngine.Atom;
using CollectionLike;

namespace DoubleEngine_xUnit
{
        internal partial class Assert
        {
        /*
        public static void AreEqual<T>(T[] array, ReadOnlySpan<T> span)
        {
            Assert.AreEqual(array.Length, span.Length);
            Assert.AreEqual(array, span);
            for (int i = 0; i < span.Length; ++i)
            {
                Assert.AreEqual(array[i], span[i]);
            }
        }*/


            public static void AreContainsSameElements<T>(ReadOnlySpan<T> expected, ReadOnlySpan<T> actual) where T : IEquatable<T>
        {
                Equal(expected.Length, actual.Length);
                for (int i = 0; i < expected.Length; i++)
                {
                    var item = expected[i];
                    Equal(expected.Count(x => x.Equals(item)), actual.Count(x => x.Equals(item)));
                }

            }

        /* uncomment if moved testing ThreeDimensionalCell to xUnit
            public static void AreEqual(ThreeDimensionalCell cell, ThreeDimensionalCell other)
            {
                Assert.AreEqual(cell.GetHashCode(), other.GetHashCode());
                Assert.AreEqual(cell.ToBytesArray(), other.ToBytesArray());
                Assert.True(cell == other);
                NUnit.Framework.Assert.AreEqual(cell, other);
            }
            public static void AreNotEqual(ThreeDimensionalCell cell, ThreeDimensionalCell other)
            {
                Assert.AreNotEqual(cell.GetHashCode(), other.GetHashCode());
                Assert.AreNotEqual(cell.ToBytesArray(), other.ToBytesArray());
                Assert.False(cell == other);
                NUnit.Framework.Assert.AreNotEqual(cell, other);
            }
        */

            public static void AreEqual<T>(Span<T> expected, Span<T> actual) =>
                AreEqual((ReadOnlySpan<T>)expected, (ReadOnlySpan<T>)actual);
            public static void AreEqual<T>(ReadOnlySpan<T> expected, ReadOnlySpan<T> actual)
            {
                Equal(expected.Length, actual.Length);
                for (int i = 0; i < expected.Length; i++)
                {
                    Equal(expected[i], actual[i]);
                }
                //Debug.Log($"AssertAreEqual seccessfully for {expected.Length} items");
            }
            public static void AreEqual(IThreeDimensionalGrid expected, IThreeDimensionalGrid actual)
            {
                var dimensions = actual.GetDimensions();
                Assert.Equal(expected.GetDimensions(), dimensions);
                for (int xi = 0; xi < dimensions.x; ++xi)
                    for (int yi = 0; yi < dimensions.y; ++yi)
                        for (int zi = 0; zi < dimensions.z; ++zi)
                            Assert.Equal(expected.GetCell(xi, yi, zi), actual.GetCell(xi, yi, zi));
            }
        }
}
