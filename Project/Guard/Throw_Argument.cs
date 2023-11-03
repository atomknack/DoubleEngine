using System;
using System.Linq;
using System.Collections.Generic;

namespace DoubleEngine.Guard;

internal static partial class Throw
{
    public static void ArgumentException(string message)
    {
        throw new ArgumentException(message);
    }
    public static void ArgumentException(string message, string paramName)
    {
        throw new ArgumentException(message, paramName);
    }
    public static void ArgumentOutOfRangeException(string paramName)
    {
        throw new ArgumentOutOfRangeException(paramName);
    }
    public static void ArgumentLengthCannotBeNegative()
    {
        throw new ArgumentOutOfRangeException("length cannot be negative");
    }
    public static void ArgumentOutOfRangeException(string paramName, string message)
    {
        throw new ArgumentOutOfRangeException(paramName, message);
    }

}
