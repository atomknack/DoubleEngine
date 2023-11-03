using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace DoubleEngine.Helpers
{
    public static class GetCaller
    {
        private const int frameNumber = 2; // frame 2 to get caller of this method caller,
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static (string fullName, string methodName) GetMethodCallerAsStrings()
        {
            StackFrame frame = new StackFrame(frameNumber, false); //false for no source info
            var method = frame.GetMethod();

            return (method.DeclaringType.FullName, method.Name);
        }
        public static MethodBase GetMethodCaller()
        {
            StackFrame frame = new StackFrame(frameNumber, false); //false for no source info
            return frame.GetMethod();
        }
    }
}
