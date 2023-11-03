using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public static class Remappers_Extensions
    {
        public static void AddMany(this RemapperRegistryIndex remapper, ReadOnlySpan<EdgeRegistered> itemsToAdd)
        { 
            for (int i = 0; i < itemsToAdd.Length; i++)
            {
                remapper.Add(itemsToAdd[i].start);
                remapper.Add(itemsToAdd[i].end);
            }
        }

        public static void RemappedToBuffer(this RemapperRegistryIndex remapper, PoolListEdgeIndexed buffer, ReadOnlySpan<EdgeRegistered> edgesToRemap)
        {
            for (int i = 0; i < edgesToRemap.Length; i++)
            {
                var edge = edgesToRemap[i];
                buffer.Add(new EdgeIndexed(remapper.GetRemappedForExisting(edge.start), remapper.GetRemappedForExisting(edge.end)));
            }
        }
        public static void RemappedToBuffer(this RemapperRegistryIndex remapper, PoolListEdgeIndexed buffer, IEnumerable<EdgeRegistered> edgesToRemap)
        {
            foreach (var edge in edgesToRemap)
                buffer.Add(new EdgeIndexed(remapper.GetRemappedForExisting(edge.start), remapper.GetRemappedForExisting(edge.end)));
        }

        /*
        public static Vec3D[] BuildVec3DRemappedFromRegister(this RemapperRegistryIndex remapper, EverGrowingVec3DVector3 register) //TODO: make copy for public static Vector3[] BuildVector3RemappedFromRegister(Remapper remapper)
        {
            int length = remapper.GetCount();
            Vec3D[] vertices = new Vec3D[length];
            foreach (RegistryIndex key in remapper.GetItemsToRemap())
            {
                vertices[remapper.GetRemappedForExisting(key)] = register.GetItem(key);
            }
            return vertices;
        }*/

        public static T[] BuildRemappedFromRegister<T>(this RemapperRegistryIndex remapper, IRegistry<T> register) //TODO: make copy for public static Vector3[] BuildVector3RemappedFromRegister(Remapper remapper)
        {
            int length = remapper.GetCount();
            T[] vertices = new T[length];
            foreach (RegistryIndex key in remapper.GetItemsToRemap())
            {
                vertices[remapper.GetRemappedForExisting(key)] = register.GetItem(key);
            }
            return vertices;
        }
    }
}
