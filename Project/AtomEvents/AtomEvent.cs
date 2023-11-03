using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.AtomEvents;

//redo all events based on weak references:
//  https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/weak-references
//  https://docs.microsoft.com/en-us/dotnet/api/system.weakreference?view=net-6.0
//redo are probably requires to make LookUpTable use event as key, and allow deletion of items in LookUpTable

public class AtomEvent : IAtomEvent
{
    private event Action _action;

    public void Subscribe(Action action)
    {
        _action += action;
    }

    public void UnSubscribe(Action action)
    {
        _action -= action;
    }
    public void Publish()
    {
        _action?.Invoke();
    }
    public Delegate[] GetInvocationList() => _action?.GetInvocationList() ?? Array.Empty<Delegate>();
}
