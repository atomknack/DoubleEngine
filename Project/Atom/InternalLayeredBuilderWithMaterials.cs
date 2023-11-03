using CollectionLike.Pooled;
using Collections.Pooled;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    internal partial class InternalLayeredBuilderWithMaterials: IDisposable, IMeshFragmentWithMaterials<Vec3D>
    {
        internal partial class LayerWithMaterials{}

        private LayerWithMaterials EmptyLayer;

        MeshBufferBuilder _builder;
        internal int _xLength;
        internal int _yLength;
        internal int _zLength;
        //internal PoolListVec3D _verticesBuffer;
        internal PoolListIndexedTri _trianglesOfCollapsedLayersBuffer;
        internal PooledList<MaterialByte> _materialsOfTrianglesOfCollapsedLayersBuffer;
        private PooledStack<LayerWithMaterials> _cleanLayers;
        private PooledQueue<LayerWithMaterials> _queue;
        private LayerWithMaterials _lastAddedLayer;
        private LayerWithMaterials _lastCollapsedLayer;
        private PoolListInt _nearCellVerticesBuffer;
        protected ThreeDimensionalCell[] _cellBuffer;
        protected Vec3I _current;
        protected Vec3D _currentVec3D;

        //internal MeshFragmentVec3D _buildedResult;
        private bool _haveBuilded;
        private bool _disposed;

        public ReadOnlySpan<byte> FaceMaterials => _materialsOfTrianglesOfCollapsedLayersBuffer.AsReadOnlySpan();

        public ReadOnlySpan<Vec3D> Vertices => _builder.Vertices;

        public ReadOnlySpan<int> Triangles => _builder.Triangles;

        public ReadOnlySpan<IndexedTri> Faces => _builder.Faces;

        internal static InternalLayeredBuilderWithMaterials Create(int lengthX, int lengthY, int lengthZ) =>
            new InternalLayeredBuilderWithMaterials(lengthX, lengthY, lengthZ);

        internal MeshFragmentVec3DWithMaterials BuildedFragmentWithMaterials()
        {
            //Debug.Log(String.Join(',', _materialsOfTrianglesOfCollapsedLayersBuffer));
            if (_haveBuilded == false)
                throw new InvalidOperationException("Should be called only after Build(ThreeDimensionalGridBase grid)");
            return MeshFragmentVec3DWithMaterials.Create(_builder.BuildFragment(), _materialsOfTrianglesOfCollapsedLayersBuffer);
        }
        internal MeshFragmentVec3D BuildedFragment()
        {
            //Debug.Log(String.Join(',', _materialsOfTrianglesOfCollapsedLayersBuffer));
            if (_haveBuilded == false)
                throw new InvalidOperationException("Should be called only after Build(ThreeDimensionalGridBase grid)");
            return _builder.BuildFragment();
        }

        //internal MeshFragmentVec3D Build(IThreeDimensionalGrid grid)
        internal void Build(IThreeDimensionalGrid grid)
        {
            //_buildedResult = null;
            //_builder = new MeshBufferBuilder(_xLength * _yLength * _zLength * 20, _xLength * _yLength * _zLength * 30);
            _trianglesOfCollapsedLayersBuffer.Clear();
            _materialsOfTrianglesOfCollapsedLayersBuffer.Clear();

            _builder.Clear();
            _lastAddedLayer = EmptyLayer;
            _lastCollapsedLayer = EmptyLayer;
            //var gridDimensions = grid.GetDimensions();
            //if (gridDimensions.x != _xLength || gridDimensions.y != _yLength || gridDimensions.z != _zLength)
            //    throw new ArgumentException("grid size different from layered builder");
            for (int yi = 0; yi < _yLength; ++yi)
            {
                //Debug.Log($"layer {yi}, _queue.Count {_queue.Count}, _cleanLayers.Count{_cleanLayers.Count}");
                if (_queue.Count > 2)
                    CollapseLayer(_queue.Dequeue());
                AddLayer(yi, grid);
            }

            while (_queue.Count > 0)
                CollapseLayer(_queue.Dequeue());
            RestoreLastCollapsedToClean();

            _builder.ResetTriangles();
            _builder.AddFaces(_trianglesOfCollapsedLayersBuffer.AsReadOnlySpan());
            //_buildedResult = _builder.Build();
            _haveBuilded = true;
            //return _buildedResult;
        }

        private void AddLayer(int yi, IThreeDimensionalGrid grid)
        {
            //_cleanLayers.Push(Layer.Create(_xLength, _zLength)); //DebugOnly
            LayerWithMaterials layer = _cleanLayers.Pop();

            for (int xi = 0; xi<_xLength; ++xi)
                for(int zi = 0; zi<_zLength; ++zi)
                {
                    _nearCellVerticesBuffer.Clear();
                    _lastAddedLayer.AppendLayersNearCellVerticesToBuffer(xi,zi,_nearCellVerticesBuffer);
                    layer.AppendLayersNearCellVerticesToBuffer(xi, zi,_nearCellVerticesBuffer);

                    _current = new Vec3I(xi, yi, zi);
                    _currentVec3D = new Vec3D(xi, yi, zi);
                    AddCell(grid, layer);

                }
            _queue.Enqueue(layer);
            _lastAddedLayer = layer;
        }
        private void CollapseLayer(LayerWithMaterials layer)
        {
                LayerWithMaterials nextLayer;
#pragma warning disable CA2000 // Dispose objects before losing scope
            if (!_queue.TryPeek(out nextLayer))
                    nextLayer = LayerWithMaterials.Empty;
#pragma warning restore CA2000 // Dispose objects before losing scope

            ROSpanVec3D vertices = _builder.Vertices;

            for (int xi = 0; xi < _xLength; ++xi)
                    for (int zi = 0; zi < _zLength; ++zi)
                    {
                    _nearCellVerticesBuffer.Clear();
                    _lastCollapsedLayer.AppendLayersNearCellVerticesToBuffer(xi, zi, _nearCellVerticesBuffer);
                    layer.AppendLayersNearCellVerticesToBuffer(xi, zi, _nearCellVerticesBuffer);
                    nextLayer.AppendLayersNearCellVerticesToBuffer(xi, zi, _nearCellVerticesBuffer);

                    var cellTriangles = layer.GetCellTriangles(xi, zi);
                    //_trianglesOfCollapsedLayersBuffer.AppendSpan(cellTriangles);
                    int before = _trianglesOfCollapsedLayersBuffer.Count;
                    foreach (var tri in cellTriangles)
                        _trianglesOfCollapsedLayersBuffer.AddTriangleAndSplitIfNeeded(tri, _nearCellVerticesBuffer.AsReadOnlySpan(), vertices);
                    int after = _trianglesOfCollapsedLayersBuffer.Count;
                    //Debug.Log(layer.GetCellMaterial(xi, zi));
                    _materialsOfTrianglesOfCollapsedLayersBuffer.AddRepeating(layer.GetCellMaterial(xi, zi), after - before);
                    //_materialsOfTrianglesOfCollapsedLayersBuffer.A
                }

            RestoreLastCollapsedToClean();
            _lastCollapsedLayer = layer;
        }

        private void RestoreLastCollapsedToClean()
        {
            if (_lastCollapsedLayer.IsNotEqualToEmpty())
            {
                _lastCollapsedLayer.ClearForReuse();
                _cleanLayers.Push(_lastCollapsedLayer);
            }
        }

        private void AddCell(IThreeDimensionalGrid grid, LayerWithMaterials layer)
        {
            _builder.ResetTriangles();
            _builder.ResetNoveltyVertices();
            _builder.SetToCheckDuplicateVertices(_nearCellVerticesBuffer);

            ThreeDimensionalCell cell = grid.GetCellAndNeighbours(_current.x, _current.y, _current.z, _cellBuffer);
            DeshelledCubeMesh mesh = ThreeDimensionalCellMeshes.GetDeshelled(cell.meshID);
            Grid6SidesCached orientation = Grid6SidesCached.GetCached(cell.grid6SidesCachedIndex);
            if (orientation._invertedY)
                AddCellMeshFragment(mesh.core, orientation.Scale, orientation._rotation);
            else
                AddCellMeshFragment(mesh.core, orientation._rotation);

            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.zNegative);//, _cellBuffer, AddCellMeshFragment);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.xNegative);//, _cellBuffer, AddCellMeshFragment);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.yNegative);//, _cellBuffer, AddCellMeshFragment);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.zPositive);//, _cellBuffer, AddCellMeshFragment);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.xPositive);//, _cellBuffer, AddCellMeshFragment);
            AddCombinedSideToBuilder(mesh, orientation, CubeSides.Enum.yPositive);//, _cellBuffer, AddCellMeshFragment);

            //current. //current Layer
            layer.SetCellNoveltyVertices(_current.x, _current.z, _builder.GetNoveltyVertices());
            layer.SetCellTriangles(_current.x, _current.z, _builder.Faces);
            layer.SetCellMaterial(_current.x, _current.z, cell.material);
            //throw new NotImplementedException();
        }

        protected virtual void AddCellMeshFragment(MeshFragmentVec3D fragment, Vec3D Scale, QuaternionD rotation)
        {
            _builder.AddMeshFragment(fragment, Scale, rotation, _currentVec3D);
        }
        protected virtual void AddCellMeshFragment(MeshFragmentVec3D fragment, QuaternionD rotation)
        {
            _builder.AddMeshFragment(fragment, rotation, _currentVec3D);
        }

        protected void AddCombinedSideToBuilder(DeshelledCubeMesh mesh, Grid6SidesCached orientation, CubeSides.Enum side)//, ThreeDimensionalCell[] cellBuffer, Action<MeshFragmentVec3D, QuaternionD> actionAddCellMeshFragment)
        {
            int sideIndex = (int)side;
            FlatNode flatNode = ThreeDimensionalGridBase.SideFlatNode[sideIndex](mesh, orientation);//mesh.ZNegativeFlatNode(orientation);
            ThreeDimensionalCell complimentary = _cellBuffer[sideIndex];//cellBuffer[(int)Grid6Sides.SideEnum.zNegative];
            FlatNode complimentaryNode = ThreeDimensionalGridBase.SideComplementAsNeighbourFlatNode[sideIndex](
                ThreeDimensionalCellMeshes.GetDeshelled(complimentary.meshID),
                Grid6SidesCached.GetCached(complimentary.grid6SidesCachedIndex));//complimentaryDeshelled.ZPositiveComplementFlatNode(Grid6SidesCached.GetCached(complimentary.Grid6SidesCachedIndex));
            MeshFragmentVec3D sideMesh3D = FlatNodesComplimentaryNotOptimizedMatcher.GetFlatnodeComplimentaryAs3D(
                flatNode.id, flatNode.flatTransform, complimentaryNode.id, complimentaryNode.flatTransform);
            //if (sideMesh3D.Faces.Length == 0)
            //    Debug.Log($"Empty side {side} at {_currentVec3D}");
            AddCellMeshFragment(sideMesh3D, CubeSides.FromZNeg[sideIndex]);
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this); // no finalizer
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                //_buildedResult = null;
                _haveBuilded = false;
                //_verticesBuffer.Dispose();
                _trianglesOfCollapsedLayersBuffer.Dispose();
                _materialsOfTrianglesOfCollapsedLayersBuffer.Dispose();
                while (_cleanLayers.TryPop(out LayerWithMaterials cleanLayer))
                    cleanLayer.Dispose();
                _cleanLayers.Dispose();
                _queue.Dispose();
                _nearCellVerticesBuffer.Dispose();
            }
            _disposed = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "all created will be disposed in Dispose")]
        InternalLayeredBuilderWithMaterials(int lengthX, int lengthY, int lengthZ)
        {
            _haveBuilded = false;
            //_buildedResult = null;

            _cellBuffer = new ThreeDimensionalCell[6];
            _builder = new MeshBufferBuilder(lengthX * lengthZ * lengthY * 20, lengthX * lengthZ * lengthY * 30);
            EmptyLayer = LayerWithMaterials.Empty;

            _xLength = lengthX;
            _yLength = lengthY;
            _zLength = lengthZ;

            //_verticesBuffer = Expendables.CreateList<Vec3D>(lengthX*lengthY*lengthZ*20);
            int trianglesOfCollapsedLayersInitialSize = lengthX * lengthY * lengthZ * 30;
            _trianglesOfCollapsedLayersBuffer = Expendables.CreateList<IndexedTri>(trianglesOfCollapsedLayersInitialSize);
            _materialsOfTrianglesOfCollapsedLayersBuffer = Expendables.CreateList<MaterialByte>(lengthX * lengthY * lengthZ * 30);
            _cleanLayers = Expendables.CreateStack<LayerWithMaterials>(4);
            _cleanLayers.Push(LayerWithMaterials.Create(lengthX, lengthZ));
            _cleanLayers.Push(LayerWithMaterials.Create(lengthX, lengthZ));
            _cleanLayers.Push(LayerWithMaterials.Create(lengthX, lengthZ));
            _cleanLayers.Push(LayerWithMaterials.Create(lengthX, lengthZ));

            _queue = Expendables.CreateQueue<LayerWithMaterials>(4);
            _nearCellVerticesBuffer = Expendables.CreateList<int>(300);
            _lastAddedLayer = EmptyLayer;
            _disposed = false;
        }
    }
}
