using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DoubleEngine.AtomEvents
{
    [Obsolete("not used now, Should be tested with first use")]
    public class AtomEventBindedWithStoredValue<T1>: AtomEvent<T1>, IAtomEvent<T1>
    {
        private T1 _storedValue = default(T1);
        private MethodBase _allowedCaller = null;
        public T1 Value => _storedValue;
        [Obsolete("Not tested, not used, Yet")]
        new public void Publish(T1 value)
        {
            MethodBase caller = Helpers.GetCaller.GetMethodCaller();
            if (_allowedCaller == null)
                _allowedCaller = caller;
            if (caller != _allowedCaller)
                Guard.Throw.InvalidOperationException("AtomEvent was called with different caller than first time");

            _storedValue = value;
            base.Publish(value);
        }
        [Obsolete("Not tested, not used, Yet")]
        public AtomEventBindedWithStoredValue(T1 initialValue)
        {
            _storedValue = initialValue;
        }
        public AtomEventBindedWithStoredValue()
        {
            _storedValue = default(T1);
        }
    }
}
