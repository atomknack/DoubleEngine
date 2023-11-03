using System;

namespace DoubleEngine.Atom
{
    public static partial class CubeSides
{
        //not tested
        public enum Enum : int
        {
            zNegative = 0,
            xNegative = 1,
            yNegative = 2,
            zPositive = 3,
            xPositive = 4,
            yPositive = 5
        }//

        public static readonly CubeSides.Enum[] allSides = new CubeSides.Enum[] {
            Enum.zNegative,
            Enum.xNegative,
            Enum.yNegative,
            Enum.zPositive,
            Enum.xPositive,
            Enum.yPositive};

        public static readonly QuaternionD[] ToZNeg =
        {
            AngleMethods.ZNegToZNeg,
            AngleMethods.XNegToZNeg,
            AngleMethods.YNegToZNeg,
            AngleMethods.ZPosToZNeg,
            AngleMethods.XPosToZNeg,
            AngleMethods.YPosToZNeg
        };

        public static readonly QuaternionD[] FromZNeg =
        {
            AngleMethods.ZNegToZNeg,
            AngleMethods.ZNegToXNeg,
            AngleMethods.ZNegToYNeg,
            AngleMethods.ZNegToZPos,
            AngleMethods.ZNegToXPos,
            AngleMethods.ZNegToYPos
        };

        /*
        public static readonly QuaternionD[] rotationsToZNeg = {
            AngleMethods.ZNegToZNeg,
            AngleMethods.XNegToZNeg,
            AngleMethods.YNegToZNeg,
            AngleMethods.ZPosToZNeg,
            AngleMethods.XPosToZNeg,
            AngleMethods.YPosToZNeg, };

        public static readonly QuaternionD[] rotationsFromZNeg = {
            AngleMethods.ZNegToZNeg,
            AngleMethods.ZNegToXNeg,
            AngleMethods.ZNegToYNeg,
            AngleMethods.ZNegToZPos,
            AngleMethods.ZNegToXPos,
            AngleMethods.ZNegToYPos, };
        */
        //todo decrease border size to 0.00001d

        //private const double maxDimension = 0.5d;
        //private const double borderEpsilon = 0.002;
        //private const double dimensionWithoutBorder = maxDimension - borderEpsilon; //should be 0.498d
        //not tested
        public static readonly Func<Vec3D, bool>[] verticeSelectors =
        {
            v => v.z <= -0.498d,
            v => v.x <= -0.498d,
            v => v.y <= -0.498d,
            v => v.z >= 0.498d,
            v => v.x >= 0.498d,
            v => v.y >= 0.498d,
        };//

    }

}