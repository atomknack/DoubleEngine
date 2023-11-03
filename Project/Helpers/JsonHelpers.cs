using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace DoubleEngine
{

public static class JsonHelpers 
{
        public static string ApplicationDataPath => __GlobalStatic.ApplicationDataPath;
        public static string AppendApplicationDataPath(string path) =>
            ApplicationDataPath + "\\" + path;

        public static string ArrayToJsonString<T>(this T[] arr) => JsonConvert.SerializeObject(arr, Formatting.None);

        public static void SaveToJsonFile<T>(T toJson, string fileName)
    {
        File.WriteAllText(fileName, JsonConvert.SerializeObject(toJson, Formatting.Indented));
    }

    public static T LoadFromJsonFile<T>(string fileName)
    {
        string flatNodesSource = File.ReadAllText(fileName);
        return (T)JsonConvert.DeserializeObject(flatNodesSource, typeof(T));
    }
}


}
