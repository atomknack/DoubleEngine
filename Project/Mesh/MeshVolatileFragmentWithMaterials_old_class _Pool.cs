/*
using Collections.Pooled;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public partial class MeshVolatileFragmentWithMaterials
    {
        private static readonly ConcurrentBag<object> s_pool = new ConcurrentBag<object>();
#if TESTING || DEBUG
        private static readonly ConcurrentDictionary<object, byte> s_set = new ConcurrentDictionary<object, byte>();

        public void ReturnToPool()
        {
            //Debug.Log($"Trying to return {this} to pool");
            if (s_set.ContainsKey((object)this))
                throw new InvalidOperationException("Pool already contains this mesh");
            s_pool.Add(this);
            s_set.TryAdd(this, 0);
        }
        public static MeshVolatileFragmentWithMaterials GetFromPoolOrCreateNew()
        {
            //Debug.Log($"Trying to get violatileFragment from pool");
            if (s_pool.TryTake(out var pooled))
            {
                if (s_set.Remove(pooled, out var _) == false)
                    throw new InvalidOperationException("Cannot remove retrieved from pool mesh from Debug set");
                return (MeshVolatileFragmentWithMaterials)pooled;
            }
            //Debug.Log($"Cannot get violatileFragment from pool, creating new");
            return new MeshVolatileFragmentWithMaterials();
        }
#else
        public void ReturnToPool()
        {
            s_pool.Add(this);
        }
        public static MeshVolatileFragmentWithMaterials GetFromPoolOrCreateNew()
        {
            if (s_pool.TryTake(out var pooled))
                return (MeshVolatileFragmentWithMaterials)pooled;
            return new MeshVolatileFragmentWithMaterials();
        }
#endif

        public static MeshVolatileFragmentWithMaterials Create(IMeshFragmentWithMaterials<Vec3D> iFragment)
        {
            var mesh = GetFromPoolOrCreateNew();
            mesh.UpdateFrom(iFragment.Vertices, iFragment.Faces, iFragment.FaceMaterials);
            return mesh;
        }
        public static MeshVolatileFragmentWithMaterials Create(ROSpanVec3D vertices, ROSpanIndexedTri faces, ReadOnlySpan<MaterialByte> faceMaterials)
        {
            var mesh = GetFromPoolOrCreateNew();
            mesh.UpdateFrom(vertices, faces, faceMaterials);
            return mesh;
        }
    }
}
*/