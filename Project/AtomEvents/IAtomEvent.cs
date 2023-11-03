using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.AtomEvents
{
    public interface IAtomEvent
    {
        public void Subscribe(Action action);
        public void UnSubscribe(Action action);
        public void Publish();
        public Delegate[] GetInvocationList();
    }
    public interface IAtomEvent<T>
    {
        public void Subscribe(Action<T> action);
        public void UnSubscribe(Action<T> action);
        public void Publish(T value);
        public Delegate[] GetInvocationList();
    }
}
