using CollectionLike.Enumerables;
using CollectionLike.Pooled;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DoubleEngine.Atom
{


public sealed partial class Grid6SidesCached
{
        public static readonly QuaternionD[] rotationsToZNeg = CubeSides.ToZNeg;
        public static readonly QuaternionD[] rotationsFromZNeg = CubeSides.FromZNeg;
        public static Grid6SidesCached Default => allPositions[0];

        private static readonly List<Grid6SidesCached> allPositions;
        private static readonly HashSet<Grid6Sides> allPositionsSet;

        private static readonly Grid6SidesCached[] allRotationsInversionsTable; //probably ok, need more testing
        //[Obsolete("probably ok, need more testing. Use direct methods from every object like InvertX, RotateY etc.")]
        public static Grid6SidesCached FromRotationAndScale(ScaleInversionPerpendicularRotation3 scaleRotation) =>
            allRotationsInversionsTable[scaleRotation.index]; //need testing


        static Grid6SidesCached()
        {
            allPositions = new();
            allPositionsSet = new();
            allPositions.Add(ComputeFromRotationInversion(PerpendicularRotation3.PerpendicularRotationFrom4PositionInt_NoNeedToCheck(0, 0, 0), false));
            ComputeInitialRotations();

            //allRotationsInversionsTable = new Grid6SidesCached[ScaleInversionPerpendicularRotation3.maxValuePlusOne];
            allRotationsInversionsTable = ComputeRotationsScaleTableFrom_InitialRotations();


            // not tested, need testing
            CalcTable_s_transformedByID();
        }



        private readonly Grid6Sides _toCache;
        public readonly ScaleInversionPerpendicularRotation3 _orientation;
        internal readonly byte _allPositionsIndex;
        public byte OrientationIndex() => _allPositionsIndex;

        public readonly GridSide zNegative;
        public readonly GridSide xNegative;
        public readonly GridSide yNegative;
        public readonly GridSide zPositive;
        public readonly GridSide xPositive;
        public readonly GridSide yPositive;


        public bool _invertedY = false;
        public QuaternionD _rotation;
        public Vec3D Scale => _invertedY ? new Vec3D(1d, -1d, 1d) : Vec3D.one;
        public static Grid6SidesCached GetCached(byte index) => allPositions[index];

        //can deal without Lazy<T> - thread safety not important, because we use cached copy //readmore about it at: https://killalldefects.com/2020/01/15/getting-lazy-in-c/
        private Grid6SidesCached _invertX_reference;
        public Grid6SidesCached InvertX() => _invertX_reference ??= GetCached(_toCache.InvertX());

        private Grid6SidesCached _invertY_reference;
        public Grid6SidesCached InvertY() => _invertY_reference ??= GetCached(_toCache.InvertY());

        private Grid6SidesCached _invertZ_reference;
        public Grid6SidesCached InvertZ() => _invertZ_reference ??= GetCached(_toCache.InvertZ());

        private Grid6SidesCached _rotateX90_reference;
        public Grid6SidesCached RotateX90() => _rotateX90_reference ??= GetCached(_toCache.RotateX90());

        private Grid6SidesCached _rotateY90_reference;
        public Grid6SidesCached RotateY90() => _rotateY90_reference ??= GetCached(_toCache.RotateY90());

        private Grid6SidesCached _rotateZ90_reference;
        public Grid6SidesCached RotateZ90() => _rotateZ90_reference ??= GetCached(_toCache.RotateZ90());

        private Grid6SidesCached(ScaleInversionPerpendicularRotation3 id, byte allPositionsIndex, Grid6Sides toCache, bool invertedY, QuaternionD rotation)
        {
            _orientation = id;
            _allPositionsIndex = allPositionsIndex;
            _toCache = toCache;
            _invertedY = invertedY;
            _rotation = rotation;

            zNegative = _toCache.zNegative;
            xNegative = _toCache.xNegative;
            yNegative = _toCache.yNegative;
            zPositive = _toCache.zPositive;
            xPositive = _toCache.xPositive;
            yPositive = _toCache.yPositive;

            _invertX_reference = null;
            _invertY_reference = null;
            _invertZ_reference = null;
            _rotateX90_reference = null;
            _rotateY90_reference = null;
            _rotateZ90_reference = null;

    }


        private static Grid6SidesCached GetCached(Grid6Sides find)
        {
            for (int i = 0; i < allPositions.Count; i++)
            {
                if (allPositions[i]._toCache == find)
                    return allPositions[i];
            }
            throw new ArgumentException("not found, initial input should be changed");
        }

        private static Grid6SidesCached FindOrAdd(Grid6Sides sides, PerpendicularRotation3 perpRotation, bool inverted, QuaternionD quatRotation)
        {
            if (allPositionsSet.Contains(sides))
            {
                /*
                //not important, here just for better Quaternion undestanding
                Quaternion cachedRotation = GetCached(sides)._rotation;
                if (cachedRotation != rotation)
                {
                    if (cachedRotation.IsEqualOrFlippedEqual(rotation))
                        Debug.Log($"quaternions not equal, BUT FLIPPED Equality is FINE, unity just have only wrong quaternion equality");
                    else 
                        Debug.LogError($" SOMETHING WEry WronG , flipping equality not helping, cached quaternion not equal to new {cachedRotation} {rotation}");
                }
                else
                    Debug.Log("cached Quaternion is fine as is");
                // 
                */

                return GetCached(sides);
            }

            allPositionsSet.Add(sides);
            ScaleInversionPerpendicularRotation3 orientation = new ScaleInversionPerpendicularRotation3(new ScaleInversionV3(false, inverted, false), perpRotation);//allPositions.Count;
            byte newItemIndex = (byte)allPositions.Count;
            allPositions.Add(new Grid6SidesCached(orientation, newItemIndex, sides, inverted, quatRotation));
            return allPositions.Last();//[newid];
        }

        private static Grid6SidesCached[] ComputeRotationsScaleTableFrom_InitialRotations()
        {
            Grid6SidesCached[] result = new Grid6SidesCached[ScaleInversionPerpendicularRotation3.maxValuePlusOne];
            bool[] invertAxis = { false, true };
            int[] rot4 = { 0, 1, 2, 3 };

            foreach (var invertX in invertAxis)
                foreach (var invertY in invertAxis)
                    foreach (var invertZ in invertAxis)
                        foreach (var rotX in rot4)
                    foreach (var rotZ in rot4)
                        foreach (var rotY in rot4)
                        {
                                    var rotation = PerpendicularRotation3.PerpendicularRotationFrom4PositionInt_NoNeedToCheck(rotX, rotY, rotZ);
                                    var scaleRotation = new ScaleInversionPerpendicularRotation3(new ScaleInversionV3(invertX,invertY,invertZ),rotation);

                                    result[scaleRotation.index] = 
                                        ComputeFromRotationInversionXYZAndThenReturnExistingCached(rotation, invertX,invertY,invertZ);
                        }
            return result;
        }

        private static void ComputeInitialRotations()
        {
            bool[] invertAxis = { false, true };
            int[] rot4 = { 0, 1, 2, 3 };

            foreach (var invertY in invertAxis)
                foreach (var rotX in rot4)
                    foreach (var rotZ in rot4)
                        foreach (var rotY in rot4)
                        {
                            ComputeFromRotationInversion(PerpendicularRotation3.PerpendicularRotationFrom4PositionInt_NoNeedToCheck(rotX, rotY, rotZ), invertY);
                        }
        }

        private static Grid6SidesCached ComputeFromRotationInversion(PerpendicularRotation3 rotation, bool yInversion)
        {
            Grid6Sides tge = Grid6Sides.Default;

            QuaternionD q = QuaternionD.identity;

            if (yInversion)
                tge = tge.InvertY();

            int tRotX = rotation._rotationX4Position;
            while (tRotX > 0)
            {
                tge = tge.RotateX90();
                q = QuaternionD.Euler(90f, 0, 0) * q;
                tRotX--;
            }

            int tRotY = rotation._rotationY4Position;
            while (tRotY > 0)
            {
                tge = tge.RotateY90();
                q = QuaternionD.Euler(0, 90f, 0) * q;
                tRotY--;
            }

            int tRotZ = rotation._rotationZ4Position;
            while (tRotZ > 0)
            {
                tge = tge.RotateZ90();
                q = QuaternionD.Euler(0, 0, 90f) * q;
                tRotZ--;
            }
            return FindOrAdd(tge, rotation, yInversion, q);
        }


        private static Grid6SidesCached ComputeFromRotationInversionXYZAndThenReturnExistingCached(PerpendicularRotation3 rotation, bool invertX, bool invertY, bool invertZ)
        {
            Grid6Sides tge = Grid6Sides.Default;

            if (invertX)
                tge = tge.InvertX();
            if (invertY)
                tge = tge.InvertY();
            if (invertZ)
                tge = tge.InvertZ();

            int tRotX = rotation._rotationX4Position;
            while (tRotX > 0)
            {
                tge = tge.RotateX90();
                tRotX--;
            }

            int tRotY = rotation._rotationY4Position;
            while (tRotY > 0)
            {
                tge = tge.RotateY90();
                tRotY--;
            }

            int tRotZ = rotation._rotationZ4Position;
            while (tRotZ > 0)
            {
                tge = tge.RotateZ90();
                tRotZ--;
            }
            return GetCached(tge);
        }

    }

}