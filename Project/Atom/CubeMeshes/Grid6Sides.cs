using CollectionLike.Enumerables;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DoubleEngine.Atom
{
    [System.Serializable]
    public record Grid6Sides
    {
        public readonly GridSide zNegative;
        public readonly GridSide xNegative;
        public readonly GridSide yNegative;
        public readonly GridSide zPositive;
        public readonly GridSide xPositive;
        public readonly GridSide yPositive;



        //not tested, need testing
        public static MeshFragmentVec3D DeshelledMeshFragmentVec3D(MeshFragmentVec3D mesh)
        {
            List<int> unshelledFaces = CoreMeshFaces(mesh);

            if (unshelledFaces.Count == 0)
                return MeshFragmentVec3D.Empty;
            return MeshFragmentVec3D.CreateMeshFragmentAsIsWithoutArraysCopying(
                MeshUtil.RemoveUnusedVerticesAndFacesAndReturnNewArraysTriangles(mesh.Vertices, mesh.Faces, unshelledFaces));
        }
        //not tested, need testing
        private static List<int> CoreMeshFaces(MeshFragmentVec3D mesh)
        {
            List<int> allShellFaces = new List<int>();
            foreach (var selector in CubeSides.verticeSelectors)
                allShellFaces.AddRange(mesh.SelectFacesWhereAllVerticesIs(selector));
            List<int> unshelledFaces = allShellFaces.InvertedIndexesSelection(mesh.Faces.Length);
            return unshelledFaces;
        }

        public static MeshFragmentVec3D MeshFragmentVec3DFromSide(MeshFragmentVec3D mesh, CubeSides.Enum side) =>
            mesh.MakeFragmentWhereAllVerticesIs(CubeSides.verticeSelectors[(int)side]).Rotated(CubeSides.ToZNeg[(int)side]);
        /*
        public static int[] GridElementSides(Grid6Sides ge) //need testing
        {
            int[] fragmentIndexes = new int[6];
            fragmentIndexes[0] = ge.zNegative.sideIndex;
            fragmentIndexes[1] = ge.xNegative.sideIndex;
            fragmentIndexes[2] = ge.yNegative.sideIndex;
            fragmentIndexes[3] = ge.zPositive.sideIndex;
            fragmentIndexes[4] = ge.xPositive.sideIndex;
            fragmentIndexes[5] = ge.yPositive.sideIndex;
            return fragmentIndexes;
        }*/


        public static Grid6Sides Default => new (
            new GridSide(0, FlatNodeTransform.Default),
            new GridSide(1, FlatNodeTransform.Default),
            new GridSide(2, FlatNodeTransform.Default),
            new GridSide(3, FlatNodeTransform.Default),
            new GridSide(4, FlatNodeTransform.Default),
            new GridSide(5, FlatNodeTransform.Default)
            );

        public Grid6Sides(
            GridSide zNegative, 
            GridSide xNegative, 
            GridSide yNegative, 

            GridSide zPositive, 
            GridSide xPositive, 
            GridSide yPositive
            )
        {
            this.zNegative = zNegative;
            this.xNegative = xNegative;
            this.yNegative = yNegative;

            this.zPositive = zPositive;
            this.xPositive = xPositive;
            this.yPositive = yPositive; 
                
        }



        public Grid6Sides RotateY90()
        {
            return new Grid6Sides(

                xPositive,
                zNegative,
                yNegative.Rotate(PerpendicularAngle.a90), //TODO: Check, Test

                xNegative,
                zPositive,
                yPositive.Rotate(PerpendicularAngle.aNegative90) //TODO: Check, Test
                );

        }

        public Grid6Sides RotateX90() 
        {
            return new Grid6Sides(

                yNegative,
                xNegative.Rotate(PerpendicularAngle.a90),
                zPositive.InvertY().InvertX(), //TODO: Check, Test

                yPositive.InvertY().InvertX(),
                xPositive.Rotate(PerpendicularAngle.aNegative90),
                zNegative //TODO: Check, Test
                );
        }

        public Grid6Sides RotateZ90() 
        {
            return new Grid6Sides(
                zNegative.Rotate(PerpendicularAngle.a90),
                yPositive.Rotate(PerpendicularAngle.a90),
                xNegative.Rotate(PerpendicularAngle.a90),

                zPositive.Rotate(PerpendicularAngle.aNegative90),
                yNegative.Rotate(PerpendicularAngle.a90),
                xPositive.Rotate(PerpendicularAngle.a90)
                );
        }
        public Grid6Sides InvertY()
        {
            return new Grid6Sides(
                zNegative.InvertY(),
                xNegative.InvertY(),
                yPositive.InvertY(), // ypositive is yNegative that InvertedY
                
                zPositive.InvertY(),
                xPositive.InvertY(),
                yNegative.InvertY()); // ynegative is yPositive that InvertedY
        }
        public Grid6Sides InvertX()
        {
            return new Grid6Sides(
                zNegative.InvertX(),
                xPositive.InvertX(), // xnegative is xPositive that InvertedX
                yNegative.InvertX(),

                zPositive.InvertX(),
                xNegative.InvertX(), // xpositive is xNegative that InvertedX
                yPositive.InvertX());
        }
        public Grid6Sides InvertZ()
        {
            return new Grid6Sides(
                zPositive.InvertX(), // znegative is zPositive that InvertedX
                xNegative.InvertX(),
                yNegative.InvertY(), //inversion by z inverts bottom by Y

                zNegative.InvertX(), // zpositive is zNegative that InvertedX
                xPositive.InvertX(),
                yPositive.InvertY());  //inversion by z inverts bottom by Y
        }

    }




}

