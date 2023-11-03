using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Helpers
{
    public static class AbstractLoggerMethod_ParamListExtestions
    {
        public static (string name, object value) GetParamByArgumentPosition(this List<AbstractLoggerMethodAsJson.Param> paramsList, int argumentPosition)
        {
            foreach (var param in paramsList)
            {
                if (param.position == argumentPosition)
                    return (param.name, param.value);
            }
            throw new ArgumentException("no param with such position in arguments");
        }
        public static (int position, object value) GetParamByName(this List<AbstractLoggerMethodAsJson.Param> paramsList, string name)
        {
            foreach (var param in paramsList)
            {
                if (param.name == name)
                    return (param.position, param.value);
            }
            throw new ArgumentException("no param with such name in arguments");
        }
    }
}
