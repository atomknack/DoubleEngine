using System;
using DoubleEngine.Helpers;

namespace DoubleEngine;

public static class __GlobalStatic
{
    private static string s_dataPath;
    private static Action<string> s_logger;
    public static string ApplicationDataPath => 
        s_dataPath==null ? 
        throw new Exception("DoubleEngine data path is not initialized") : 
        s_dataPath;
    public static Action<string> Logger => s_logger;
    public static readonly string EngineLibraryRegenerated;

    static __GlobalStatic() 
    {
        EngineLibraryRegenerated = "11/03/2023 19:07:31";
        s_logger = (string s) => { };
    }
    public static void Init(string applicationDataPath, Action<string> logger)
    {
        s_dataPath = applicationDataPath;
        s_logger = logger;
        Logger($"DoubleEngine Global Init, engine version date: {EngineLibraryRegenerated}");
    }
}
