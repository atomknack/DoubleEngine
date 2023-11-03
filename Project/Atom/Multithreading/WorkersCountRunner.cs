using DoubleEngine.Guard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DoubleEngine.Atom.Multithreading;

public class WorkersCountRunner
{
    public static void RunWorkersNTimes(IWorker[] workers, int count)
    {
        //if (count<0)
        //    Throw.ArgumentOutOfRangeException(nameof(count), "should be 0 or more");
        //if (workers == null)
        //    throw new ArgumentNullException("workers is null");

        Throw.IfArgumentInNotRange(count < 0, nameof(count), "should be 0 or more");
        Throw.IfNullOrEmpty(workers, nameof(workers));

        while (count>=0)
        {
            for (int i = 0; i < workers.Length; i++)
                workers[i].TryDoTheWork();
            --count;
        }
    }
}