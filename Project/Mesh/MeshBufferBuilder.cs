
using System;
using System.Collections.Generic;
using Collections.Pooled;
using CollectionLike.Pooled;

namespace DoubleEngine
{
    public class MeshBufferBuilder: IDisposable, IMeshFragment<Vec3D>, IMeshBuilder<MeshFragmentVec3D, Vec3D, QuaternionD>
    {
        private PoolListVec3D _vertices;
        private PoolListIndexedTri _faces;
        private IReadOnlyList<int> _externalToCheckDuplicateVerticesIndexes;
        private PoolListVec3D _toCheckDuplicateVerticesVec3D;
        private PoolListInt _noveltyVertices;

        public ReadOnlySpan<Vec3D> Vertices => _vertices.AsReadOnlySpan();

        public ReadOnlySpan<int> Triangles => Faces.CastToInt_ReadOnlySpan();// throw new NotImplementedException();

        public ReadOnlySpan<IndexedTri> Faces => _faces.AsReadOnlySpan();

        public MeshBufferBuilder() : this(1000, 1000) { }
        public MeshBufferBuilder(int initialVertexBufferSize, int initialTrianglesBufferSize)
        {
            this._vertices = Expendables.CreateList<Vec3D>(initialVertexBufferSize);
            this._faces = Expendables.CreateList<IndexedTri>(initialTrianglesBufferSize);
            this._toCheckDuplicateVerticesVec3D = Expendables.CreateList<Vec3D>(300);
            this._noveltyVertices = Expendables.CreateList<int>(1000);
        }

        //public ROSpanVec3D GetVertices() => _vertices.AsReadOnlySpan();
        //public ROSpanIndexedTri GetFaces() => _faces.AsReadOnlySpan();
        public void AddFaces(ROSpanIndexedTri span)
        {
            _faces.AppendSpan(span);
        }
        public void ResetTriangles() => _faces.Clear();

        public ROSpanInt GetNoveltyVertices() => _noveltyVertices.AsReadOnlySpan();
        public void ResetNoveltyVertices() => _noveltyVertices.Clear();

        //internal void SetVertexBuffer(IList<Vec3D> verticesBuffer) { vertices = verticesBuffer; }
        //internal void SetTrianglesBuffer(IList<IndexedTri> trianglesBuffer) { triangles = trianglesBuffer; }
        internal void SetToCheckDuplicateVertices(IReadOnlyList<int> toCheckDuplicateVertices) 
        {
            _toCheckDuplicateVerticesVec3D.Clear();
            _externalToCheckDuplicateVerticesIndexes = toCheckDuplicateVertices;
            if (_externalToCheckDuplicateVerticesIndexes!=null && _externalToCheckDuplicateVerticesIndexes.Count > 0)
            {
                var writableSpan = _toCheckDuplicateVerticesVec3D.AddSpan(_externalToCheckDuplicateVerticesIndexes.Count);
                _vertices.AssembleIndicesToBuffer(_externalToCheckDuplicateVerticesIndexes, writableSpan);
            }
        }
        //internal void SetNoveltyVerticesBuffer(IList<int> noveltyBuffer) { noveltyVertices = noveltyBuffer; }

        public void AddMeshFragment(MeshFragmentVec3D fragment)
        {
            AddMeshFragment(fragment, Vec3D.one, QuaternionD.identity, Vec3D.zero);
        }

        public void AddMeshFragment(MeshFragmentVec3D fragment, Vec3D fragmentTranslation)
        {
            AddMeshFragment(fragment, Vec3D.one, QuaternionD.identity, fragmentTranslation);
        }

        public void AddMeshFragment(MeshFragmentVec3D fragment, QuaternionD rotation, Vec3D fragmentTranslation)
        {
            AddMeshFragment(fragment, Vec3D.one, rotation, fragmentTranslation);
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment, Vec3D scale, QuaternionD rotation, Vec3D translation)
        {
            ROSpanVec3D fragmentVertices = fragment.Vertices;
            ReadOnlySpan<IndexedTri> fragmentFaces = fragment.Faces;

            ROSpanVec3D toCheckSpan = _toCheckDuplicateVerticesVec3D.AsReadOnlySpan();//_externalToCheckDuplicateVerticesIndexes == null ? 
                //_toCheckDuplicateVerticesVec3D.GetReadOnlySpan() :
                //ROSpanVec3D.Empty;

            ROSpanInt novelty = GetNoveltyVertices();
            ROSpanVec3D builderAllVertices = Vertices;

            var m = MatrixD4x4.FromOperationScaleThenRotateThenTranslate(scale, rotation, translation);
            //using PooledArrayStruct<int> localVertices = new PooledArrayStruct<int>(fragmentVertices.Length);
            int[] localVertices  = new int[fragmentVertices.Length];
            for (int i = 0; i < fragmentVertices.Length; ++i)
                {
                Vec3D vertex = m.MultiplyPoint3x4(fragmentVertices[i]);
                localVertices[i] = FindExistiogOrAddVertex(builderAllVertices, vertex, toCheckSpan, novelty);  
                }

            if (scale.x * scale.y * scale.z < 0)
                for (int i = 0; i < fragmentFaces.Length; ++i)
                {
                    IndexedTri old = fragmentFaces[i];
                    _faces.Add(new IndexedTri(localVertices[old.v2], localVertices[old.v1], localVertices[old.v0]));
                }
            else
                for (int i = 0; i < fragmentFaces.Length; ++i)
                {
                    IndexedTri old = fragmentFaces[i];
                    _faces.Add(new IndexedTri(localVertices[old.v0], localVertices[old.v1], localVertices[old.v2]));
                }
            //throw new NotImplementedException();
        }

        public int FindExistiogOrAddVertex(ROSpanVec3D vertices, Vec3D vertex, ROSpanVec3D toCheckSpan, ROSpanInt novelty)
        {
            if (toCheckSpan.In_CompareByEach(vertex, out int indexOftoCheckDuplicateVertices))
                return _externalToCheckDuplicateVerticesIndexes[indexOftoCheckDuplicateVertices];
            if (novelty.InIndexes_CompareByEach(vertices, vertex, out int indexInNovelty))
                return novelty[indexInNovelty];
            return AddVertex(vertex);
        }

        private int AddVertex(Vec3D vertex)
        {
            int newVertexIndex = _vertices.Count;
            _vertices.Add(vertex);
            _noveltyVertices.Add(newVertexIndex);
            return newVertexIndex;//_vertices.IndexOfLastElement();
        }

        public void Dispose()
        {
            Clear();
            _vertices.Dispose();
            _faces.Dispose();
            //_externalToCheckDuplicateVerticesIndexes = null;
            _toCheckDuplicateVerticesVec3D.Dispose();
            _noveltyVertices.Dispose();
        }

        public MeshFragmentVec3D BuildFragment()
        {
            return MeshFragmentVec3D.CreateMeshFragment(_vertices.AsReadOnlySpan(), _faces.AsReadOnlySpan());
        }

        public void Clear()
        {
            _vertices.Clear();
            _faces.Clear();
            _externalToCheckDuplicateVerticesIndexes = null;
            _toCheckDuplicateVerticesVec3D.Clear();
            _noveltyVertices.Clear();
        }
    }
}
