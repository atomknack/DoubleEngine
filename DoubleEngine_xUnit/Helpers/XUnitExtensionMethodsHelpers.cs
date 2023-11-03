using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit.Helpers
{
    public static class XUnitExtensionMethodsHelpers
    {
        /*
        public static IEnumerable<object[]> WrapAs1Parameter<T>(this T[] items)
        {
            foreach (var item in items)
                yield return new object[] { item };
        }*/

        public static IEnumerable<object[]> WrapAs1Parameter<T>(this IEnumerable<T> items)
        {
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
                yield return new object[] { enumerator.Current };
        }
        
        public static IEnumerable<object[]> WrapAs2Parameter<T>(this IEnumerable<T> items)
        {
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item0 = enumerator.Current;
                if (enumerator.MoveNext() == false)
                    throw new Exception("TestInitializer should have even (divisible by 2) number of elements");
                var item1 = enumerator.Current;
                yield return new object[] { item0, item1 };
            }

        }
    }
}
