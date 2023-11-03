using CollectionLike.Enumerables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static partial class InternalTesting
    {
        public static partial class Polys
        {
            public static bool PolyShiftedEqual<T>(ReadOnlySpan<T> poly, ReadOnlySpan<T> other, Func<T, T, bool> equals)
            {
                if (poly.Length != other.Length)
                    return false;
                for (int i = 0; i < poly.Length; i++)
                    if (PolyShiftedEqual<T>(poly, other, i, equals))
                        return true;
                return false;
            }
            public static bool PolyShiftedEqual<T>(ReadOnlySpan<T> poly, ReadOnlySpan<T> other)
            {
                if (poly.Length != other.Length)
                    return false;
                for (int i = 0; i < poly.Length; i++)
                    if (PolyShiftedEqual<T>(poly, other, i))
                        return true;
                return false;
            }

            private static bool PolyShiftedEqual<T>(ReadOnlySpan<T> poly, ReadOnlySpan<T> other, int offset, Func<T, T, bool> equals)
            {
                for (int i = 0; i < poly.Length; i++)
                    if ( ! equals( poly[i], other[(i + offset).IndexPositiveCyclicRemapForLength(poly.Length)]))
                        return false;
                return true;
            }
            private static bool PolyShiftedEqual<T>(ReadOnlySpan<T> poly, ReadOnlySpan<T> other, int offset)
            {
                for (int i = 0; i < poly.Length; i++)
                    if ( ! poly[i].Equals(other[(i+offset).IndexPositiveCyclicRemapForLength(poly.Length)]) )
                        return false;
                return true;
            }
        }
    }
}
