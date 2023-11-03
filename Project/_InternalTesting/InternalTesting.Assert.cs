using System;

namespace DoubleEngine
{
    public static partial class InternalTesting
    {
        internal static class Assert
        {
            internal static void EmptyArray<T>(T[] values)
            {
                if (values.Length!=0)
                    throw new Exception("Length not equal 0");
            }
            internal static void NotNull(object value)
            {
                if (value == null)
                    throw new Exception("Not not null");
            }
            internal static void IsTrue(bool value)
            {
                if (!value)
                    throw new Exception("Not true");
            }
        }
    }
}
