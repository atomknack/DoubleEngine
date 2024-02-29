//----------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a MeshBuilder.tt
//     Changes will be lost if the code is regenerated.
// </auto-generated>
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Collections.Pooled;
using System.Linq;
using CollectionLike.Pooled;
using VectorCore;

namespace DoubleEngine
{
    public class MeshBuilderVec3D: IDisposable, IMeshBuilder<MeshFragmentVec3D, Vec3D, QuaternionD>
    {
        private PooledList<Vec3D> _vertexes;
        private PooledList<int> _triangles;

        private HashSet<int> _onlyUseFaces;
        private HashSet<int> _facesToRemove;

        Vec3D? scale = null; 
        QuaternionD? rotation = null;
        Vec3D? translation = null;

        private int MeshFacesCount => _triangles.Count / 3;

        public MeshBuilderVec3D()
        {
            _vertexes = new PooledList<Vec3D>();
            _triangles = new PooledList<int>();
        }
        public MeshBuilderVec3D(MeshFragmentVec3D initial)
        {
            _vertexes = new PooledList<Vec3D>(initial.vertices);
            _triangles = new PooledList<int>(initial.triangles);
        }
        public MeshBuilderVec3D(Vec3D[] vertexes, int[] triangles)
        {
            _vertexes = new PooledList<Vec3D>(vertexes);
            _triangles = new PooledList<int>(triangles);
            MeshFragmentVec3D.CheckTriangles(triangles);
            MeshFragmentVec3D.CheckVerticeIndexes(triangles, vertexes.Length);
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment) //TODO TEST
        {
            int newStartAt = _vertexes.Count;
            _triangles.AddRange(fragment.triangles.Select(x=>x+newStartAt));
            _vertexes.AddRange(fragment.vertices);
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment, Vec3D fragmentTranslation) //TODO TEST
        {
            int newStartAt = _vertexes.Count;
            _triangles.AddRange(fragment.triangles.Select(x=>x+newStartAt));
            for(int i=0;i<fragment.vertices.Length;++i)
                _vertexes.Add(fragment.vertices[i]+fragmentTranslation);
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment, QuaternionD rotation, Vec3D translation) //TODO TEST
        {
            int newStartAt = _vertexes.Count;
            _triangles.AddRange(fragment.triangles.Select(x=>x+newStartAt));
            MatrixD4x4 m = MatrixD4x4.FromOperationRotateThenTranslate(rotation, translation);
            for(int i=0;i<fragment.vertices.Length;++i)
                _vertexes.Add(m.MultiplyPoint3x4(fragment.vertices[i]));//rotation.Rotate(fragment.vertices[i])+translation);
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment,Vec3D scale, QuaternionD rotation, Vec3D translation) //TODO TEST
        {
            int newStartAt = _vertexes.Count;
            if (scale.x * scale.y * scale.z < 0)
                _triangles.AddRange(fragment.triangles.Select(x => x + newStartAt).Reverse());
            else
                _triangles.AddRange(fragment.triangles.Select(x => x + newStartAt));
            var m = MatrixD4x4.FromOperationScaleThenRotateThenTranslate(scale, rotation, translation);
            for(int i=0;i<fragment.vertices.Length;++i)
                _vertexes.Add(m.MultiplyPoint3x4(fragment.vertices[i]));//rotation.Rotate(fragment.vertices[i].Scaled(scale))+translation);
        }
        public void Dispose()
        {
            Clear();
            _vertexes.Dispose();
            _triangles.Dispose();
        }
        public void Clear()
        {
            _vertexes.Clear();
            _triangles.Clear();
            _onlyUseFaces = null;
            _facesToRemove = null;
            scale = null;
            rotation = null;
            translation = null;
        }
        public void AddTranslation(Vec3D t) => translation = translation.Translated(t);
        public void AddRotation(QuaternionD r) => rotation = rotation.Rotated(r);
        public void AddScale(Vec3D v) => scale = scale.Scaled(v);
        public MeshFragmentVec3D BuildFragment()
        {
            Build_5_RemoveUnusedFacesAndVertices();
            Build_11_ApplyScaleRotationTranslation();
            return MeshFragmentVec3D.CreateMeshFragment(_vertexes.AsReadOnlySpan(), _triangles.AsReadOnlySpan());
        }
        public void RemoveFaces(int[] faces)
        {
            Init_facesToRemove();
            foreach (var face in faces)
                _facesToRemove.Add(face);
        }
        public void OnlyUseFaces(int[] faces)
        {
            _onlyUseFaces = new HashSet<int>(faces);
        }

        private void Build_5_RemoveUnusedFacesAndVertices()
        {
            IEnumerable<int> useFaces = null;

            if(_facesToRemove!=null && _onlyUseFaces!=null)
                useFaces = _onlyUseFaces.Except(_facesToRemove);
            else if(_facesToRemove!=null)
            {
                useFaces = Enumerable.Range(0, MeshFacesCount).Except(_facesToRemove);
            }
            else if (_onlyUseFaces != null)
                useFaces = _onlyUseFaces;

            _facesToRemove = null;
            _onlyUseFaces = null;
            RemoveUnusedFacesAndVertices(useFaces);
        }
        [Obsolete("not tested")]
        private void RemoveUnusedFacesAndVertices(IEnumerable<int> newMeshFaceIdexes = null)
        {
           (var newVertexes, var newTriangles) = MeshUtil.RemoveUnusedVerticesAndFacesAndReturnNewArraysTriangles(
           _vertexes.AsReadOnlySpan(),_triangles.AsReadOnlySpan().CastToIndexedTri_ReadOnlySpan(),newMeshFaceIdexes);
            _vertexes.Clear();
            _vertexes.AddRange(newVertexes);
            _triangles.Clear();
            _triangles.AddRange(newTriangles);
        
        }
        private void Build_11_ApplyScaleRotationTranslation()
        {
            if (scale == null && rotation == null && translation == null)
                return;

            MatrixD4x4 m = MatrixD4x4.identity;
            //order of matrix multiplication important
            if (translation != null)
                m = m * MatrixD4x4.Translate(translation.Value);

            if (rotation != null)
                m = m * MatrixD4x4.Rotate(rotation.Value);

            if (scale != null)
            {
                Vec3D _scale = scale.Value;
                m = m * MatrixD4x4.Scale(_scale);
                if (_scale.x * _scale.y * _scale.z < 0)
                    ReverseTriangles();
            }
            //if (m.m00 * m.m11 * m.m22 < 0) //do not use matrix to find inversion because it somehow gives falsepositives depending on rotation Quaternion
            //    ReverseTriangles();

            for (var i = 0; i<_vertexes.Count; i++)
                _vertexes[i] = m.MultiplyPoint3x4(_vertexes[i]);

        }
        private void ReverseTriangles()
        {
            for (var i = 0; i < _triangles.Count; i += 3)
                (_triangles[i], _triangles[i + 2]) = (_triangles[i + 2], _triangles[i]);
        }
        private void Init_facesToRemove()
        {
            if (_facesToRemove == null)
                _facesToRemove = new HashSet<int>();
        }
    }
}
