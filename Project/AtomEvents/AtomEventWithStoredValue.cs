using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.AtomEvents
{
    public class AtomEventWithStoredValue<T1>: AtomEvent<T1>, IAtomEvent<T1>
    {
        private T1 _storedValue = default(T1);
        public T1 Value => _storedValue;
        new public void Publish(T1 value)
        {
            _storedValue = value;
            base.Publish(value);
        }
        [Obsolete("Not tested, not used, Yet")]
        public AtomEventWithStoredValue(T1 initialValue)
        {
            _storedValue = initialValue;
        }
        public AtomEventWithStoredValue()
        {
            _storedValue = default(T1);
        }
    }
}
