using System;

namespace DoubleEngine.Helpers;

internal static class Logger
{
    internal static void Log(string s)
    {
        __GlobalStatic.Logger(s);
        //Debug.Log(s);
    }
    internal static void DebugLog(string s)
    {
#if DEBUG
        Log(s);
#endif
    }
    internal static void LogException(Exception ex)
    {
        Log(ex.ToString());
    }
}