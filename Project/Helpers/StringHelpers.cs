using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Helpers;

public static class StringHelpers
{
    public static bool TryGetFirstStringBetweenCharacters(this string input, char charFrom, char charTo, out string @out)
    {
        int posFrom = input.IndexOf(charFrom);
        if (posFrom < 0)
        {
            @out = String.Empty;
            return false;
        }
        int posTo = input.IndexOf(charTo, posFrom + 1);
        if (posTo < 0)
        {
            @out = String.Empty;
            return false;
        }
        @out = input.Substring(posFrom + 1, posTo - posFrom - 1);
        return true;
    }
}
