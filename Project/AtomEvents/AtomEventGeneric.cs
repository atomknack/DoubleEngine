using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.AtomEvents;

public class AtomEvent<T1>: IAtomEvent<T1>
{
    private event Action<T1> _action;

    public void Subscribe(Action<T1> action)
    {
        _action += action;
    }

    public void UnSubscribe(Action<T1> action)
    {
        _action -= action;
    }
    public void Publish(T1 value)
    {
        _action?.Invoke(value);
    }

    public Delegate[] GetInvocationList() =>_action?.GetInvocationList() ?? Array.Empty<Delegate>();
}
