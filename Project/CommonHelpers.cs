using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DoubleEngine
{
    public static class CommonHelpers
    {
        public static string ToStringRound5WithDot(double s)
        {
            return Math.Round(s, 5).ToString("0.00000").Replace(",", ".");
        }
        public static string ToStringWithDot(double s, string format = null, IFormatProvider formatProvider = null)
        {
            if (string.IsNullOrEmpty(format))
                format = "F2";
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
                return (s.ToString(format, formatProvider)).Replace(",", ".");
            }
            return (s.ToString(format, formatProvider));
        }
/*
        public static string ExampleTestForEachSpanItem<T>(Span<T> elements)
        {
            string tempBad = "";
            for (var i = 0; i < elements.Length; i++)
            {
                tempBad += elements[i].ToString();
            }
            return tempBad;
        }*/
    }
}
