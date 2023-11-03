using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CollectionLike.Pooled;
using Collections.Pooled;

namespace DoubleEngine
{

    public static partial class MeshUtil
    {
        public static PooledArrayStruct<TVertice> RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_Span<TVertice>(
            ReadOnlySpan<IndexedTri> faces, Span<IndexedTri> newFaces, ReadOnlySpan<TVertice> vertices) where TVertice : struct
            => UnusedVerticeRemover.RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_Span(
                faces, newFaces, vertices);
        public static PooledArrayStruct<TVertice> RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_IList<TVertice>(
            IReadOnlyList<IndexedTri> faces, IList<IndexedTri> newFaces, IReadOnlyList<TVertice> vertices) where TVertice : struct
            => UnusedVerticeRemover.RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_IList(
                faces, newFaces, vertices);

        public static PooledList<IndexedTri> RemoveUnusedFaces(ROSpanIndexedTri faces, IEnumerable<int> usedFaceIndexes)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            var result = Expendables.CreateList<IndexedTri>(faces.Length);
#pragma warning restore CA2000 // Dispose objects before losing scope

            if (usedFaceIndexes == null)
            {
                foreach (var face in faces)
                    result.Add(face);
                return result;
            }
#if TESTING || DEBUG
            using PooledSet<int> usedFacesSet = Expendables.CreateSet<int>(faces.Length*2);
#endif
            foreach (int used in usedFaceIndexes)
            {
#if TESTING || DEBUG
                if (usedFacesSet.Contains(used))
                    throw new ArgumentException(nameof(usedFaceIndexes));
                usedFacesSet.Add(used);
#endif
                result.Add(faces[used]);
            }
            return result;
        }
    }

}