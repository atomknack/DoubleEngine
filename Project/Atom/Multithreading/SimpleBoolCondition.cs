using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom.Multithreading;

public class SimpleBoolCondition : ICondition
{
    private bool _value;
    public bool IsTrue() => _value;
    public void SetValue(bool value) => _value = value;
    public SimpleBoolCondition(bool value)
    {
        _value = value;
    }


}
