using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom.Multithreading;

public interface ICondition
{
    public bool IsTrue();
}
