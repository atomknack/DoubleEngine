using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleEngine.Helpers
{
    public class MethodLoggerToFile : AbstractLoggerMethodAsJson
    {
        string _pathToSaveFiles;
        string _logFilePrefix;
        MethodLogHeader _header;
        List<MethodLogInProcess> _loggedValuesInProcess;
        public MethodLoggerToFile(string pathToSaveFiles, string logFilePrefix) : base()
        {
            _pathToSaveFiles = pathToSaveFiles;
            _logFilePrefix = logFilePrefix;
        }
        public record struct MethodLogAsJson(MethodLogHeader header, List<MethodLogInProcess> loggedInsideMethod, MethodLogFooter footer)
        {
            public string MethodName() => header.methodName;
            public string MethodCaller() => header.methodCaller;
            public string ParamsAsJson() => header.listOfParamAsJSON;
            public List<Param> Params() => JsonConvert.DeserializeObject<List<Param>>(ParamsAsJson());
            public string StartLogTimeStamp() => header.timeStamp;

            public T GetValueLoggedInProcess<T>(string name)
            {
                MethodLogInProcess inProcess = loggedInsideMethod.First(x=>x.name == name);
                return JsonConvert.DeserializeObject<T>(inProcess.valueAsJSON);
            }

            public string endOutputDictAsJson() => footer.dictionaryStringObjectAsJSON;
            public Dictionary<string, object> endOutput => JsonConvert.DeserializeObject<Dictionary<string, object>>(endOutputDictAsJson());
            public string EndLogTimeStamp() => footer.timeStamp;
        };

        protected override void Log(MethodLogHeader header)
        {
            _header = header;
            _loggedValuesInProcess = new();
        }

        protected override void Log(MethodLogInProcess inProcess)
        {
            _loggedValuesInProcess.RemoveAll(x => x.name == inProcess.name);
            _loggedValuesInProcess.Add(inProcess);
        }

        protected override void Log(MethodLogFooter footer)
        {
            JsonHelpers.SaveToJsonFile(
                new MethodLogAsJson(_header, _loggedValuesInProcess, footer), 
                _pathToSaveFiles + _logFilePrefix + _header.timeStamp + ".json");

            _loggedValuesInProcess.Clear();
            _header = new();
        }

        public static MethodLogAsJson LoadFromFile(string path) => JsonHelpers.LoadFromJsonFile<MethodLogAsJson>(path);
    }
}
