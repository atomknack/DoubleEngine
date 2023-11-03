using DoubleEngine.AtomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit
{
    public partial class EventWithParameter
    {
        private int testValue;

        private void Inc_TestValue(int n) => testValue += n;
    }
}
