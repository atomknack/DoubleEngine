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
        [Fact]
        public void SubscribedPublishedAndCalled_AtomEventWithStoredValue_int()
        {
            eventSubscriberWasCalledCounter = 0;
            AtomEventWithStoredValue<int> ev = new();
            ev.Subscribe(Subscriber_int);
            Assert.Equal(0, eventSubscriberWasCalledCounter);
            ev.Publish(99);
            Assert.Equal(1, eventSubscriberWasCalledCounter);
            ev.UnSubscribe(Subscriber_int);
            Assert.Equal(1, eventSubscriberWasCalledCounter);
            ev.Publish(99);
            Assert.Equal(1, eventSubscriberWasCalledCounter);
        }
    }
}
