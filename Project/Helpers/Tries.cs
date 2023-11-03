using DoubleEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static class Try
    {
        public static void Action(Action<string> action, string actionParameter, string logName = "do action")
        {
            try
            {
                action(actionParameter);
            }
            catch (Exception ex)
            {
                Logger.Log($"Cannot {logName}: {actionParameter}");
                Logger.LogException(ex);
            }
        }
    }
}
