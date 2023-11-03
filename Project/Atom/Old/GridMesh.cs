using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace DoubleEngine.Atom
{
    public interface IGridMesh { void InvertX(); void InvertY(); void InvertZ(); void RotateX90(); void RotateY90(); void RotateZ90(); }

    [JsonObject(MemberSerialization.OptIn)]
    public class GridMesh : IGridMesh
    {
        public Grid6SidesCached ge;
        [JsonProperty("sixSides", Required = Required.Always)] private readonly FlatNode[] sides;
        [JsonProperty("faceRemove", Required = Required.Always)] private readonly int[][] _facesToRemove;
        [JsonProperty("fragment", Required = Required.Always)] private readonly MeshFragmentVec3D baseMeshFragment;
        //private readonly Vector3[][] _sideAdditionalSplitters_ToChangeFlatNode;
        //private readonly MeshFragment baseMeshFragmentInvertedY;


        [Obsolete("only for debug and export")]
        public MeshFragmentVec3D BaseMeshFragment() => baseMeshFragment;


        public void InvertX() => ge = ge.InvertX(); 
        public void InvertY() => ge = ge.InvertY(); 
        public void InvertZ() => ge = ge.InvertZ(); 
        public void RotateX90() => ge = ge.RotateX90();
        public void RotateY90() => ge = ge.RotateY90();
        public void RotateZ90() => ge = ge.RotateZ90();

        public MeshFragmentVec3D GetCoreMesh(
            bool removeZneg, bool removeXneg, bool removeYneg, 
            bool removeZpos, bool removeXpos, bool removeYpos)
        {
            //MeshFragment temp;
            /*if (ge._invertedY)
                temp = baseMeshFragmentInvertedY;w
            else
                temp = baseMeshFragment;
                //return( baseMeshFragmentInvertedY, ge._rotation);*/
            using MeshBuilderVec3D builder = new MeshBuilderVec3D(baseMeshFragment);
            builder.AddRotation(ge._rotation);
            builder.AddScale(new Vec3D(1f, ge._invertedY ? -1f : 1f, 1f));
            if (removeZneg) builder.RemoveFaces(_facesToRemove[ge.zNegative.sideIndex]);
            if (removeXneg) builder.RemoveFaces(_facesToRemove[ge.xNegative.sideIndex]);
            if (removeYneg) builder.RemoveFaces(_facesToRemove[ge.yNegative.sideIndex]);
            if (removeZpos) builder.RemoveFaces(_facesToRemove[ge.zPositive.sideIndex]);
            if (removeXpos) builder.RemoveFaces(_facesToRemove[ge.xPositive.sideIndex]);
            if (removeYpos) builder.RemoveFaces(_facesToRemove[ge.yPositive.sideIndex]);
            return builder.BuildFragment();

        }
        [JsonConstructor]
        public GridMesh(MeshFragmentVec3D fragment, FlatNode[] sixSides, int[][] faceRemove)
        {
            ge = Grid6SidesCached.Default;
            sides = new FlatNode[6];
            for (int i = 0; i < sides.Length; i++)
                sides[i] = sixSides[i];
            //if (faceRemove!=null)
            //{
            _facesToRemove = new int[6][];
            for (int i = 0; i < faceRemove.Length;i++)
                _facesToRemove[i] = (int[])faceRemove[i].Clone();

            //}
            baseMeshFragment = fragment;
            //baseMeshFragmentInvertedY = baseMeshFragment.Scaled(new Vector3(1, -1, 1));
            
        }

        public FlatNode ZNegativeFlatNode => sides[ge.zNegative.sideIndex]?.TransformedByFlatNodeTransform(ge.zNegative.flatTransform);
        public FlatNode XNegativeFlatNode => sides[ge.xNegative.sideIndex]?.TransformedByFlatNodeTransform(ge.xNegative.flatTransform);
        public FlatNode YNegativeFlatNode => sides[ge.yNegative.sideIndex]?.TransformedByFlatNodeTransform(ge.yNegative.flatTransform);
        public FlatNode ZPositiveFlatNode => sides[ge.zPositive.sideIndex]?.TransformedByFlatNodeTransform(ge.zPositive.flatTransform);
        public FlatNode XPositiveFlatNode => sides[ge.xPositive.sideIndex]?.TransformedByFlatNodeTransform(ge.xPositive.flatTransform);
        public FlatNode YPositiveFlatNode => sides[ge.yPositive.sideIndex]?.TransformedByFlatNodeTransform(ge.yPositive.flatTransform);

        public MeshFragmentVec3D ZNegativeFragment => 
            ZNegativeFlatNode?.Transformed.Rotated(AngleMethods.ZNegToZNeg); //ZNegativeFlatNode?.Transformed.ToMeshFragmentVector3().Rotated(AngleMethods.ZNegToZNeg);
        public MeshFragmentVec3D XNegativeFragment => 
            XNegativeFlatNode?.Transformed.Rotated(AngleMethods.ZNegToXNeg);
        public MeshFragmentVec3D YNegativeFragment => 
            YNegativeFlatNode?.Transformed.Rotated(AngleMethods.ZNegToYNeg);
        public MeshFragmentVec3D ZPositiveFragment => 
            ZPositiveFlatNode?.Transformed.Rotated(AngleMethods.ZNegToZPos);
        public MeshFragmentVec3D XPositiveFragment => 
            XPositiveFlatNode?.Transformed.Rotated(AngleMethods.ZNegToXPos);
        public MeshFragmentVec3D YPositiveFragment => 
            YPositiveFlatNode?.Transformed.Rotated(AngleMethods.ZNegToYPos);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //for testing of behaviout with other objects
        public FlatNode ZNegativeComplementFlatNode => ZNegativeFlatNode?.InvertX();
        public FlatNode XNegativeComplementFlatNode => XNegativeFlatNode?.InvertX();
        public FlatNode YNegativeComplementFlatNode => YNegativeFlatNode?.InvertY();
        public FlatNode ZPositiveComplementFlatNode => ZPositiveFlatNode?.InvertX();
        public FlatNode XPositiveComplementFlatNode => XPositiveFlatNode?.InvertX();
        public FlatNode YPositiveComplementFlatNode => YPositiveFlatNode?.InvertY();

        public MeshFragmentVec3D ZNegativeComplementFragment => 
            ZNegativeComplementFlatNode?.Transformed.Rotated(AngleMethods.ZNegToZPos).Translated(new Vec3D(0,0,-1.1f));//            ZNegativeComplementFlatNode?.Transformed.ToMeshFragmentVector3().Rotated(AngleMethods.ZNegToZPos).Translated(new Vector3(0,0,-1.1f));
        public MeshFragmentVec3D XNegativeComplementFragment => 
            XNegativeComplementFlatNode?.Transformed.Rotated(AngleMethods.ZNegToXPos).Translated(new Vec3D(-1.1f, 0, 0));
        public MeshFragmentVec3D YNegativeComplementFragment => 
            YNegativeComplementFlatNode?.Transformed.Rotated(AngleMethods.ZNegToYPos).Translated(new Vec3D(0, -1.1f, 0));
        public MeshFragmentVec3D ZPositiveComplementFragment => 
            ZPositiveComplementFlatNode?.Transformed.Rotated(AngleMethods.ZNegToZNeg).Translated(new Vec3D(0, 0, 1.1f));
        public MeshFragmentVec3D XPositiveComplementFragment => 
            XPositiveComplementFlatNode?.Transformed.Rotated(AngleMethods.ZNegToXNeg).Translated(new Vec3D(1.1f, 0, 0));
        public MeshFragmentVec3D YPositiveComplementFragment => 
            YPositiveComplementFlatNode?.Transformed.Rotated(AngleMethods.ZNegToYNeg).Translated(new Vec3D(0, 1.1f, 0));


        public static MeshFragmentVec3D BuildMeshFragmentFromSides(MeshFragmentVec3D[] sides)
        {
            using MeshBuilderVec3D builder = new();
            for (int i = 0; i < sides.Length; i++)
                if (sides[i] != null)
                    builder.AddMeshFragment(sides[i]);
            return builder.BuildFragment();
        }

        public MeshFragmentVec3D[] GridElementSides()
        {
            MeshFragmentVec3D[] fragments = new MeshFragmentVec3D[6];
            fragments[0] = ZNegativeFragment;
            fragments[1] = XNegativeFragment;
            fragments[2] = YNegativeFragment;
            fragments[3] = ZPositiveFragment;
            fragments[4] = XPositiveFragment;
            fragments[5] = YPositiveFragment;
            return fragments;
        }

        public MeshFragmentVec3D[] GridElementComplementSides()
        {
            MeshFragmentVec3D[] fragments = new MeshFragmentVec3D[6];
            fragments[0] = ZNegativeComplementFragment;
            fragments[1] = XNegativeComplementFragment;
            fragments[2] = YNegativeComplementFragment;
            fragments[3] = ZPositiveComplementFragment;
            fragments[4] = XPositiveComplementFragment;
            fragments[5] = YPositiveComplementFragment;
            return fragments;
        }
    }

}