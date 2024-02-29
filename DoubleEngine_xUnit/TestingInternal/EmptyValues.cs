using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleEngine;
using DoubleEngine.Atom;

namespace DoubleEngine_xUnit.TestingInternal
{
    public class EmptyValues
    {
        [Fact]
        internal static void EmptyMeshFragmentVec3D()
        {
            MeshFragmentVec3D v = MeshFragmentVec3D.Empty;
            Assert.NotNull(v);
            Assert.NotNull(v.Vertices.ToArray());
            Assert.EmptyArray(v.Vertices.ToArray());
            Assert.NotNull(v.Triangles.ToArray());
            Assert.EmptyArray(v.Triangles.ToArray());
        }
        [Fact]
        internal static void EmptyMeshFragmentVec2D()
        {
            MeshFragmentVec2D v = MeshFragmentVec2D.Empty;
            Assert.NotNull(v);
            Assert.NotNull(v.Vertices.ToArray());
            Assert.EmptyArray(v.Vertices.ToArray());
            Assert.NotNull(v.Triangles.ToArray());
            Assert.EmptyArray(v.Triangles.ToArray());
        }
        [Fact]
        internal static void EmptyIndexedEdgePoly()
        {
            IndexedEdgePoly v = IndexedEdgePoly.Empty;
            Assert.NotNull(v);
            Assert.NotNull(v.allEdges);
            Assert.EmptyArray(v.allEdges);
            Assert.NotNull(v.subPolygons);
            Assert.EmptyArray(v.subPolygons);
        }
        [Fact]
        internal static void EmptyIndexedPolyVec2D()
        {
            IndexedPolyVec2D v = IndexedPolyVec2D.Empty;
            Assert.NotNull(v);
            Assert.NotNull(v.GetVertices().ToArray());
            Assert.EmptyArray(v.GetVertices().ToArray());
            Assert.NotNull(v.GetSlivers().ToArray());
            Assert.EmptyArray(v.GetSlivers().ToArray());
            Assert.NotNull(v.GetIndexedEdgePoly());
            Assert.True(v.GetIndexedEdgePoly() == IndexedEdgePoly.Empty);
        }
    }
}
