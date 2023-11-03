using DoubleEngine.Guard;

namespace DoubleEngine.Atom
{
    public readonly struct PerpendicularRotation3
    {
        internal readonly int _rotationX4Position;
        internal readonly int _rotationY4Position;
        internal readonly int _rotationZ4Position;

        public Vec3I ToAngleDegreesVec3I() => new Vec3I( _rotationX4Position * 90, _rotationY4Position * 90, _rotationZ4Position * 90);
        public Vec3D ToAngleDegreesVec3D() => new Vec3D( _rotationX4Position * 90, _rotationY4Position * 90, _rotationZ4Position * 90);

        public static PerpendicularRotation3 PerpendicularRotationFrom4PositionInt(int x, int y, int z)
        {
            CheckInt4Positions(x);
            CheckInt4Positions(y);
            CheckInt4Positions(z);
            return PerpendicularRotationFrom4PositionInt_NoNeedToCheck(x, y, z);
        }
        internal static PerpendicularRotation3 PerpendicularRotationFrom4PositionInt_NoNeedToCheck(int x, int y, int z)
        {
            return new PerpendicularRotation3(x, y, z);
        }

        public static PerpendicularRotation3 PerpendicularRotationFromAngleDegrees(int x, int y, int z)
        {
            CheckIntAngle(x);
            CheckIntAngle(y);
            CheckIntAngle(z);
            return new PerpendicularRotation3(x / 90, y / 90, z / 90);
        }

        private static void CheckInt4Positions(int position)
        {
            if (position < 0 || position > 3)
                Throw.ArgumentException("position must be one of: 0, 1, 2, 3");
        }
        private static void CheckIntAngle(int angle)
        {
            if (angle < 0 || angle > 270 || angle % 90 != 0)
                Throw.ArgumentException("angle must be one of: 0, 90, 180, 270");
        }
        private PerpendicularRotation3(int xInt4pos, int yInt4pos, int zInt4pos)
        {
            _rotationX4Position = xInt4pos;
            _rotationY4Position = yInt4pos;
            _rotationZ4Position = zInt4pos;
        }
    }

}