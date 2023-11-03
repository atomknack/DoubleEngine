using System;
using System.Collections.Generic;
using Collections.Pooled;
using Newtonsoft.Json;
using CollectionLike;
using CollectionLike.Comparers.SetsWithCustomComparer;
using CollectionLike.Pooled;

namespace DoubleEngine.Atom
{
    public ref struct DecimatorColored //:IDisposable // using still accept structure
    {
        //[Obsolete("TODO: Move to some external place")]
        private static Func<byte, byte, bool> MaterialsEqual = DEMaterials.MaterialsEqual;
        //prerequisite
        private readonly ReadOnlySpan<Vec3D> _vertices;
        private readonly ReadOnlySpan<IndexedTri> _faces;
        //private readonly _faces;
        internal PooledSet<int> _unremovableSet;
        internal LookUpInt _connectedTo;
        internal LookUpInt _verticeBelongsToFaces;
        internal PooledArrayStruct<Vec3D> _faceNormals;
        internal LookUpVec3D _verticeNormals;
        internal LookUpByte _verticeMaterials; //TODO

        internal PooledQueue<int> _queue;
        internal PooledArrayStruct<int> _claimedBy; 
        internal PooledArrayStruct<double> _distance; //at start _unremovable have distance 0, all other maxFloat
        internal EdgeIndexed.MultiMaterialEdges _edgeMaterialsSet;
        //internal PoolListEdgeIndexed _current;
        //internal PoolListEdgeIndexed _next;

        public static MeshVolatileFragmentWithMaterials DecimateAndReturnViolatileFragment(IMeshFragmentWithMaterials<Vec3D> mesh)
        {
            MeshVolatileFragmentWithMaterials result;
            (var verticesArray, var faces, var faceMaterials) = Decimate(mesh);
            using (faces) using (faceMaterials)
            {
                var facesArray = PooledArrayStruct<IndexedTri>.CreateFromList(faces);
                var facesMaterialsArray = PooledArrayStruct<MaterialByte>.CreateFromList(faceMaterials);
                //buffer.UpdateByReplacingInternalsFrom(verticesArray, facesArray, facesMaterialsArray);
                result = MeshVolatileFragmentWithMaterials.CreateFromWithoutCopy(verticesArray, facesArray, facesMaterialsArray);
            }
            return result;
        }
        /*
        public static void DecimateAndWriteResultToBuffer(IMeshFragmentWithMaterials<Vec3D> mesh, ref MeshVolatileFragmentWithMaterials buffer)
        {
            (var verticesArray, var faces, var faceMaterials) = Decimate(mesh);
            using(faces) using(faceMaterials)
            {
                var facesArray = PooledArrayStruct<IndexedTri>.CreateFromList(faces);
                var facesMaterialsArray = PooledArrayStruct<MaterialByte>.CreateFromList(faceMaterials);
                //buffer.UpdateByReplacingInternalsFrom(verticesArray, facesArray, facesMaterialsArray);
            }
        }
        */

        public static MeshFragmentVec3DWithMaterials DecimateAndReturnMeshFragmentVec3DWithMaterials(IMeshFragmentWithMaterials<Vec3D> mesh)
        {
            (var verticesArray, var faces, var faceMaterials) = Decimate(mesh);
            using (verticesArray) using (faces) using (faceMaterials)
                return MeshFragmentVec3DWithMaterials.Create(MeshFragmentVec3D.CreateMeshFragment(verticesArray.AsReadOnlySpan(), faces.AsReadOnlySpan()), faceMaterials);
        }


        public static (PooledArrayStruct<Vec3D> vertices, PoolListIndexedTri faces, PooledList<MaterialByte> faceMaterials) 
            Decimate(IMeshFragmentWithMaterials<Vec3D> mesh)
        {
            PooledList<IndexedTri> newFaces = Expendables.CreateList<IndexedTri>(mesh.Faces.Length);
            PooledList<MaterialByte> newFacesMaterials = Expendables.CreateList<MaterialByte>(mesh.Faces.Length);
            PooledArrayStruct<Vec3D> newVertices = Decimate(mesh, newFaces, newFacesMaterials);
            return (newVertices, newFaces, newFacesMaterials);
        }
        public static PooledArrayStruct<Vec3D> Decimate(
            IMeshFragmentWithMaterials<Vec3D> mesh, 
            PooledList<IndexedTri> newFacesBuffer,
            PooledList<MaterialByte> newFacesMaterialsBuffer
            )
        {
            var faces = mesh.Faces;
            var vertices = mesh.Vertices;
            var faceMaterials = mesh.FaceMaterials;
            var triangles = mesh.Triangles;
            using var edgesCountingDict = EdgeIndexed.EdgesCountDirectionIgnore(triangles);
            var normals = ExpendablesDoubleEngineSpecific.CreateFaceNormalsFromMeshFragment3D(mesh);
            var verticeNormals = ExpendablesDoubleEngineSpecific.CreateVerticeToNormalsLookUp(vertices.Length, faces, normals);
            var verticeMaterials = ExpendablesDoubleEngineSpecific.CreateVerticeToMaterialsLookUp(vertices.Length, faces, faceMaterials);
            using EdgeIndexed.MultiMaterialEdges edgeMaterialsSet = new EdgeIndexed.MultiMaterialEdges(faces, faceMaterials);
            //throw new NotImplementedException(); // edgeMaterialsSet
            using var edgesCountingWithMaterials = EdgeIndexed.EdgesCountWithMaterialsDirectionIgnore(faces, faceMaterials);
            PooledSet<int> unremovableSet = MakeSetOfUnremovable(vertices, edgesCountingDict, verticeNormals, edgesCountingWithMaterials);
            var verticeBelongsToFaces = ExpendablesDoubleEngineSpecific.CreateVerticeToFacesLookUp(vertices.Length, faces);
            using var decimator = new DecimatorColored(
                vertices,
                faces,
                unremovableSet,
                normals,
                verticeNormals,
                verticeMaterials,
                edgesCountingDict,
                edgeMaterialsSet,
                verticeBelongsToFaces);
            //Debug.Log("decimator created but not started");
            decimator.CalcVerticeRemapTable();
            //Debug.Log("decimator after CalcShortestPaths");
            var claimedBy = decimator._claimedBy.AsReadOnlySpan();

            newFacesBuffer.Clear();
            newFacesMaterialsBuffer.Clear();
            MeshUtil.AddRemappedNonDegenerateFacesToBuffers(claimedBy, faces, faceMaterials, newFacesBuffer, newFacesMaterialsBuffer );
            PooledArrayStruct<Vec3D> newVertices =
                MeshUtil.RemoveUnusedVerticesReturningPooledArrayAndReindexFaces_Span(newFacesBuffer.AsReadOnlySpan(), newFacesBuffer.AsSpan(), vertices);
            return newVertices;
        }

        private static PooledSet<int> MakeSetOfUnremovable(
            ReadOnlySpan<Vec3D> vertices, 
            PooledDictionary<EdgeIndexed, int> edgesCountingDict, 
            LookUpTable<Vec3D> verticeNormals,
            PooledDictionary<(EdgeIndexed, MaterialByte), int> edgesCountingDictWithMaterials)
        {
            PooledSet<int> unremovableSet = SingleEdgesVertices(edgesCountingDict);
            int length = vertices.Length;
            for (int i = 0; i < length; i++)
                if (verticeNormals.GetValues(i).Length >= 3)
                    unremovableSet.Add(i);
            //Debug.Log(String.Join(',', edgesCountingDictWithMaterials.Keys));
            //Debug.Log(String.Join(',', EdgeIndexed.SingleEdgesWithMaterialsFromCountingDictionary(edgesCountingDictWithMaterials)));
            using PooledDictionary<int, Vec3D> verticeEdge = Expendables.CreateDictionary<int, Vec3D>(180);
            foreach (var edgeMaterial in EdgeIndexed.SingleEdgesWithMaterialsFromCountingDictionary(edgesCountingDictWithMaterials))
            {
                EdgeIndexed edge = edgeMaterial.Item1;
                MaterialByte material = edgeMaterial.Item2;
                CheckMaterialVertice(vertices, edge.start, edge.end);
                CheckMaterialVertice(vertices, edge.end, edge.start);
            }
            
            //Debug.Log(String.Join(',', unremovableSet));
            //TODO add vertices that have othogonal single edges with materials to unremovable set
            
            //SingleEdgesWithMaterialsFromCountingDictionary


            return unremovableSet;
            
            void CheckMaterialVertice(ReadOnlySpan<Vec3D> vertices, int current, int other)
            {
                if(unremovableSet.Contains(current))
                    return;
                Vec3D edgeDirection = vertices[other] - vertices[current];
                if (verticeEdge.ContainsKey(current))
                {
                    Vec3D exisingDirection = verticeEdge[current];
                    if(Vec3D.Cross(edgeDirection, exisingDirection).MagnitudeSqr() > 0.000001) //need testing. compare edges by dot? cross?
                        unremovableSet.Add(current);
                    return;
                }
                verticeEdge.Add(current, edgeDirection);
            }
            
        }

        public void Dispose()
        {
            _unremovableSet.Dispose();
            _connectedTo.Dispose();
            _verticeNormals.Dispose();
            _queue.Dispose();
            //_claimedBy.Dispose(); TODO Change when change Decimate method
            _edgeMaterialsSet.Dispose();
            _distance.Dispose();
            //throw new NotImplementedException();
        }

        private DecimatorColored(
            ReadOnlySpan<Vec3D> vertices, 
            ReadOnlySpan<IndexedTri> faces, 
            PooledSet<int> unremovableSet, 
            PooledArrayStruct<Vec3D> faceNormals,
            LookUpVec3D verticeNormals,
            LookUpByte verticeMaterials,
            PooledDictionary<EdgeIndexed, int> edgeCountingDictionary,
            EdgeIndexed.MultiMaterialEdges edgeMaterialsSet,
            LookUpInt verticeBelongsToFaces) // todo: replace by constructor
        {
            _vertices = vertices;
            _faces = faces;
            _faceNormals = faceNormals;
            _verticeNormals = verticeNormals;
            _verticeMaterials = verticeMaterials;
            _unremovableSet = unremovableSet;
            //TODO add to _unremovableSet vertice with 2 or more materials
            _queue = Expendables.CreateQueueFromSet(_unremovableSet);
            _connectedTo = ExpendablesDoubleEngineSpecific.CreateConnectionsLookUp(_vertices.Length, edgeCountingDictionary);
            _edgeMaterialsSet = edgeMaterialsSet;
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
                    if ( FaceShouldBeRestored(i, oldFace, newFace))
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
        private bool FaceShouldBeRestored(int i, IndexedTri oldFace, IndexedTri newFace)
        {
            if (newFace.NotDegenerate())
            {
                if (!NormalsCloseEnough(_faceNormals[i], newFace.Normal(_vertices)))
                {
                    return true;
                }
                /*
                if (_edgeMaterialsSet.IsEdgeMultiMaterial(newFace.v0, newFace.v1) && 
                    !OldVectorCloseToNewOne(oldFace.v0, oldFace.v1, newFace.v0, newFace.v1))
                {
                    return true;
                }
                if (_edgeMaterialsSet.IsEdgeMultiMaterial(newFace.v1, newFace.v2) &&
                    !OldVectorCloseToNewOne(oldFace.v1, oldFace.v2, newFace.v1, newFace.v2))
                {
                    return true;
                }
                if (_edgeMaterialsSet.IsEdgeMultiMaterial(newFace.v2, newFace.v0) &&
                    !OldVectorCloseToNewOne(oldFace.v2, oldFace.v0, newFace.v2, newFace.v0))
                {
                    return true;
                }*/
                /*
                int v0MatCount = _verticeMaterials.GetValuesLength(oldFace.v0);
                int v1MatCount = _verticeMaterials.GetValuesLength(oldFace.v1);
                int v2MatCount = _verticeMaterials.GetValuesLength(oldFace.v2);
                if (v0MatCount > 1 && v1MatCount > 1 &&
                    !OldVectorCloseToNewOne(oldFace.v0, oldFace.v1, newFace.v0, newFace.v1))
                        return true;
                if (v1MatCount > 1 && v2MatCount > 1 &&
                    !OldVectorCloseToNewOne(oldFace.v1, oldFace.v2, newFace.v1, newFace.v2))
                        return true;
                if (v2MatCount > 1 && v0MatCount > 1 &&
                    !OldVectorCloseToNewOne(oldFace.v2, oldFace.v0, newFace.v2, newFace.v0))
                        return true;
                */
            }
            return false;
        }
        
        private bool OldVectorCloseToNewOne(int oldA, int oldB, int newA, int newB)
        {
            return NormalsCloseEnough(
                        Vec3D.NormalFromLineSegment(_vertices[oldA], _vertices[oldB]),
                        Vec3D.NormalFromLineSegment(_vertices[newA], _vertices[newB]));
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
            var vertexMaterials = _verticeMaterials.GetValues(vertex);
            var proxyMaterials = _verticeMaterials.GetValues(proxy);
            if ( vertexMaterials.SubsetOf(proxyMaterials, MaterialsEqual))
            {
                if(vertexMaterials.Length>1 && proxyMaterials.Length>1 && !_edgeMaterialsSet.IsEdgeMultiMaterial(vertex,proxy))
                    return false;
            }
            else
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
