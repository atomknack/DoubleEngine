using System;
using System.Linq;
using System.Collections.Generic;

namespace DoubleEngine.Guard;

internal static partial class Throw
{
    public static void InvalidOperationException(string message)
    {
        throw new InvalidOperationException(message);
    }
    
}
