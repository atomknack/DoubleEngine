using DoubleEngine.Guard;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DoubleEngine.Atom
{
    //public readonly struct ScaleInversionV3
    //public readonly struct PerpendicularRotation3
    //public readonly struct ScaleInversionPerpendicularRotation3 compile them to(from) int index that has minvalue and maxvalue+1

    /// <summary>
    /// stores 3d boolean vector and 3d 4position rotation vector in single int
    /// 0b00000000_00000000_0000000_11_11_11_111;
    /// 0b00000000_00000000_ 0000000_ (zrotation4pos:) 11_ (yrotation4pos:) 11_ (xrotation4pos:) 11 _ (zscale:) 1 (yscale:) 1 (xscale:) 1;
    /// </summary>
    public readonly struct ScaleInversionPerpendicularRotation3: IEquatable<ScaleInversionPerpendicularRotation3>
    {
        public const int minValue = 0;
        //public const int maxValue     =  0b00000000_00000000_0000000_11_11_11_111;
        public const int maxValuePlusOne = 0b00000000_00000000_0000001_00_00_00_000;
        public const int NeedLengthForFullArray = maxValuePlusOne;
        /* ///totally not needed: it just array of numbers from 0 to maxValue///
        public static readonly ScaleInversionPerpendicularRotation3[] AllPossibleValues;

        [Obsolete("Need testing")]
        static ScaleInversionPerpendicularRotation3()
        {
            AllPossibleValues = CalcAllPossibleValues();
        }

        [Obsolete("May be will box to object, need testing")]
        private static ScaleInversionPerpendicularRotation3[] CalcAllPossibleValues()
        {
            var result = new ScaleInversionPerpendicularRotation3[NeedLengthForFullArray];
            for (int i = minValue; i < NeedLengthForFullArray; i++)
            {
                result[i] = new ScaleInversionPerpendicularRotation3(i);
            }
            return result;
        }*/


        public readonly int index;

        //private const int INVERT_X_bitmask = 0b00000000_00000000_00000000_00000001;
        private const int INVERT_XSCALE_shift = 0;
        //private const int INVERT_Y_bitmask = 0b00000000_00000000_00000000_00000010;
        private const int INVERT_YSCALE_shift = 1;
        //private const int INVERT_Z_bitmask = 0b00000000_00000000_00000000_00000100;
        private const int INVERT_ZSCALE_shift = 2;
        private int Shift_Scale_X(int temp) => temp >> INVERT_XSCALE_shift;
        private int Shift_Scale_Y(int temp) => temp >> INVERT_YSCALE_shift;
        private int Shift_Scale_Z(int temp) => temp >> INVERT_ZSCALE_shift;

        private const int ROTATE_X_shift = 3;
        private const int ROTATE_Y_shift = 5;
        private const int ROTATE_Z_shift = 7;
        private int Shift_Rotate_X(int temp) => temp >> ROTATE_X_shift;
        private int Shift_Rotate_Y(int temp) => temp >> ROTATE_Y_shift;
        private int Shift_Rotate_Z(int temp) => temp >> ROTATE_Z_shift;

        //private const int SHIFT_1bit = 1;
        private const int MASK_1bit = 0b00000000_00000000_00000000_00000001;
        //private const int SHIFT_2bit = 2;
        private const int MASK_2bit = 0b00000000_00000000_00000000_00000011;

        public ScaleInversionPerpendicularRotation3(ScaleInversionV3 s, PerpendicularRotation3 r)
        {
            int temp = 0;
            temp = temp | Convert.ToInt32(s.invertX);
            temp = temp | (Convert.ToInt32(s.invertY) << INVERT_YSCALE_shift);
            temp = temp | (Convert.ToInt32(s.invertZ) << INVERT_ZSCALE_shift);
            temp = temp | (r._rotationX4Position << ROTATE_X_shift);
            temp = temp | (r._rotationY4Position << ROTATE_Y_shift);
            temp = temp | (r._rotationZ4Position << ROTATE_Z_shift);
            this.index = temp;
        }

        //private int Shift_1bit(int val) => val >> SHIFT_1bit;
        private int Get_1bit(int val) => val & MASK_1bit;
        public ScaleInversionV3 GetScaleInversionVector()
        {
            return new ScaleInversionV3(Get_1bit(Shift_Scale_X(index)) == 1, Get_1bit(Shift_Scale_Y(index)) == 1, Get_1bit(Shift_Scale_Z(index)) == 1);
        }
        private int Get_2bit(int val) => val & MASK_2bit;
        public PerpendicularRotation3 GetPerpendicularRotation3D()
        {
            return PerpendicularRotation3.PerpendicularRotationFrom4PositionInt_NoNeedToCheck(
                Get_2bit(Shift_Rotate_X(index)),
                Get_2bit(Shift_Rotate_Y(index)),
                Get_2bit(Shift_Rotate_Z(index)) );
        }

        public bool Equals(ScaleInversionPerpendicularRotation3 other) => index == other.index;
        public override bool Equals(object obj) => obj is ScaleInversionPerpendicularRotation3 sipr3d && this.Equals(sipr3d);
        public static bool operator !=(in ScaleInversionPerpendicularRotation3 lhs, in ScaleInversionPerpendicularRotation3 rhs) => !(lhs == rhs);
        public static bool operator ==(in ScaleInversionPerpendicularRotation3 lhs, in ScaleInversionPerpendicularRotation3 rhs) => lhs.index == rhs.index;
        public override int GetHashCode() => index;


        public static explicit operator int(ScaleInversionPerpendicularRotation3 sipr3d) => ToInt(sipr3d);
        public static int ToInt(ScaleInversionPerpendicularRotation3 sipr3d) => sipr3d.index;
        public static explicit operator ScaleInversionPerpendicularRotation3(int value) => FromInt(value);
        public static ScaleInversionPerpendicularRotation3 FromInt(int value)
        {
            if (IsOutsideOfValidValues(value))
                Throw.ArgumentOutOfRangeException(nameof(value));
            return new ScaleInversionPerpendicularRotation3(value);
        }

        public static bool IsValid(int value) => 
            !IsOutsideOfValidValues(value);

        private static bool IsOutsideOfValidValues(int value) =>
            value < minValue || value >= maxValuePlusOne;
        

        /// <summary>
        ///  Dangerous, No checks. Use static FromInt instead.
        /// </summary>
        /// <param name="value"></param>
        private ScaleInversionPerpendicularRotation3(int value)
        {
            index = value;
        }
    }

}