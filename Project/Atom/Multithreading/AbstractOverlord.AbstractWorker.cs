using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DoubleEngine.Atom.Multithreading;

public partial class AbstractOverlord<TInput, TWork, TResult>
{
    public abstract partial class AbstractWorker : IWorker
    {
        protected readonly AbstractOverlord<TInput, TWork, TResult> _overlord;

        public abstract TResult DoTheActualWork(TWork work);

        public bool TryDoTheWork()
        {
            //Debug.Log($"Try do the work: worker {this.GetHashCode()} overlord {_overlord.GetHashCode()}");
            if (_overlord.WorkerCallable_MaybeHaveWork() == false)
                return false;
            if (_overlord.WorkerCallable_TryGetWork(out int index, out TWork work) == false)
               return false;
            //Debug.Log($"Worker now can do work {index} {work}");
            //var result = DoTheActualWork(work);
            //Debug.Log($"Worker work {index} {work} result is {result}");
            _overlord.WorkerCallable_ReportFinishedResult( index, DoTheActualWork(work));
            return true;
        }
        protected AbstractWorker(AbstractOverlord<TInput, TWork, TResult> overlord)
        {
            _overlord = overlord;
        }
    }
}
