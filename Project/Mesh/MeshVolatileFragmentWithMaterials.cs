
using CollectionLike.Pooled;
using Collections.Pooled;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly struct MeshVolatileFragmentWithMaterials : IDisposable, IMeshFragmentWithMaterials<Vec3D>
    {
        public static readonly MeshVolatileFragmentWithMaterials Empty = new MeshVolatileFragmentWithMaterials();

        readonly PooledArrayStruct<Vec3D> _vertices;
        readonly PooledArrayStruct<IndexedTri> _faces;
        readonly PooledArrayStruct<MaterialByte> _faceMaterials;

        public ReadOnlySpan<byte> FaceMaterials => _faceMaterials.AsReadOnlySpan();

        public ReadOnlySpan<Vec3D> Vertices => _vertices.AsReadOnlySpan();
        public ReadOnlySpan<int> Triangles => Faces.CastToInt_ReadOnlySpan();

        public ReadOnlySpan<IndexedTri> Faces => _faces.AsReadOnlySpan();

        public void Translate(Vec3D translation)
        {
            var vertices = _vertices.AsSpan();
            for(var i = 0; i < vertices.Length; ++i)
                vertices[i] = vertices[i] + translation;
        }

        public static MeshVolatileFragmentWithMaterials Create() => new MeshVolatileFragmentWithMaterials();
#if TESTING
        public static MeshVolatileFragmentWithMaterials TESTING_CreateFromWithoutCopy(PooledArrayStruct<Vec3D> vertices, PooledArrayStruct<IndexedTri> faces, PooledArrayStruct<MaterialByte> faceMaterials) =>
            CreateFromWithoutCopy(vertices, faces, faceMaterials);
#endif
        internal static MeshVolatileFragmentWithMaterials CreateFromWithoutCopy(PooledArrayStruct<Vec3D> vertices, PooledArrayStruct<IndexedTri> faces, PooledArrayStruct<MaterialByte> faceMaterials) =>
            new MeshVolatileFragmentWithMaterials(vertices, faces, faceMaterials);
        public static MeshVolatileFragmentWithMaterials CreateByCopyingFrom(IMeshFragmentWithMaterials<Vec3D> fragment)
        {
            PooledArrayStruct<Vec3D> vertices = Expendables.CreateArrayFromSpan(fragment.Vertices);
            PooledArrayStruct<IndexedTri> faces = Expendables.CreateArrayFromSpan(fragment.Faces);
            PooledArrayStruct<MaterialByte> faceMaterials = Expendables.CreateArrayFromSpan(fragment.FaceMaterials);
            return new MeshVolatileFragmentWithMaterials(vertices, faces, faceMaterials);
        }

       internal MeshVolatileFragmentWithMaterials(PooledArrayStruct<Vec3D> vertices, PooledArrayStruct<IndexedTri> faces, PooledArrayStruct<MaterialByte> faceMaterials)
        {
            if (faces.Count != faceMaterials.Count)
                throw new ArgumentException($"length of indices length {faces.Count} not equal faceMaterials length {faceMaterials.Count}");

            _vertices = vertices;
            _faces = faces;
            _faceMaterials = faceMaterials;
        }
        public MeshVolatileFragmentWithMaterials()
        {
            _vertices = PooledArrayStruct<Vec3D>.Empty;
            _faces = PooledArrayStruct<IndexedTri>.Empty;
            _faceMaterials = PooledArrayStruct<MaterialByte>.Empty;
        }

        public void Dispose()
        {
            _vertices.Dispose();
            _faces.Dispose();
            _faceMaterials.Dispose();
        }
    }
}