using DoubleEngine.Atom.Multithreading;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DoubleEngine.Atom
{
    internal partial class theory_old_ThreeDimensionalChunker
    {
        internal partial class Worker : IWorker
        {
            theory_old_ThreeDimensionalChunker _chunker;
            ThreeDimensionalGridOffsetter _gridOffsetter;
            ThreeDimensionalBuilder _builder;
            public bool TryDoTheWork()
            {
                int chunkIndex;
                Vec3I chunkCoord;
                if (_chunker.TryGetChunkToRebuild(out chunkIndex, out chunkCoord) == false)
                {
                   return false;
                }

                _gridOffsetter.SetOutOffset(chunkCoord); //not tested, maybe incorrect
                _builder.Build(_gridOffsetter);
                _chunker.ReportFinishedChunk( chunkIndex, DecimatorColored.DecimateAndReturnViolatileFragment(_builder));
                return true;
            }
            internal Worker(theory_old_ThreeDimensionalChunker chunker)
            {
                _chunker = chunker;
                _gridOffsetter = ThreeDimensionalGridOffsetter.CreateClone(chunker._offsetter);
                _builder = ThreeDimensionalBuilder.Create(_chunker._chunkDimension, _chunker._chunkDimension, _chunker._chunkDimension);
            }
        }
    }
}
