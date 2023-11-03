using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleEngine
{
    public static class Int_Helpers
    {
        public static byte NextByteCyclic(this byte num, int maxExclusive) =>
            (byte)(num < (maxExclusive - 1) ? num + 1 : 0);
        public static int NextIntCyclic(this int num, int maxExclusive, int minInclusive = 0)
        {
            if (minInclusive > maxExclusive)
                throw new System.ArgumentOutOfRangeException($"{minInclusive} is bigger than {maxExclusive}");

            return num < (maxExclusive - 1) ? num + 1 : minInclusive;
        }
        public static short NextShortCyclic(this short num, short maxExclusive) =>
            (short)NextIntCyclic(num, maxExclusive);
        public static void NextIntCyclicRef(this ref int num, int maxExclusive)
        {
            num = num < (maxExclusive - 1) ? num + 1 : 0;
        } 
        
    }
}
