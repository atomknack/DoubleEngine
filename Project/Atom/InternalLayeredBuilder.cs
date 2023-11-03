/*
using Collections.Pooled;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    [Obsolete("useLayeredBuilderWithMaterials instead")]
    internal partial class InternalLayeredBuilder: IDisposable
    {
        internal partial class Layer{}

        private Layer EmptyLayer;

        MeshBufferBuilder _builder;
        internal int _xLength;
        internal int _yLength;
        internal int _zLength;
        //internal PoolListVec3D _verticesBuffer;
        internal PoolListIndexedTri _trianglesOfCollapsedLayersBuffer;
        private PooledStack<Layer> _cleanLayers;
        private PooledQueue<Layer> _queue;
        private Layer _lastAddedLayer;
        private Layer _lastCollapsedLayer;
        private PoolListInt _nearCellVerticesBuffer;
        protected ThreeDimensionalCell[] _cellBuffer;
        protected Vec3I _current;
        protected Vec3D _currentVec3D;
        private bool _disposed;

        public static InternalLayeredBuilder Create(int lengthX, int lengthY, int lengthZ) =>
            new InternalLayeredBuilder(lengthX, lengthY, lengthZ);

        internal MeshFragmentVec3D Build(ThreeDimensionalGridBase grid)
        {
            _builder = new MeshBufferBuilder(_xLength * _yLength * _zLength * 20, _xLength * _yLength * _zLength * 30);
            _trianglesOfCollapsedLayersBuffer.Clear();

            _builder.Clear();
            _lastAddedLayer = EmptyLayer;
            _lastCollapsedLayer = EmptyLayer;
            var gridDimensions = grid.GetDimensions();
            if (gridDimensions.x != _xLength || gridDimensions.y != _yLength || gridDimensions.z != _zLength)
                throw new ArgumentException("grid size different from layered builder");
            for (int yi = 0; yi < gridDimensions.y; ++yi)
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
            return _builder.Build();
        }

        private void AddLayer(int yi, ThreeDimensionalGridBase grid)
        {
            //_cleanLayers.Push(Layer.Create(_xLength, _zLength)); //DebugOnly
            Layer layer = _cleanLayers.Pop();

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
        private void CollapseLayer(Layer layer)
        {
                Layer nextLayer;
                if (!_queue.TryPeek(out nextLayer))
                    nextLayer = Layer.Empty;

            var vertices = _builder.GetVertices();

            for (int xi = 0; xi < _xLength; ++xi)
                    for (int zi = 0; zi < _zLength; ++zi)
                    {
                    _nearCellVerticesBuffer.Clear();
                    _lastCollapsedLayer.AppendLayersNearCellVerticesToBuffer(xi, zi, _nearCellVerticesBuffer);
                    layer.AppendLayersNearCellVerticesToBuffer(xi, zi, _nearCellVerticesBuffer);
                    nextLayer.AppendLayersNearCellVerticesToBuffer(xi, zi, _nearCellVerticesBuffer);

                    var cellTriangles = layer.GetCellTriangles(xi, zi);
                    //_trianglesOfCollapsedLayersBuffer.AppendSpan(cellTriangles);
                    foreach (var tri in cellTriangles)
                        _trianglesOfCollapsedLayersBuffer.AddTriangleAndSplitIfNeeded(tri, _nearCellVerticesBuffer.AsReadOnlySpan(), vertices);

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

        private void AddCell(IThreeDimensionalGrid grid, Layer layer)
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
            layer.SetCellTriangles(_current.x, _current.z, _builder.GetFaces());
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
                //_verticesBuffer.Dispose();
                _trianglesOfCollapsedLayersBuffer.Dispose();
                while (_cleanLayers.TryPop(out Layer cleanLayer))
                    cleanLayer.Dispose();
                _cleanLayers.Dispose();
                _queue.Dispose();
                _nearCellVerticesBuffer.Dispose();
            }
            _disposed = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "all created will be disposed in Dispose")]
        InternalLayeredBuilder (int lengthX, int lengthY, int lengthZ)
        {
            _cellBuffer = new ThreeDimensionalCell[6];
            _builder = new MeshBufferBuilder(lengthX * lengthZ * lengthY * 20, lengthX * lengthZ * lengthY * 30);
            EmptyLayer = Layer.Empty;

            _xLength = lengthX;
            _yLength = lengthY;
            _zLength = lengthZ;

            //_verticesBuffer = Expendables.CreateList<Vec3D>(lengthX*lengthY*lengthZ*20);
            _trianglesOfCollapsedLayersBuffer = Expendables.CreateList<IndexedTri>(lengthX*lengthY*lengthZ*30);
            _cleanLayers = Expendables.CreateStack<Layer>(4);
            _cleanLayers.Push(Layer.Create(lengthX, lengthZ));
            _cleanLayers.Push(Layer.Create(lengthX, lengthZ));
            _cleanLayers.Push(Layer.Create(lengthX, lengthZ));
            _cleanLayers.Push(Layer.Create(lengthX, lengthZ));

            _queue = Expendables.CreateQueue<Layer>(4);
            _nearCellVerticesBuffer = Expendables.CreateList<int>(300);
            _lastAddedLayer = EmptyLayer;
            _disposed = false;
        }
    }
}
*/