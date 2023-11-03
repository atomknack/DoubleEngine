using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    internal static class FlatNodesComplimentaryNotOptimizedMatcher
    {
        private static Dictionary<
            (int id,FlatNodeTransform transform,int complimentaryId,FlatNodeTransform complimentaryTransform), MeshFragmentVec3D> 
                matcherDict = new();

        internal static MeshFragmentVec3D GetFlatnodeComplimentaryAs3D(
            int id,
            FlatNodeTransform transform,
            int complimentaryId,
            FlatNodeTransform complimentaryTransform)
        {
            if (id == 0)
                return MeshFragmentVec3D.Empty;
            if (complimentaryId == 1)
                return MeshFragmentVec3D.Empty;
            if (matcherDict.TryGetValue((id, transform, complimentaryId, complimentaryTransform), out var mesh))
                return mesh;//? node : throw new KeyNotFoundException();)

            var basemesh2D = FlatNodes.GetFlatNode(id, transform).Transformed2D;
            var complimentary2D = FlatNodes.GetFlatNode(complimentaryId, complimentaryTransform).Transformed2D;
            var result2D = MeshFragmentVec2D.Subtract(basemesh2D, complimentary2D);
            var result3D = FlatNode.To3D(result2D);
            matcherDict.Add((id, transform, complimentaryId, complimentaryTransform), result3D);
            return result3D;
        }

    }
}
