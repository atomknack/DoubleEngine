using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DoubleEngine.Atom.Multithreading;

public class WorkersConditionRunner
{
    IWorker[] _workers;
    ICondition _condition;
    long _cyclesWithOutWork = 0;

    public void SimpleRunWorkersWhileConditionTrue()
    {
        bool someoneDidSomeWork = true;
        while (someoneDidSomeWork || _condition.IsTrue())
        {
            someoneDidSomeWork = false;
            for (int i = 0; i < _workers.Length; i++)
                someoneDidSomeWork = _workers[i].TryDoTheWork() | someoneDidSomeWork;
            //Debug.Log($"tick {Thread.CurrentThread.GetHashCode()}, {_workers.Length}, {someoneDidSomeWork}, {_condition.IsTrue()}");
        }
    }
    public void RunWorkersWhileConditionTrueWithRestIfNoWork()
    {
        bool someoneDidSomeWork = true;
        while (someoneDidSomeWork || _condition.IsTrue())
        {
            someoneDidSomeWork = false;
            for (int i = 0; i < _workers.Length; i++)
                someoneDidSomeWork = _workers[i].TryDoTheWork() | someoneDidSomeWork;
            //Debug.Log($"tick {Thread.CurrentThread.GetHashCode()}, {_workers.Length}, {someoneDidSomeWork}, {_condition.IsTrue()}");
            if (someoneDidSomeWork)
                _cyclesWithOutWork = 0;
            else
                ++_cyclesWithOutWork;
            WaitIfNoWork();
        }
    }
    private void WaitIfNoWork()
    {
        if (_cyclesWithOutWork > 500)
            Thread.Sleep(10);
        else if (_cyclesWithOutWork > 20)
            Thread.Sleep(5);
        else if (_cyclesWithOutWork > 10)
            Thread.Sleep(1);
        else if (_cyclesWithOutWork > 0)
            Thread.Sleep(0);
    }

    public WorkersConditionRunner(IWorker[] workers, ICondition condition)
    {
        if (workers == null || workers.Length == 0)
            throw new ArgumentNullException();
        _workers = workers;
        _condition = condition;
    }
}

/*idleCounter = Interlocked.Increment(ref _iterationsInFreeAndIdleStateCounter);
if (idleCounter > 100)
Thread.Sleep(0);
if (idleCounter > 200)
Thread.Sleep(25);
if (idleCounter > 1000)
Interlocked.Exchange(ref _iterationsInFreeAndIdleStateCounter, 300);*/