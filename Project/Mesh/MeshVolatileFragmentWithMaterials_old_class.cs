/*
using Collections.Pooled;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    [Obsolete("URGENT! need testing")]
    public readonly struct class MeshVolatileFragmentWithMaterials : IDisposable, IMeshFragmentWithMaterials<Vec3D>
    {

        PooledArrayStruct<Vec3D> _vertices;
        PooledArrayStruct<IndexedTri> _faces;
        PooledArrayStruct<MaterialByte> _faceMaterials;

        public ReadOnlySpan<byte> FaceMaterials => _faceMaterials.AsReadOnlySpan();

        public ReadOnlySpan<Vec3D> Vertices => _vertices.AsReadOnlySpan();
        [Obsolete("not tested, cast need testing")]
        public ReadOnlySpan<int> Triangles => Faces.CastToInt_ReadOnlySpan();

        public ReadOnlySpan<IndexedTri> Faces => _faces.AsReadOnlySpan();

        public void Translate(Vec3D translation)
        {
            var vertices = _vertices.AsSpan();
            for(var i = 0; i < vertices.Length; ++i)
                vertices[i] = vertices[i] + translation;
        }
        public void UpdateFrom(IMeshFragmentWithMaterials<Vec3D> iFragment)
        {
            UpdateFrom(iFragment.Vertices,iFragment.Faces,iFragment.FaceMaterials);
        }
        [Obsolete("need testing")]
        public void UpdateFrom(ROSpanVec3D vertices, ROSpanIndexedTri faces, ReadOnlySpan<MaterialByte> faceMaterials)
        {
            if (faces.Length != faceMaterials.Length)
                throw new ArgumentException($"length of indices length {faces.Length} not equal faceMaterials length {faceMaterials.Length}");
            PooledArrayStruct<Vec3D>.ReSize(ref _vertices, vertices.Length);
            vertices.CopyTo(_vertices.AsSpan());
            PooledArrayStruct<IndexedTri>.ReSize(ref _faces, faces.Length);
            faces.CopyTo(_faces.AsSpan());
            PooledArrayStruct<MaterialByte>.ReSize(ref _faceMaterials, faceMaterials.Length);
            faceMaterials.CopyTo(_faceMaterials.AsSpan());
            //throw new NotImplementedException();
        }
        internal void UpdateByReplacingInternalsFrom(PooledArrayStruct<Vec3D> vertices, PooledArrayStruct<IndexedTri> faces, PooledArrayStruct<MaterialByte> faceMaterials)
        {
            if (faces.Count != faceMaterials.Count)
                throw new ArgumentException($"length of indices length {faces.Count} not equal faceMaterials length {faceMaterials.Count}");

            _vertices.Dispose();
            _vertices = vertices;
            _faces.Dispose();
            _faces = faces;
            _faceMaterials.Dispose();
            _faceMaterials = faceMaterials;
        }

        private MeshVolatileFragmentWithMaterials()
        {
            _vertices = PooledArrayStruct<Vec3D>.Empty; //Expendables.CreateArray<Vec3D>(800);
            _faces = PooledArrayStruct<IndexedTri>.Empty; //Expendables.CreateArray<IndexedTri>(500);
            _faceMaterials = PooledArrayStruct<MaterialByte>.Empty; //Expendables.CreateArray<MaterialByte>(500);
        }
        public void Dispose()
        {
            _vertices.Dispose();
            _vertices = PooledArrayStruct<Vec3D>.Empty;
            _faces.Dispose();
            _faces = PooledArrayStruct<IndexedTri>.Empty;
            _faceMaterials.Dispose();
            _faceMaterials= PooledArrayStruct<MaterialByte>.Empty;
            this.ReturnToPool();
        }
    }
}
*/