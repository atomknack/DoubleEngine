using System;
using System.Collections.Generic;
using System.Text;
using CollectionLike;
using CollectionLike.Pooled;

namespace DoubleEngine.Atom
{
    internal partial class InternalLayeredBuilderWithMaterials
    {
        internal partial class LayerWithMaterials : IDisposable
        {
            public static LayerWithMaterials Empty => new LayerWithMaterials();
            public bool IsNotEqualToEmpty() => _LengthX != 0 && _LengthZ != 0;
            //internal PoolListVec3D _gridBuilderVertices;
            internal int _LengthX;
            internal int _LengthZ;
            internal PoolListInt _layerVerticesIndexes;
            internal RelativeMemory[,] _cellNewVertices;
            internal PoolListIndexedTri _layerTriangles;
            internal RelativeMemory[,] _cellTriangles;
            internal MaterialByte[,] _cellMaterial;
            //internal (int x, int z) _lastAddedCell;
            private bool _disposed;

            internal void SetCellMaterial(int x, int z, MaterialByte material)
            {
                _cellMaterial[x, z] = material;
            }
            internal void SetCellTriangles(int x, int z, ReadOnlySpan<IndexedTri> triangles)
            {
                _cellTriangles[x, z] = _layerTriangles.AppendSpanAndReturnRelativeMemory(triangles);
            }
            internal MaterialByte GetCellMaterial(int x, int z) => _cellMaterial[x, z];
            internal ReadOnlySpan<IndexedTri> GetCellTriangles(int x, int z) => 
                _layerTriangles.GetRelativeSpan(_cellTriangles[x, z]);

            internal void SetCellNoveltyVertices(int x, int z, ReadOnlySpan<int> indexes)
            {
                _cellNewVertices[x, z] = _layerVerticesIndexes.AppendSpanAndReturnRelativeMemory(indexes);
            }

            internal static LayerWithMaterials Create(int xSize, int zSize)
            {
                LayerWithMaterials result = new LayerWithMaterials(xSize, zSize);
                result.ClearForReuse();
                return result;
            }

            internal void AppendLayersNearCellVerticesToBuffer(int x, int z, PoolListInt buffer)
            {
                for(int xi = x-1; xi < x+2; ++xi)
                    for(int zi = z-1; zi < z+2; ++zi)
                        AppendCellAddedVerticesToBuffer(xi, zi, buffer);
            }
            private void AppendCellAddedVerticesToBuffer(int x, int z, PoolListInt buffer)
            {
                if(x<0 || z<0 || x>= _LengthX || z>= _LengthZ)
                    return;
                if (_cellNewVertices[x, z].length == 0)
                    return;
                ReadOnlySpan<int> cellVertices = _cellNewVertices[x, z].GetReadOnlySpan<int>(_layerVerticesIndexes.Span);
                for (int i = 0; i < cellVertices.Length; i++)
                    if( ! buffer.Contains(cellVertices[i]))
                        buffer.Add(cellVertices[i]);
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
                    _layerVerticesIndexes?.Dispose();
                    _layerTriangles?.Dispose();
                }
                _disposed = true;
            }


            internal void ClearForReuse()
            {
                //_lastAddedCell = (-1, -1);
                //int lx = _cellNewVertices.GetLength(0);
                //int lz = _cellNewVertices.GetLength(1);
                for (int xi = 0; xi < _LengthX; ++xi)
                    for (int zi = 0; zi < _LengthZ; ++zi)
                    {
                        _cellNewVertices[xi, zi] = RelativeMemory.Empty;
                        _cellTriangles[xi, zi] = RelativeMemory.Empty;
                        _cellMaterial[xi, zi] = ThreeDimensionalCell.DefaultMaterial;
                    }
                _layerVerticesIndexes.Clear();
                _layerTriangles.Clear();
            }
            private LayerWithMaterials(int xSize, int zSize)//, PoolListVec3D gridBuilderVerticesBuffer)
            {
                if (xSize < 0 || zSize < 0)
                    throw new ArgumentOutOfRangeException($"size of layer cannot be negative sizes: x {xSize}, z {zSize}");
                _LengthX = xSize;
                _LengthZ = zSize;
                //_gridBuilderVertices = gridBuilderVerticesBuffer;
                _layerVerticesIndexes = Expendables.CreateList<int>(_LengthX * _LengthZ * 100);
                _cellNewVertices = new RelativeMemory[_LengthX, _LengthZ];
                _layerTriangles = Expendables.CreateList<IndexedTri>(_LengthX * _LengthZ * 100);
                _cellTriangles = new RelativeMemory[_LengthX, _LengthZ];
                _cellMaterial = new MaterialByte[_LengthX, _LengthZ];
                _disposed = false;
            }
            private LayerWithMaterials()
            {
                _LengthX = 0;
                _LengthZ = 0;
                _layerVerticesIndexes = null;
                _cellNewVertices = new RelativeMemory[0,0];
                _layerTriangles = null;
                _cellTriangles = new RelativeMemory[0,0];
                _disposed = true;
            }
            //internal bool TryGetVertices(int x, int z, out Rea)
        }

    }
}
