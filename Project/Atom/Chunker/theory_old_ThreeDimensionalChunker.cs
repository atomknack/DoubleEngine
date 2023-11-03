using System;
using System.Collections.Concurrent;
using System.Threading;
using CollectionLike;
using CollectionLike.Enumerables;

namespace DoubleEngine.Atom
{
    internal partial class theory_old_ThreeDimensionalChunker
    {
        internal partial class Worker {}


        private enum State : int
        {
            FreeAndIdle,
            AddedCells,
            FinishedProcessingChunks,
        }
        private bool _isResultsReady;
        private MeshVolatileFragmentWithMaterials[] _results;

        private State _state;
        private int _iterationsInFreeAndIdleStateCounter;
        private readonly int _chunkDimension;
        private readonly ThreeDimensionalGridOffsetter _offsetter;
        private readonly FixedSizeArrayBufferWithLeeway<Vec3I> _chunksToRebuild;
        private int _chunkIndexReadyToRebuild;
        private int _chunksFinishedCount;

        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly int _sizeZ;

        ConcurrentQueue<SpaceCell> _toAdd;

        public theory_old_ThreeDimensionalChunker(int chunkDimension, ThreeDimensionalGridOffsetter offsetter)
        {
            _chunkDimension = chunkDimension;
            _offsetter = offsetter;
            var dimensions = _offsetter.GetDimensions();
            _sizeX = dimensions.x;
            _sizeY = dimensions.y;
            _sizeZ = dimensions.z;
            _chunksToRebuild = new FixedSizeArrayBufferWithLeeway<Vec3I>(7);
            _results = new MeshVolatileFragmentWithMaterials[_chunksToRebuild.GetRealArrayFullLength()];
            _iterationsInFreeAndIdleStateCounter = -20;
            _chunkIndexReadyToRebuild = -1;
            _chunksFinishedCount = 0;
            _toAdd = new ConcurrentQueue<SpaceCell>();
            _state = State.FreeAndIdle;
        }

        public int HowManyResultsReady()
        {
            if (_isResultsReady == false)
                return 0;
            return _chunksFinishedCount;
        }
        [Obsolete("need testing")]
        public int RetrieveResults(Span<MeshVolatileFragmentWithMaterials> buffer)
        {
            if (_isResultsReady == false)
                throw new InvalidOperationException("results is not ready");
            if (buffer.Length < _chunksFinishedCount)
                throw new ArgumentException("buffer is too small for results to fit");

            _results.AsSpan(0, _chunksFinishedCount).AsReadOnly().CopyTo(buffer);
            for (int i = 0; i < _chunksFinishedCount; ++i)
            {
                _results[i] = MeshVolatileFragmentWithMaterials.Empty;
            }
            Thread.MemoryBarrier();
            _isResultsReady = false;
            return _chunksFinishedCount;
        }

        private bool TryGetChunkCoordFromWordCoord(Vec3I cellCoord, out Vec3I chunkStartCoord)
        {
            Vec3I coord = _offsetter.ProjectCoordinateInsideGrid(cellCoord);
            if (coord.x < 0 || coord.y < 0 || coord.z < 0 || coord.x >= _sizeX || coord.y >= _sizeY || coord.z >= _sizeZ)
            {
                chunkStartCoord = Vec3I.zero;
                return false;
            }
            chunkStartCoord = new Vec3I(coord.x % _chunkDimension, coord.y % _chunkDimension, coord.z % _chunkDimension);
            //chunkStartCoord = _offsetter.ProjectCoordinateFromGridToOutside(chunkStartCoord);
            return true;
        }

