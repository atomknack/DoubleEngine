using DoubleEngine.Guard;
using System;

namespace DoubleEngine.Atom
{
    public static partial class AngleMethods
{
    public static QuaternionD ZNegToZNeg => QuaternionD.identity;
    public static QuaternionD XNegToZNeg => QuaternionD.Euler(0, PerpendicularAngle.a270.Float(), 0);
    public static QuaternionD YNegToZNeg => QuaternionD.Euler(PerpendicularAngle.a90.Float(), 0, 0);
    public static QuaternionD ZPosToZNeg => QuaternionD.Euler(0, PerpendicularAngle.a180.Float(), 0); //?
    public static QuaternionD XPosToZNeg => QuaternionD.Euler(0, PerpendicularAngle.a90.Float(), 0); //?
    public static QuaternionD YPosToZNeg => QuaternionD.Euler(PerpendicularAngle.a270.Float(), 0, 0); //?

    public static QuaternionD ZNegToXNeg => QuaternionD.Euler(0, PerpendicularAngle.a90.Float(), 0);
    public static QuaternionD ZNegToYNeg => QuaternionD.Euler(PerpendicularAngle.a270.Float(), 0, 0);
    public static QuaternionD ZNegToZPos => QuaternionD.Euler(0, PerpendicularAngle.a180.Float(), 0);
    public static QuaternionD ZNegToXPos => QuaternionD.Euler(0, PerpendicularAngle.a270.Float(), 0);
    public static QuaternionD ZNegToYPos => QuaternionD.Euler(PerpendicularAngle.a90.Float(), 0, 0);




    public static float Float(this PerpendicularAngle angle)
    {
        return (float)angle;
    }

    /// <exception cref="System.ArgumentException">Angle {rotation} is not divisible by 90</exception>
    public static PerpendicularAngle FromInt(int rotation)
    {
        if (rotation % 90 != 0)
            Throw.ArgumentException("Angle {rotation} is not divisible by 90");
        return (PerpendicularAngle)CalcRotation(rotation);
    }
    public static PerpendicularAngle Rotate(this PerpendicularAngle angle, PerpendicularAngle rotation) ///????
    {
        var r = CalcRotation((int)angle + (int)rotation);
        return (PerpendicularAngle)r;
    }

    private static int CalcRotation(int rotation)////???????
    {
        if (rotation == 0)
            return 0;
        int newRotation = rotation % 360;
        if (newRotation < 0)
            newRotation += 360;
        return newRotation;
    }
}

}