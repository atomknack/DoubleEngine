using CollectionLike;
using CollectionLike.Enumerables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DoubleEngine.Atom.Multithreading;

public abstract partial class AbstractOverlord<TInput, TWork, TResult> : IWorker
    where TInput : struct
    where TWork : struct, IEquatable<TWork> 
    where TResult : struct
{
    public abstract partial class AbstractWorker : IWorker {};

    private enum State : int
    {
        Pause,
        FreeAndIdle,
        ProcessedInput,
        FinishedProcessingWork,
    }
    private bool _shouldBePaused;
    private bool _haveResultsToRetrieve;
    private readonly TResult[] _results;
    private readonly FixedSizeArrayBufferWithLeeway<TInput> _inputForResults;

    private State _state;
    private readonly FixedSizeArrayBufferWithLeeway<TWork> _workToBeProcessed;
    private int _chunkIndexReadyToRebuild;
    private int _workFinishedCount;

    private readonly ConcurrentQueue<TInput> _toAdd;

    //public bool HaveUnprocessedInput() => _toAdd.TryPeek(out _);
    //public bool HaveUnprocessedWork() =>
    //    _state == State.ProcessedInput && _chunkIndexReadyToRebuild < _workToBeProcessed.Count;
    public void AddInput(TInput input)
    {
        _toAdd.Enqueue(input);
    }
    public void AddWorkItemToBeProcessed(TWork work, bool addOnlyIfUnique = false)
    {
        //if (_workToBeProcessed.CanAddMore() == false)
        //    throw new ArgumentException("to be processed work cannot accept more work");
        if (addOnlyIfUnique)
        {
            var roWorkSpan = _workToBeProcessed.AsReadOnlySpan();
            for(int i = 0; i < roWorkSpan.Length; ++i)
            {
                if (work.Equals(roWorkSpan[i]))
                    return;
            }
        }
        _workToBeProcessed.Add(work);
    }

    public abstract AbstractWorker CreateSubordinateWorker();

    public bool TryGetInputForWork(out TInput input)
    {
        if (_state == State.FreeAndIdle && 
            _workToBeProcessed.CanAddMore() && 
            _inputForResults.CanAddMore() && 
            _toAdd.TryDequeue(out input))
        {
            _inputForResults.Add(input);
            return true;
        }

        input = default;
        return false;
    }
    protected abstract void AddWork();
    public void PauseProcessingInput()
    {
        _shouldBePaused = true;
    }
    public void UnPauseProcessingInput()
    {
        _shouldBePaused = false;
    }
    public bool IsPaused() =>
        _shouldBePaused == true && _state == State.Pause;

    public bool ResultsReady()
    {
        if (_haveResultsToRetrieve == false)
            return false;
        if (_state != State.FinishedProcessingWork)
            return false;
        return _workFinishedCount != 0 || _inputForResults.Count != 0; //_workFinishedCount;
    }
    public bool ResultsReady(out int resultsCount, out int processedInputCount)
    {
        if (ResultsReady() == false)
        {
            resultsCount = 0;
            processedInputCount = 0;
            return false;
        }
        resultsCount = _workFinishedCount;
        processedInputCount = _inputForResults.Count;
        return true;
    }
    
    public void RetrieveResults(Span<TResult> resultsBuffer, Span<TInput> bufferForProcessedInput)
    {

        var count = _workFinishedCount;
        if (_haveResultsToRetrieve == false)
            throw new InvalidOperationException("results is not ready");
        if (resultsBuffer.Length < count)
            throw new ArgumentException("resultsBuffer is too small for results to fit");
        if (bufferForProcessedInput.Length < _inputForResults.Count)
            throw new ArgumentException("bufferForProcessedInput is too small for processed input to fit");

        _results.AsSpan(0, count).AsReadOnly().CopyTo(resultsBuffer);
        _inputForResults.AsReadOnlySpan().CopyTo(bufferForProcessedInput);
        Thread.MemoryBarrier();
        _haveResultsToRetrieve = false;
    }
    public bool TryDoTheWork()
    {
        return OverlordMethod_TryChangeState();
    }
    protected bool TryAddWorkFromInput()
    {
        if (_state != State.FreeAndIdle || _toAdd.TryPeek(out _) == false)
            return false;
        //if (_workToBeProcessed.CanAddMore() == false || _inputForResults.CanAddMore() == false)
        //    return false;
        AddWork();
        return true;
    }

    internal virtual void CleanupResults()
    {
        if (_haveResultsToRetrieve != false)
            throw new InvalidOperationException("Cannot cleanup not retrieved results array");
        for (int i = 0; i < _workFinishedCount; ++i)
        {
            _results[i] = default(TResult);
        }
        _inputForResults.Clear();
    }

    protected bool WorkerCallable_MaybeHaveWork() => _state == State.ProcessedInput || _chunkIndexReadyToRebuild < _workToBeProcessed.Count;
    protected bool WorkerCallable_TryGetWork(out int chunkIndex, out TWork chunk)
    {
        //Debug.Log($"overlord Try get work asked {_state} {_chunkIndexReadyToRebuild} {_workToBeProcessed.Count}");
        if (_state != State.ProcessedInput || _chunkIndexReadyToRebuild>=_workToBeProcessed.Count)
        {
            chunkIndex = 0;
            chunk = default(TWork);
            return false;
        }
        chunkIndex = Interlocked.Increment(ref _chunkIndexReadyToRebuild);
        //Debug.Log($"overlord Try get work asked recieved index {chunkIndex}");
        if (chunkIndex < 0)
            throw new InvalidOperationException("this should never happen");
        if (chunkIndex>=_workToBeProcessed.Count)
        {
            chunk = default(TWork);
            return false;
        }
        chunk = _workToBeProcessed[chunkIndex];
        return true;
    }
    protected void WorkerCallable_ReportFinishedResult(int chunkIndex, TResult result)
    {
        //Debug.Log($"Worker report finished {chunkIndex} {result}");
        _results[chunkIndex] = result;
        //Interlocked.Add(ref _chunksFinishedSumm, chunkIndex);
        var count = Interlocked.Increment(ref _workFinishedCount);
        //Debug.Log(count);
    }

    protected bool WorkersFinishedAllWork()
    {
        //Debug.Log("WorkersFinishedAllWork Called");
        if (_state != State.ProcessedInput)
            return false;
        if (_workFinishedCount == _workToBeProcessed.Count)
            return true;
        //Debug.Log($"WorkersFinishedAllWork return false {_workFinishedCount} {_workToBeProcessed.Count}");
        return false;
    }

    private void SetState(State state)
    {
        //Debug.Log($"SetState called, prev {_state} next {state}");
        switch (state)
        {
            case State.Pause:
                Thread.MemoryBarrier();
                _state = state;
                break;
            case State.FreeAndIdle:
                if (_shouldBePaused)
                {
                    SetState(State.Pause);
                    return;
                }
                _workToBeProcessed.Clear();
                Thread.MemoryBarrier();
                _state = state;
                break;
            case State.ProcessedInput:
                _chunkIndexReadyToRebuild = -1;
                _workFinishedCount = 0;
                Thread.MemoryBarrier();
                _state = state;
                Thread.MemoryBarrier();
                break;
            case State.FinishedProcessingWork:
                _haveResultsToRetrieve = true;
                Thread.MemoryBarrier();
                _state = state;
                break;
        }
    }

    //should be run only by ONE thread, SAME thread
    internal bool OverlordMethod_TryChangeState() 
    {
        //Debug.Log($"OverlordMethod_TryChangeState called, current state {_state}");
        //int idleCounter;
        switch (_state)
        {
            case State.Pause:
                if(_shouldBePaused == false)
                {
                    SetState(State.FreeAndIdle);
                    return true;
                }
                break;
            case State.FreeAndIdle:
                if(_shouldBePaused == true)
                {
                    SetState(State.Pause);
                    return true;
                }
                if (TryAddWorkFromInput())
                {
                    //idleCounter = 0;
                    SetState(State.ProcessedInput);
                    return true;
                }
                break;
            case State.ProcessedInput:
                if (WorkersFinishedAllWork())
                {
                    SetState(State.FinishedProcessingWork);
                    return true;
                }
                break;
            case State.FinishedProcessingWork:
                //Debug.Log($"waiting to be taken results {HowManyResultsReady()} {_haveResultsToRetrieve}");
                if (_haveResultsToRetrieve == false)
                {
                    CleanupResults();
                    SetState(State.Pause);
                    //Debug.Log($"changed state to Pause");
                    return true;
                }
                break;
        }
        return false;
    }
    protected AbstractOverlord(int desiredMinimumWorkBufferCount, int desiredMinimumProcessedInputBufferCount)
    {
        _inputForResults = new FixedSizeArrayBufferWithLeeway<TInput>(desiredMinimumProcessedInputBufferCount);
        _workToBeProcessed = new FixedSizeArrayBufferWithLeeway<TWork>(desiredMinimumWorkBufferCount);
        _results = new TResult[_workToBeProcessed.GetRealArrayFullLength()];
        _chunkIndexReadyToRebuild = -1;
        _workFinishedCount = 0;
        _toAdd = new ConcurrentQueue<TInput>();
        _shouldBePaused = true;
        _state = State.Pause;
    }
}
