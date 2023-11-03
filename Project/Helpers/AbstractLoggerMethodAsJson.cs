using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DoubleEngine.Helpers
{
    [Obsolete("NOt Done, in process")]
    public abstract class AbstractLoggerMethodAsJson //TODO
    {
        public record struct Param(int position, string name, object value);
        public record struct MethodLogHeader(string methodName, string methodCaller, string timeStamp, string listOfParamAsJSON);
        public record struct MethodLogInProcess(string name, string timeStamp, string valueAsJSON);
        public record struct MethodLogFooter(string timeStamp, string dictionaryStringObjectAsJSON);


        protected static readonly JsonSerializerSettings s_jsonAll = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        private bool _startedLogging;

        protected string GetTimeStamp()
        {
            var dt = DateTime.Now;
            long timestamp = dt.Ticks % 1000000;
            return String.Format("{0:0000000}", timestamp); ;
        }

        protected AbstractLoggerMethodAsJson()
        {
            _startedLogging = false;
        }

        [Conditional("TESTING")]
        public void StartLogMethod(params object[] callingMethodParamValues)
        {
            if (_startedLogging == true)
                throw new InvalidOperationException("Cannot log method while not finished logging previous");
            _startedLogging = true;

            MethodBase method = new StackFrame(skipFrames: 1).GetMethod();
            ParameterInfo[] methodParams = method.GetParameters();
            MethodBase callerOfcaller = new StackFrame(skipFrames: 2).GetMethod();

            string callerOfLoggedMethod = callerOfcaller == null ? "" : $"{callerOfcaller.DeclaringType.Name}.{callerOfcaller.Name}";

            List<Param> paramList = new List<Param>();
            if (methodParams.Length == callingMethodParamValues.Length)
            {
                //todo, maybe, add check that parameter names, values, positions - are correctly correspond to each other
                foreach (var param in methodParams)
                {
                    paramList.Add(new Param(param.Position, param.Name, callingMethodParamValues[param.Position]));//$"{param.Name}={callingMethodParamValues[param.Position]}");
                }
            }
            else
                throw new ArgumentException("you must pass all argumens of Logged method in same order");

            Log(new MethodLogHeader(
                method.Name, 
                callerOfLoggedMethod, 
                GetTimeStamp(), 
                JsonConvert.SerializeObject(paramList, Formatting.None, s_jsonAll)));
        }

        [Conditional("TESTING")]
        public void InMethodLog<T>(string recordName, T value)
        {
            if (_startedLogging == false)
                throw new InvalidOperationException("Cannot log that was not started");

            Log(new MethodLogInProcess(
                recordName,
                GetTimeStamp(),
                JsonConvert.SerializeObject(value, Formatting.None, s_jsonAll)));
        }

        [Conditional("TESTING")]
        public void EndLogMethod(Dictionary<string,object> endOutput)
        {
            if (_startedLogging == false)
                throw new InvalidOperationException("Cannot end log that was not started");

            Log(new MethodLogFooter(
                GetTimeStamp(), 
                JsonConvert.SerializeObject(endOutput, Formatting.None, s_jsonAll)));
            _startedLogging = false;
        }

        protected abstract void Log(MethodLogHeader header);
        protected abstract void Log(MethodLogInProcess record);
        protected abstract void Log(MethodLogFooter footer);

    }
}
