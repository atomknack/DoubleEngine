#nullable enable

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DoubleEngine.Atom
{
    [JsonObject(MemberSerialization.OptIn)]
    public record DeshelledCubeMesh: ISerializableCubeMesh
    {
        [DisallowNull]
        internal readonly MeshFragmentVec3D core;
        [DisallowNull]
        internal readonly FlatNode[] sides;

        public FlatNode ZNegativeFlatNode(Grid6SidesCached orientation) => 
            FlatNodes.GetFlatNode(sides[orientation.zNegative.sideIndex], orientation.zNegative); //sides[ge.zNegative.sideIndex]?.TransformedByFlatNodeTransform(ge.zNegative.flatTransform);
        public FlatNode XNegativeFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNode(sides[orientation.xNegative.sideIndex], orientation.xNegative);
        public FlatNode YNegativeFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNode(sides[orientation.yNegative.sideIndex], orientation.yNegative);
        public FlatNode ZPositiveFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNode(sides[orientation.zPositive.sideIndex], orientation.zPositive);
        public FlatNode XPositiveFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNode(sides[orientation.xPositive.sideIndex], orientation.xPositive);
        public FlatNode YPositiveFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNode(sides[orientation.yPositive.sideIndex], orientation.yPositive);

        //public FlatNode ZNegativeComplementFlatNode => ZNegativeFlatNode?.InvertX();
        public FlatNode ZNegativeComplementFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNodeInvertedX(sides[orientation.zNegative.sideIndex], orientation.zNegative);

        //public FlatNode XNegativeComplementFlatNode => XNegativeFlatNode?.InvertX();
        public FlatNode XNegativeComplementFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNodeInvertedX(sides[orientation.xNegative.sideIndex], orientation.xNegative);

        //public FlatNode YNegativeComplementFlatNode => YNegativeFlatNode?.InvertY();
        public FlatNode YNegativeComplementFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNodeInvertedY(sides[orientation.yNegative.sideIndex], orientation.yNegative);

        //public FlatNode ZPositiveComplementFlatNode => ZPositiveFlatNode?.InvertX();
        public FlatNode ZPositiveComplementFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNodeInvertedX(sides[orientation.zPositive.sideIndex], orientation.zPositive);

        //public FlatNode XPositiveComplementFlatNode => XPositiveFlatNode?.InvertX();
        public FlatNode XPositiveComplementFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNodeInvertedX(sides[orientation.xPositive.sideIndex], orientation.xPositive);

        //public FlatNode YPositiveComplementFlatNode => YPositiveFlatNode?.InvertY();
        public FlatNode YPositiveComplementFlatNode(Grid6SidesCached orientation) =>
            FlatNodes.GetFlatNodeInvertedY(sides[orientation.yPositive.sideIndex], orientation.yPositive);

        [JsonProperty(Required = Required.Always, PropertyName = "Mesh")]
        MeshFragmentVec3D ISerializableCubeMesh.Mesh => core;

        [JsonProperty(Required = Required.Always, PropertyName = "SideIds")]
        int[] ISerializableCubeMesh.SideIds => sides.Select(x=>x.id).ToArray();

        [JsonProperty(Required = Required.Always, PropertyName = "SideTransforms")]
        FlatNodeTransform[] ISerializableCubeMesh.SideTransforms => sides.Select(x=>x.flatTransform).ToArray();

        public static DeshelledCubeMesh Create(IntactCubeMesh intact)
        {
            MeshFragmentVec3D core = Grid6Sides.DeshelledMeshFragmentVec3D(intact.mesh);
            FlatNode[] sides = new FlatNode[CubeSides.allSides.Length];
            foreach (CubeSides.Enum side in CubeSides.allSides)
            {
                sides[(int)side] = FlatNodes.GetFlatNode(intact.GetSide(side));
            }
            return new DeshelledCubeMesh(core, sides);
        }

        public MeshFragmentVec3D Reshelled_ForDebug()
        {
            using MeshBuilderVec3D builder = new MeshBuilderVec3D(core);
            foreach (CubeSides.Enum side in CubeSides.allSides)
            {
                builder.AddMeshFragment(sides[(int)side].Transformed.Rotated(CubeSides.FromZNeg[(int)side]));
            }
            return builder.BuildFragment();
        }

        [JsonConstructor]
        private DeshelledCubeMesh(
            [JsonProperty("Mesh")] MeshFragmentVec3D core,
            [JsonProperty("SideIds")] int[] sidesFlatNodeId,
            [JsonProperty("SideTransforms")] FlatNodeTransform[] sidesFlatNodeTransform) : 
                this(core, JoinToFlatNodeArray(sidesFlatNodeId, sidesFlatNodeTransform))
            { }

        private static FlatNode[] JoinToFlatNodeArray(int[] sidesFlatNodeId, FlatNodeTransform[] sidesFlatNodeTransform)
        {
            if (sidesFlatNodeId.Length != 6)
                throw new ArgumentException($"Cube node should have 6 flat sides, not {sidesFlatNodeId.Length}");
            if (sidesFlatNodeTransform.Length != 6)
                throw new ArgumentException($"Cube node should have 6 flan transforms, not {sidesFlatNodeTransform.Length}");
            FlatNode[] sides = new FlatNode[6];
            for (int i = 0; i < 6; ++i)
            {
                sides[i] = FlatNodes.GetFlatNode(sidesFlatNodeId[i], sidesFlatNodeTransform[i]);
            }

            return sides;
        }

        private DeshelledCubeMesh(MeshFragmentVec3D core, FlatNode[] sides)
        {
            if (sides.Length != 6)
                throw new ArgumentException($"Cube node should have 6 FlatNode sides, not {sides.Length}");
            this.core = core;
            this.sides = sides;
        }
    }
}
