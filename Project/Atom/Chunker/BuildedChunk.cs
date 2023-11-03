using System;

namespace DoubleEngine.Atom
{
    public readonly struct BuildedChunk: IDisposable
    {
        public readonly Vec3I globalOffset;
        public readonly MeshVolatileFragmentWithMaterials chunkMesh;

        public BuildedChunk(Vec3I offset, MeshVolatileFragmentWithMaterials chunkMesh)
        {
            this.globalOffset = offset;
            this.chunkMesh = chunkMesh;
        }

        public void Dispose()
        {
            chunkMesh.Dispose();
        }
    }
}
