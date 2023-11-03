using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public partial record MeshFragmentVec2D
    {
        [Obsolete("need testing")]
        public static MeshFragmentVec2D Subtract(MeshFragmentVec2D original, MeshFragmentVec2D subtrahend) =>
            SubtractSinglePieceFromSinglePiece(original, subtrahend);

        private static MeshFragmentVec2D SubtractSinglePieceFromSinglePiece(MeshFragmentVec2D original, MeshFragmentVec2D subtrahend)
        {
            //Debug.Log($"try subtract {original!=null} {subtrahend!=null}");

            if (original == null || original == MeshFragmentVec2D.Empty)
                return original;
            if (original.vertices.Length == 0 || original.Triangles.Length == 0)
                return MeshFragmentVec2D.Empty;
            if (subtrahend == null || subtrahend == MeshFragmentVec2D.Empty || 
                subtrahend.vertices.Length==0 || subtrahend.Triangles.Length==0)
                return original;

            //Debug.Log($"subtracting: meshes not empty");

            var originalPoly = new IndexedPolyVec2D(original);
            if (originalPoly._slivers.Length != 1)
                throw new Exception("Currently can subtract only from single piece MeshFragment");
            if (originalPoly._slivers[0]._holes!=null && originalPoly._slivers[0]._holes.Length != 0)
                throw new Exception("Cannot subtract from MeshFragment with holes");

            var subtrahendPoly = new IndexedPolyVec2D(subtrahend);
            if (subtrahendPoly._slivers.Length != 1)
                throw new Exception("Currently can subtract only by single piece MeshFragment");
            if (subtrahendPoly._slivers[0]._holes!=null && subtrahendPoly._slivers[0]._holes.Length != 0)
                throw new Exception("Cannot subtract by MeshFragment with holes");

            //Debug.Log("initial subtraction checks ok, should be fine");

            bool overlaps = IndexedPolyVec2D.Subtracter.CutOutIfOverlapsPoly(
                originalPoly._slivers[0]._poly.CornersVec2D(originalPoly._vertices),
                subtrahendPoly._slivers[0]._poly.CornersVec2D(subtrahendPoly._vertices),
                out Vec2D[] cutOutPolyVertices, out PoolListEdgeIndexed singleEdges);
            using (singleEdges)
                if (overlaps)
                {
                    if (cutOutPolyVertices.Length == 0)
                        return MeshFragmentVec2D.Empty;

                    //Debug.Log($"overlaps, single edges count: {singleEdges.Count}");
                    IndexedPolyVec2D indexedPoly = IndexedPolyVec2D.CreateIndexedPolyVec2D(cutOutPolyVertices, IndexedEdgePoly.
                        DebugIndexedEdgePolyFromSingleEdges(singleEdges.Span, cutOutPolyVertices));
                    List<IndexedTri> triangulated = indexedPoly.Triangulate();

                    if (triangulated.Count == 0)
                        return MeshFragmentVec2D.Empty;

                    return new MeshFragmentVec2D(cutOutPolyVertices, triangulated.ToTriangles());
                }

            return original;
        }
    }


}
