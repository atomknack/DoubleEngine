using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit
{
    internal partial class Assert: Xunit.Assert
    {
        internal static void EmptyArray<T>(T[] values)
        {
            Assert.False(values is null);
            Assert.False(values == null);
#pragma warning disable CS8602,xUnit2013 // Do not use equality check to check for collection size.
            Assert.Equal(0, values.Length);
#pragma warning restore CS8602,xUnit2013// Do not use equality check to check for collection size.
            Assert.Empty(values);
            //if (values.Length != 0)
            //    throw new Exception("Length not equal 0");
        }
    }
}
