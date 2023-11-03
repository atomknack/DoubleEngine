using System;
using Newtonsoft.Json;
using Collections.Pooled;
using CollectionLike;
using CollectionLike.Comparers.SetsWithCustomComparer;
using CollectionLike.Pooled;

namespace DoubleEngine.Atom
{
    public ref struct Decimator3D //:IDisposable // using still accept structure
    {
        //prerequisite
        private readonly ReadOnlySpan<Vec3D> _vertices;
        private readonly ReadOnlySpan<IndexedTri> _faces;
        //private readonly _faces;
        internal PooledSet<int> _unremovableSet;
        internal LookUpInt _connectedTo;
        internal LookUpInt _verticeBelongsToFaces;
        internal PooledArrayStruct<Vec3D> _faceNormals;
        internal LookUpVec3D _verticeNormals;
        //internal LookUpByte _verticeMaterials; //TODO

        internal PooledQueue<int> _queue;
        internal PooledArrayStruct<int> _claimedBy; 
        internal PooledArrayStruct<double> _distance; //at start _unremovable have distance 0, all other maxFloat
        //internal PoolListEdgeIndexed _current;
        //internal PoolListEdgeIndexed _next;

        public static MeshFragmentVec3D _for_testing_only_Decimate(MeshFragmentVec3D mesh)
        {
            var faces = mesh.Faces;
            var vertices = mesh.vertices;
            using var edgesCountingDict = EdgeIndexed.EdgesCountDirectionIgnore(mesh.Triangles);
            var normals = ExpendablesDoubleEngineSpecific.CreateFaceNormalsFromMeshFragment3D(mesh);
            var verticeNormals = ExpendablesDoubleEngineSpecific.CreateVerticeToNormalsLookUp(vertices.Length, faces, normals);
            PooledSet<int> unremovableSet = MakeSetOfUnremovable(vertices, edgesCountingDict, verticeNormals);
            var verticeBelongsToFaces = ExpendablesDoubleEngineSpecific.CreateVerticeToFacesLookUp(vertices.Length, faces);
            using var decimator = new Decimator3D(
                vertices, 
                faces, 
                unremovableSet, 
                normals, 
                verticeNormals, 
                edgesCountingDict, 
                verticeBelongsToFaces);
            //Debug.Log("decimator created but not started");
            decimator.CalcVerticeRemapTable();
            //Debug.Log("decimator after CalcShortestPaths");
            var claimedBy = decimator._claimedBy;

            using PooledList<IndexedTri> newFaces = Expendables.CreateList<IndexedTri>(mesh.Triangles.Length);
            foreach (var face in faces)
            {
                IndexedTri newFace = new IndexedTri(claimedBy[face.v0], claimedBy[face.v1], claimedBy[face.v2]);
                if (newFace.NotDegenerate())
                    newFaces.Add(newFace);
            }
            //Debug.Log(newFaces.Count*3);
            using PooledArrayStruct<Vec3D> verticesAfter =
                MeshUtil.RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_Span(newFaces.AsReadOnlySpan(), newFaces.AsSpan(), vertices.AsReadOnlySpan());
            //Debug.Log(trianglesAfter.Count);
            //var newMesh = MeshFragmentVec3D.CreateMeshFragment(vertices, newFaces.AsReadOnlySpan()).MakeFragmentWhereAllVerticesIs(_ => true);
            //Debug.Log(JsonConvert.SerializeObject(newMesh));
            return MeshFragmentVec3D.CreateMeshFragment(verticesAfter.AsReadOnlySpan(), newFaces.AsReadOnlySpan());
        }

        private static PooledSet<int> MakeSetOfUnremovable(Vec3D[] vertices, PooledDictionary<EdgeIndexed, int> edgesCountingDict, LookUpTable<Vec3D> verticeNormals)
        {
            PooledSet<int> unremovableSet = SingleEdgesVertices(edgesCountingDict);
            int length = vertices.Length;
            for (int i = 0; i < length; i++)
                if (verticeNormals.GetValues(i).Length >= 3)
                    unremovableSet.Add(i);
            return unremovableSet;
        }

        public void Dispose()
        {
            _unremovableSet.Dispose();
            _connectedTo.Dispose();
            _verticeNormals.Dispose();
            _queue.Dispose();
            //_claimedBy.Dispose(); TODO Change when change Decimate method
            _distance.Dispose();
            //throw new NotImplementedException();
        }

        private Decimator3D(
            ReadOnlySpan<Vec3D> vertices, 
            ReadOnlySpan<IndexedTri> faces, 
            PooledSet<int> unremovableSet, 
            PooledArrayStruct<Vec3D> faceNormals,
            LookUpVec3D verticeNormals, 
            PooledDictionary<EdgeIndexed, int> edgeCountingDictionary,
            LookUpInt verticeBelongsToFaces) // todo: replace by constructor
        {
            _vertices = vertices;
            _faces = faces;
            _faceNormals = faceNormals;
            _verticeNormals = verticeNormals;
            _unremovableSet = unremovableSet;
            //TODO add to _unremovableSet vertice with 2 or more materials
            _queue = Expendables.CreateQueueFromSet(_unremovableSet);
            _connectedTo = ExpendablesDoubleEngineSpecific.CreateConnectionsLookUp(_vertices.Length, edgeCountingDictionary);
            _verticeBelongsToFaces = verticeBelongsToFaces;
            //throw new NotImplementedException();

            _claimedBy = Expendables.CreateIncreasingArray(_vertices.Length); // at start all vertices claimed self
            _distance = CreateDistance(_vertices.Length, _unremovableSet); 
        }

        private static PooledArrayStruct<double> CreateDistance(int length, PooledSet<int> unremovables)
        {
            PooledArrayStruct<double> result = Expendables.CreateArray<double>(length);
            const double maxValue = double.MaxValue;
            result.Fill(maxValue);
            foreach (var unremovable in unremovables)
                result[unremovable] = 0;
            return result;
        }

        private static PooledSet<int> SingleEdgesVertices(PooledDictionary<EdgeIndexed, int> edgesCount)
        {
            var set = Expendables.CreateSet<int>(1000);
            foreach(var edge in EdgeIndexed.SingleEdgesFromCountingDictionary(edgesCount))
            {
                set.Add(edge.start);
                set.Add(edge.end);
            }
            return set;
        }

        internal void CalcVerticeRemapTable()
        {
            CalcShortestPaths();
            //using PooledQueue<int> restore = Expendables.CreateQueue<int>(100);
            bool atleastOneFaceRestored = true;
            while (atleastOneFaceRestored)
            {
                atleastOneFaceRestored = false;
                for (int i = 0; i < _faces.Length; ++i)
                {
                    var oldFace = _faces[i];
                    var newFace = new IndexedTri(_claimedBy[oldFace.v0], _claimedBy[oldFace.v1], _claimedBy[oldFace.v2]);
                    if (newFace.NotDegenerate() && !NormalsCloseEnough(_faceNormals[i], newFace.Normal(_vertices)))
                        {
                        atleastOneFaceRestored = true;
                        //restore.Enqueue(i);
                        _claimedBy[oldFace.v0] = oldFace.v0;
                        _claimedBy[oldFace.v1] = oldFace.v1;
                        _claimedBy[oldFace.v2] = oldFace.v2;
                    }
                }
                /*while(restore.TryDequeue(out int i))
                {
                    var oldFace = _faces[i];
                    _claimedBy[oldFace.v0] = oldFace.v0;
                    _claimedBy[oldFace.v1] = oldFace.v1;
                    _claimedBy[oldFace.v2] = oldFace.v2;
                }*/
            }
        }
        private void CalcShortestPaths()
        {
            //var unremovable = _unremovableSet.CreatePooledArray();

            while(_queue.TryDequeue(out int node))
            {
                CalcForVertice(node);
            }

        }

        private void CalcForVertice(int node)
        {
            double nodeDistance = _distance[node];
            foreach (int adjacent in _connectedTo.GetValues(node))
            {
                double fullDistanceToAdjacent = nodeDistance + Vec3D.DistanceSqr(_vertices[node], _vertices[adjacent]);
                int unremovable = _claimedBy[node];
                if (_distance[adjacent] > fullDistanceToAdjacent && CanBeJoined(unremovable,node, adjacent))
                {
                    _claimedBy[adjacent] = unremovable;
                    _distance[adjacent] = fullDistanceToAdjacent;
                    _queue.Enqueue(adjacent);
                }
            }
        }

        private static bool NormalsCloseEnough(Vec3D a, Vec3D b) => a.CloseByEach(b, 0.001);
        private bool CanBeJoined(int unremovable, int proxy, int vertex)
        {
            if ( !_verticeNormals.GetValues(vertex).SubsetOf(_verticeNormals.GetValues(unremovable), Vec3DComparer_2em5.StaticEquals))
                return false;
            
            var facesIndices = _verticeBelongsToFaces.GetValues(vertex);
            foreach (var faceIndex in facesIndices)
            {
                IndexedTri oldFace = _faces[faceIndex];
                Vec3D oldFaceNormal = _faceNormals[faceIndex];
                IndexedTri replaced = oldFace.ReplacedVertice(vertex, proxy);
                if ( replaced.NotDegenerate() && 
                    ( ! NormalsCloseEnough(oldFaceNormal, replaced.Normal(_vertices))) )
                    return false;
            }
            
            //TODO:
            //removable vertices can be wielded to the unremovable vertice if unremovablevertice have all materials of removable.
            //removable vertices can be wielded together if they have same materials.

            return true;

            //throw new NotImplementedException();
            //removable vertices can be wielded to the unremovable vertice if unremovablevertice have all normals of removable.
            //removable vertices can be wielded together if they have same normals.
        }

        /*
        public bool DecimationStep()
        {
            ReadOnlySpan<EdgeIndexed> edges = _current.AsReadOnlySpan();
            _next.Clear();
            for (int i = 0; i < edges.Length; ++i)
            {
                EdgeIndexed edge = edges[i];
                bool unremovableStart = NotToBeClaimed(edge.start);
                bool unremovableEnd = NotToBeClaimed(edge.end);
                if (unremovableStart && unremovableEnd)
                    continue;
                if (!unremovableStart && !unremovableEnd)
                {
                    _next.Add(edge);
                    continue;
                }
                if (unremovableStart)
                    MakeAClaim(edge.start, edge.end);
                else
                    MakeAClaim(edge.end, edge.start);
            }

            throw new NotImplementedException();
            (_current, _next) = (_next, _current);
        }

        public bool NotToBeClaimed(int verticeIndex) => _unremovable.Contains(verticeIndex);

        public void MakeAClaim(int unremovableProxyIndex, int consumableIndex)
        {
            throw new NotImplementedException();
        }*/
    }
}
