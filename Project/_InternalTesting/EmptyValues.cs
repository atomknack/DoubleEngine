using System;

namespace DoubleEngine
{
    public static partial class InternalTesting
    {/* // moved tests to xUnit project
        internal static class EmptyValues
        {
            internal static void EntryPoint()
            {
                EmptyIndexedPolyVec2D();
            }

            internal static void EmptyMeshFragmentVec3D()
            {
                MeshFragmentVec3D v = MeshFragmentVec3D.Empty;
                Assert.NotNull(v);
                Assert.NotNull(v.vertices);
                Assert.EmptyArray(v.vertices);
                Assert.NotNull(v.triangles);
                Assert.EmptyArray(v.triangles);
            }
            internal static void EmptyMeshFragmentVec2D()
            {
                MeshFragmentVec2D v = MeshFragmentVec2D.Empty;
                Assert.NotNull(v);
                Assert.NotNull(v.vertices);
                Assert.EmptyArray(v.vertices);
                Assert.NotNull(v.triangles);
                Assert.EmptyArray(v.triangles);
            }

            internal static void EmptyIndexedEdgePoly()
            {
                IndexedEdgePoly v = IndexedEdgePoly.Empty;
                Assert.NotNull(v);
                Assert.NotNull(v.allEdges);
                Assert.EmptyArray(v.allEdges);
                Assert.NotNull(v.subPolygons);
                Assert.EmptyArray(v.subPolygons);
            }
            internal static void EmptyIndexedPolyVec2D()
            {
                IndexedPolyVec2D v = IndexedPolyVec2D.Empty;
                Assert.NotNull(v);
                Assert.NotNull(v._vertices);
                Assert.EmptyArray(v._vertices);
                Assert.NotNull(v._slivers);
                Assert.EmptyArray(v._slivers);
                Assert.NotNull(v._iEPoly);
                Assert.IsTrue(v._iEPoly == IndexedEdgePoly.Empty);
            }
        }
        */
    }
}
