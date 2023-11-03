using System;
using System.Collections;
using System.Collections.Generic;

namespace DoubleEngine.Atom
{


public readonly struct FlatNodeTransform : IEquatable<FlatNodeTransform>
{
    public static readonly PerpendicularAngle[] allPosibleAngles = { PerpendicularAngle.a0, PerpendicularAngle.a90, PerpendicularAngle.a180, PerpendicularAngle.a270 };
    public static readonly FlatNodeTransform[] allFlatNodeTransforms = {
        new FlatNodeTransform(PerpendicularAngle.a0,false),
        new FlatNodeTransform(PerpendicularAngle.a90,false),
        new FlatNodeTransform(PerpendicularAngle.a180,false),
        new FlatNodeTransform(PerpendicularAngle.a270,false),
        new FlatNodeTransform(PerpendicularAngle.a0,true),
        new FlatNodeTransform(PerpendicularAngle.a90,true),
        new FlatNodeTransform(PerpendicularAngle.a180,true),
        new FlatNodeTransform(PerpendicularAngle.a270,true) };
    public static FlatNodeTransform Default => new FlatNodeTransform();

        public static readonly FlatNodeTransform InvertedX = Default.InvertX(); //need testing
        public static readonly FlatNodeTransform InvertedY = Default.InvertX(); //need testing

        public readonly PerpendicularAngle rotation;
    public readonly bool inverted;

    private readonly bool IsSidewards => (rotation == PerpendicularAngle.a90 || rotation == PerpendicularAngle.a270);


    //TODO: toooodooooo test rotation and inversions
    public FlatNodeTransform InvertX()
    {
        if (IsSidewards)
            return new FlatNodeTransform(this.rotation.Rotate(PerpendicularAngle.a180), Invert(this.inverted));
        return new FlatNodeTransform(this.rotation, Invert(this.inverted));
    }
    public FlatNodeTransform InvertY()
    {
        if (IsSidewards)
            return new FlatNodeTransform(this.rotation, Invert(this.inverted));
        return new FlatNodeTransform(this.rotation.Rotate(PerpendicularAngle.a180), Invert(this.inverted));
    }

    public FlatNodeTransform Rotate(PerpendicularAngle angle)
    {
        return new FlatNodeTransform(rotation.Rotate(angle), inverted);
    }

    private static bool Invert(bool inverted) => (!inverted);

    private FlatNodeTransform(int rotation = (int)PerpendicularAngle.a0, bool inverted = false)
    {
        this.rotation = AngleMethods.FromInt(rotation);
        this.inverted = inverted;
    }
    public FlatNodeTransform(PerpendicularAngle rotation = PerpendicularAngle.a0, bool inverted = false)
    {
        this.rotation = rotation;
        this.inverted = inverted;
    }

    public FlatNodeTransform Transform(FlatNodeTransform transform) //// is it working? - absolutely not sure. need tests.
    {
        /*bool newinvert;
        if (transform.inverted)
            newinvert = Invert(inverted);
        else
            newinvert = inverted;
        return new FlatNodeTransform(rotation.Rotate(transform.rotation), newinvert);*/

        FlatNodeTransform temp = this;
        if (transform.inverted)
            temp = temp.InvertX();
        return temp.Rotate(transform.rotation);
    }

        public bool Equals(FlatNodeTransform other) => inverted == other.inverted && rotation == other.rotation;
        public override bool Equals(object obj) => obj is FlatNodeTransform other && Equals(other);
        public override int GetHashCode() => (((int)rotation) << 5) ^ (inverted ? 1 : 0);
        public static bool operator ==(FlatNodeTransform lhs, FlatNodeTransform rhs) => lhs.inverted == rhs.inverted && lhs.rotation == rhs.rotation;
        public static bool operator !=(FlatNodeTransform lhs, FlatNodeTransform rhs) => lhs.inverted != rhs.inverted || lhs.rotation != rhs.rotation;
        public override string ToString() => $"Tranform (inverted:{inverted}, angle:{rotation.Float():F1}";


        //public FlatNodeTransform Rotate(int rotation) => new FlatNodeTransform(this, rotate: rotation);


        /*public FlatNodeTransform(bool invertX = false, bool invertY = false, Angle rotation = Angle.a0) : this(invertX, invertY, (int)rotation) 
        {
            //TODO test
        }
        public FlatNodeTransform(bool invertX = false, bool invertY = false, int rotation = 0)
        {
            _rotation = CalcRotation(rotation);
            _xScale = CalcScale(invertX);
            _yScale = CalcScale(invertY);
            CheckCorrectness(this);
        }
        private FlatNodeTransform(FlatNodeTransform node, bool invertX = false, bool invertY = false, int rotate = 0)
        {
            _rotation = CalcRotation(node._rotation + rotate);
            _xScale = node._xScale * CalcScale(invertX);
            _yScale = node._yScale * CalcScale(invertY);
            CheckCorrectness(this);
        }


        private FlatNodeTransform(int xScale, int yScale, int rotation)
        {
            _xScale = xScale;
            _yScale = yScale;
            _rotation = CalcRotation(rotation);
            CheckCorrectness(this);
        }
        private static void CheckCorrectness(FlatNodeTransform node)
        {
            if ((node._xScale != 1) || (node._xScale != -1))
                throw new ArgumentException("x scale should be 1 or -1");
            if ((node._yScale != 1) || (node._yScale != -1))
                throw new ArgumentException("y scale should be 1 or -1");
            if (node._rotation % 90 != 0)
                throw new ArgumentException("rotation should be 0 or divisible by 90");
        }
        private static int CalcScale(bool inverted) => inverted ? -1 : 1;



        public bool Equals(FlatNodeTransform other)
        {
            return other._xScale == _xScale && other._yScale == _yScale && other._rotation == _rotation;
        }*/
    }


}