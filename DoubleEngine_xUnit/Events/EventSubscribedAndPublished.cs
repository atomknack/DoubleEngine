using DoubleEngine.AtomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit.Events
{
    public partial class EventSubscribedAndPublished
    {
        public int eventSubscriberWasCalledCounter;

        private void Subscriber()
        {
            eventSubscriberWasCalledCounter += 1;
        }

        private void Subscriber_int(int _)
        {
            eventSubscriberWasCalledCounter += 1;
        }
    }
}
