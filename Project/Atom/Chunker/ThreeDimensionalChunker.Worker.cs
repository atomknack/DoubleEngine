using System;

namespace DoubleEngine.Atom;

public partial class ThreeDimensionalChunker
{
    public partial class Worker : AbstractWorker
    {
        ThreeDimensionalGridOffsetter _gridOffsetter;
        ThreeDimensionalBuilder _builder;
        public Worker(ThreeDimensionalChunker chunker) : base(chunker)
        {
            _gridOffsetter = ThreeDimensionalGridOffsetter.CreateClone(chunker._offsetter);
            _builder = ThreeDimensionalBuilder.Create(chunker._chunkDimension, chunker._chunkDimension, chunker._chunkDimension);
        }

        internal Vec3I GlobalOffset(Vec3I localOffset) => 
            _gridOffsetter.ProjectCoordinateFromGridToOutside(localOffset);

        public override BuildedChunk DoTheActualWork(Vec3I chunkLocalOffset)
        {
            var globalOffset = GlobalOffset(chunkLocalOffset);
            var tempOut = _gridOffsetter.GetOutOffset();
            _gridOffsetter.SetOutOffset(chunkLocalOffset);
            //_gridOffsetter.SetOutOffset(chunkLocalOffset); //not tested, maybe incorrect
            _builder.Build(_gridOffsetter);
            _gridOffsetter.SetOutOffset(tempOut);
            return new BuildedChunk(globalOffset, DecimatorColored.DecimateAndReturnViolatileFragment(_builder));
        }
    }
}
