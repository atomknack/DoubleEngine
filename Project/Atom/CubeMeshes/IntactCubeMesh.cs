#nullable enable

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DoubleEngine.Atom
{
    [JsonObject(MemberSerialization.OptIn)]
    public record IntactCubeMesh : ISerializableCubeMesh
    {
        [DisallowNull]
        internal readonly MeshFragmentVec3D mesh;
        [DisallowNull]
        internal readonly int[] flatNodeIDs;
        [DisallowNull]
        internal readonly FlatNodeTransform[] flatNodeTransforms;

        [JsonProperty(Required = Required.Always, PropertyName = "Mesh")]
        MeshFragmentVec3D ISerializableCubeMesh.Mesh => mesh;

        [JsonProperty(Required = Required.Always, PropertyName = "SideIds")]
        int[] ISerializableCubeMesh.SideIds => flatNodeIDs;

        [JsonProperty(Required = Required.Always, PropertyName = "SideTransforms")]
        FlatNodeTransform[] ISerializableCubeMesh.SideTransforms => flatNodeTransforms;

        [JsonConstructor]
        private IntactCubeMesh(
            [JsonProperty("Mesh")] MeshFragmentVec3D mesh,
            [JsonProperty("SideIds")] int[] sidesFlatNodeId,
            [JsonProperty("SideTransforms")] FlatNodeTransform[] sidesFlatNodeTransform)
        {
            if (sidesFlatNodeId.Length != 6)
                throw new ArgumentException($"Cube node should have 6 flat sides, not {sidesFlatNodeId.Length}");
            if (sidesFlatNodeTransform.Length != 6)
                throw new ArgumentException($"Cube node should have 6 flan transforms, not {sidesFlatNodeTransform.Length}");
            this.mesh = mesh;
            this.flatNodeIDs = sidesFlatNodeId;
            this.flatNodeTransforms = sidesFlatNodeTransform;
        }

        public (int flatNodeId, FlatNodeTransform flatNodeTransform) GetSide(CubeSides.Enum side) =>
            (flatNodeIDs[(int)side], flatNodeTransforms[(int)side]);


        public static bool TryCreate(MeshFragmentVec3D mesh, [MaybeNull] out IntactCubeMesh result)
        {
            FlatNode[] sides = new FlatNode[6];
            foreach (CubeSides.Enum side in CubeSides.allSides)
            {
                if (FlatNodes.TrySlowlyFindWithSameVertexPositions(
                    Grid6Sides.MeshFragmentVec3DFromSide(mesh, side).To2D(), out FlatNode foundSide) )
                    {
                    sides[(int)side] = foundSide;
                    }
                else
                {
                    var debugMesh = Grid6Sides.MeshFragmentVec3DFromSide(mesh, side);
                    /*
                    Debug.Log($"{debugMesh.Vertices.Length} {debugMesh.Faces.Length} {debugMesh.Triangles.Length}");
                    Debug.Log($"{debugMesh.vertices} {debugMesh.triangles} {debugMesh._joinedVertices}");
                    Debug.Log(Grid6Sides.MeshFragmentVec3DFromSide(mesh, side) == MeshFragmentVec3D.Empty);
                    Debug.Log(Grid6Sides.MeshFragmentVec3DFromSide(mesh, side).To2D() == MeshFragmentVec2D.Empty);
                    Debug.Log(Grid6Sides.MeshFragmentVec3DFromSide(mesh, side));
                    Debug.Log(Grid6Sides.MeshFragmentVec3DFromSide(mesh, side).To2D());
                    Debug.Log(foundSide);
                    */
                    result = null;
                    return false;
                }
            }
            result = Create (mesh, sides);
            return true;

        }

        public static IntactCubeMesh Create(
            MeshFragmentVec3D mesh,
            FlatNode zNegative, FlatNode xNegative, FlatNode yNegative,
            FlatNode zPositive, FlatNode xPositive, FlatNode yPositive) =>
            Create(mesh, new FlatNode[6] { zNegative, xNegative, yNegative, zPositive, xPositive, yPositive });

        public static IntactCubeMesh Create(MeshFragmentVec3D mesh, ReadOnlySpan<FlatNode> sides)
        {
            if (sides.Length != 6)
                throw new ArgumentException($"Cube node should have 6 sides, not {sides.Length}");
            int[] flatNodeIDs = new int[6];
            FlatNodeTransform[] flatNodeTransforms = new FlatNodeTransform[6];
            foreach(CubeSides.Enum side in CubeSides.allSides)
            {
                int sideIndex = (int)side;
                flatNodeIDs[sideIndex] = sides[sideIndex].id;
                flatNodeTransforms[sideIndex] = sides[sideIndex].flatTransform;
            }
            return new IntactCubeMesh(mesh, flatNodeIDs, flatNodeTransforms);
        }
    }
}
