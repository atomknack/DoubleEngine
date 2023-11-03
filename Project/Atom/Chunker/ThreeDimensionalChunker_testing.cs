#if TESTING
using DoubleEngine.Atom.Multithreading;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom;

public partial class ThreeDimensionalChunker : AbstractOverlord<SpaceCell, Vec3I, BuildedChunk>
{
    public partial class Worker
    {
        public Vec3I TESTING_GlobalOffset(Vec3I localOffset) => GlobalOffset(localOffset);
    }
    public bool TESTING_TryGetChunkCoordFromWordCoord(Vec3I cellCoord, out Vec3I chunkLocalStartCoord) =>
        TryGetChunkCoordFromWordCoord(cellCoord, out chunkLocalStartCoord);
}
#endif