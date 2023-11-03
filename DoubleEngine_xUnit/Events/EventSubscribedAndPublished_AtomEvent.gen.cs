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
        public void SubscribedPublishedAndCalled_AtomEvent()
        {
            eventSubscriberWasCalledCounter = 0;
            AtomEvent ev = new();
            ev.Subscribe(Subscriber);
            Assert.Equal(0, eventSubscriberWasCalledCounter);
            ev.Publish();
            Assert.Equal(1, eventSubscriberWasCalledCounter);
            ev.UnSubscribe(Subscriber);
            Assert.Equal(1, eventSubscriberWasCalledCounter);
            ev.Publish();
            Assert.Equal(1, eventSubscriberWasCalledCounter);
        }
    }
}
