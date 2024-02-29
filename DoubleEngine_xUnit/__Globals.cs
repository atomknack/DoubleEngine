global using DoubleEngine;
//global using Collections.Pooled;
//global using Arr = System.Collections.Immutable.ImmutableArray;
/*
global using ImmArrInt = System.Collections.Immutable.ImmutableArray<int>;
global using ImmArrVec2D = System.Collections.Immutable.ImmutableArray<DoubleEngine.Vec2D>;
global using ImmArrVec3D = System.Collections.Immutable.ImmutableArray<DoubleEngine.Vec3D>;
*/
global using MaterialByte = System.Byte;

global using IMeshBuilder3D = DoubleEngine.IMeshBuilder<DoubleEngine.MeshFragmentVec3D, VectorCore.Vec3D, VectorCore.QuaternionD>;

global using Vec2D = VectorCore.Vec2D;
global using Vec2I = VectorCore.Vec2I;
global using Vec3D = VectorCore.Vec3D;
global using Vec3I = VectorCore.Vec3I;
global using Vec3F = VectorCore.Vec3F;
global using Vec3short = VectorCore.Vec3short;
global using Vec4D = VectorCore.Vec4D;
global using QuaternionD = VectorCore.QuaternionD;
global using TRS3D = VectorCore.TRS3D;
global using MatrixD4x4 = VectorCore.MatrixD4x4;

global using ROSpanInt = System.ReadOnlySpan<int>;
global using ROSpanVec2D = System.ReadOnlySpan<VectorCore.Vec2D>;
global using ROSpanVec3D = System.ReadOnlySpan<VectorCore.Vec3D>;
global using ROSpanIndexedTri = System.ReadOnlySpan<DoubleEngine.IndexedTri>;


global using PoolListInt = Collections.Pooled.PooledList<int>;
global using PoolListVec2D = Collections.Pooled.PooledList<VectorCore.Vec2D>;
global using PoolListVec3D = Collections.Pooled.PooledList<VectorCore.Vec3D>;
global using PoolListEdgeVec2D = Collections.Pooled.PooledList<DoubleEngine.EdgeVec2D>;
global using PoolListEdgeIndexed = Collections.Pooled.PooledList<DoubleEngine.EdgeIndexed>;
global using PoolListIndexedTri = Collections.Pooled.PooledList<DoubleEngine.IndexedTri>;
global using PoolListRegistryIndex = Collections.Pooled.PooledList<DoubleEngine.RegistryIndex>;
global using PoolListEdgeRegistered = Collections.Pooled.PooledList<DoubleEngine.EdgeRegistered>;
global using PoolListTuple_edgeIndex_splitterPoint = Collections.Pooled.PooledList<(int edgeIndex, VectorCore.Vec2D splitterPoint)>;
global using LookUpVec3D = CollectionLike.Pooled.LookUpTable<VectorCore.Vec3D>;
global using LookUpInt = CollectionLike.Pooled.LookUpTable<int>;
global using LookUpByte = CollectionLike.Pooled.LookUpTable<byte>;