        private void AddChunkCoord(Vec3I chunk)
        {
            var span = _chunksToRebuild.AsReadOnlySpan();
            if (span.Contains(chunk))
                return;
            _chunksToRebuild.Add(chunk);
        }
        public bool HaveUnprocessedCells() => _toAdd.TryPeek(out _);
        public bool HaveUnprocessedChunks() =>
            _state == State.AddedCells && _chunkIndexReadyToRebuild < _chunksToRebuild.Count;
        public bool TryGetChunkToRebuild(out int chunkIndex, out Vec3I chunk)
        {
            if (_state == State.AddedCells || _chunkIndexReadyToRebuild>=_chunksToRebuild.Count)
            {
                chunkIndex = 0;
                chunk = Vec3I.zero;
                return false;
            }
            chunkIndex = Interlocked.Increment(ref _chunkIndexReadyToRebuild);
            if (chunkIndex < 0)
                throw new InvalidOperationException("this should never happen");
            if (chunkIndex>=_chunksToRebuild.Count)
            {
                chunk = Vec3I.zero;
                return false;
            }
            chunk = _chunksToRebuild[chunkIndex];
            return true;
        }
        public void ReportFinishedChunk(int chunkIndex, MeshVolatileFragmentWithMaterials result)
        {
            _results[chunkIndex] = result;
            //Interlocked.Add(ref _chunksFinishedSumm, chunkIndex);
            Interlocked.Increment(ref _chunksFinishedCount);
        }

        private void AddCells()
        {
            if(_state != State.FreeAndIdle)
                throw new InvalidOperationException("Can only add cells when there is no other work");
            if (_toAdd.TryPeek(out _) == false)
                return;
            var corners = Vec3I.Corners;
            var currentChunk = Vec3I.zero;
            bool haveChunk = false;
            while (_chunksToRebuild.CanAddMore() && _toAdd.TryDequeue(out SpaceCell cell))
            {
                var coords = cell.coords;
                foreach (var corner in corners)
                {
                    if (TryGetChunkCoordFromWordCoord(coords + corner, out Vec3I newChunk))
                    {
                        if (haveChunk)
                        {
                            if (currentChunk != newChunk)
                            {
                                currentChunk = newChunk;
                                AddChunkCoord(currentChunk);
                            }
                        }
                        else
                        {
                            haveChunk = true;
                            currentChunk = newChunk;
                            AddChunkCoord(currentChunk);
                        }
                    }
                }

            }
            SetState(State.AddedCells);
        }

        public void CheckFinishedProcessingChunks()
        {
            if (_state != State.AddedCells)
                return;
            if (_chunksFinishedCount == _chunksToRebuild.Count)
                SetState(State.FinishedProcessingChunks);
        }

        private void SetState(State state)
        {
            switch (state)
            {
                case State.FreeAndIdle:
                    //Thread.MemoryBarrier();
                    //Thread.MemoryBarrier();
                    break;
                case State.AddedCells:
                    Thread.MemoryBarrier();
                    _iterationsInFreeAndIdleStateCounter = -20;
                    _chunkIndexReadyToRebuild = -1;
                    _chunksFinishedCount = 0;
                    _state = state;
                    Thread.MemoryBarrier();
                    break;
                case State.FinishedProcessingChunks:
                    Thread.MemoryBarrier();
                    _isResultsReady = true;
                    Thread.MemoryBarrier();
                    _state = state;
                    break;
            }


        }

        //should be run only by ONE thread, SAME thread
        public void OverlordMethod() 
        {
            int idleCounter;
            switch (_state)
            {
                case State.FreeAndIdle:
                    AddCells();
                    idleCounter = Interlocked.Increment(ref _iterationsInFreeAndIdleStateCounter);
                    if (idleCounter > 100)
                        Thread.Sleep(0);
                    if (idleCounter > 200)
                        Thread.Sleep(25);
                    if (idleCounter > 1000)
                        Interlocked.Exchange(ref _iterationsInFreeAndIdleStateCounter, 300);
                    break;
                case State.AddedCells:
                    CheckFinishedProcessingChunks();
                    break;
                case State.FinishedProcessingChunks:
                    if (_isResultsReady == false) // 2022.08.08 = replaced by ==
                        SetState(State.FreeAndIdle);
                    break;
            }
        }

    }
}
