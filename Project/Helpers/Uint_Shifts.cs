using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleEngine
{
    [Obsolete("Need testing")]
    public static class Uint_Helpers
    {
        private const int REMAINDER_UInt32_BASE2 = 32 - 1;

        public static string ToBinaryAsString32(this UInt32 num) => Convert.ToString(num, 2).PadLeft(32, '0');
        public static int[] ToBinaryAsArray32(this UInt32 num) => Enumerable.Range(0, 32).Select(i => (Int32)((num >> (REMAINDER_UInt32_BASE2 - i)) & 1)).ToArray();
        public static UInt32 CyclicLeftShiftBase2(this UInt32 n) => (n << 1) | (n >> (REMAINDER_UInt32_BASE2));
        public static UInt32 CyclicRightShiftBase2(this UInt32 n) => (n >> 1) | (n << (REMAINDER_UInt32_BASE2));
    }
}
