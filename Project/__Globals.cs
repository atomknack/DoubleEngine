global using DoubleEngine;
//global using Collections.Pooled;
//global using Arr = System.Collections.Immutable.ImmutableArray;
/*
global using ImmArrInt = System.Collections.Immutable.ImmutableArray<int>;
global using ImmArrVec2D = System.Collections.Immutable.ImmutableArray<DoubleEngine.Vec2D>;
global using ImmArrVec3D = System.Collections.Immutable.ImmutableArray<DoubleEngine.Vec3D>;
*/
global using MaterialByte = System.Byte;

global using IMeshBuilder3D = DoubleEngine.IMeshBuilder<DoubleEngine.MeshFragmentVec3D, DoubleEngine.Vec3D, DoubleEngine.QuaternionD>;

global using ROSpanInt = System.ReadOnlySpan<int>;
global using ROSpanVec2D = System.ReadOnlySpan<DoubleEngine.Vec2D>;
global using ROSpanVec3D = System.ReadOnlySpan<DoubleEngine.Vec3D>;
global using ROSpanIndexedTri = System.ReadOnlySpan<DoubleEngine.IndexedTri>;


global using PoolListInt = Collections.Pooled.PooledList<int>;
global using PoolListVec2D = Collections.Pooled.PooledList<DoubleEngine.Vec2D>;
global using PoolListVec3D = Collections.Pooled.PooledList<DoubleEngine.Vec3D>;
global using PoolListEdgeVec2D = Collections.Pooled.PooledList<DoubleEngine.EdgeVec2D>;
global using PoolListEdgeIndexed = Collections.Pooled.PooledList<DoubleEngine.EdgeIndexed>;
global using PoolListIndexedTri = Collections.Pooled.PooledList<DoubleEngine.IndexedTri>;
global using PoolListRegistryIndex = Collections.Pooled.PooledList<DoubleEngine.RegistryIndex>;
global using PoolListEdgeRegistered = Collections.Pooled.PooledList<DoubleEngine.EdgeRegistered>;
global using PoolListTuple_edgeIndex_splitterPoint = Collections.Pooled.PooledList<(int edgeIndex, DoubleEngine.Vec2D splitterPoint)>;
global using LookUpVec3D = CollectionLike.Pooled.LookUpTable<DoubleEngine.Vec3D>;
global using LookUpInt = CollectionLike.Pooled.LookUpTable<int>;
global using LookUpByte = CollectionLike.Pooled.LookUpTable<byte>;