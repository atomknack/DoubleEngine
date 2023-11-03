using System.Collections;
using System.Collections.Generic;
using DoubleEngine;
using System;

public partial class TestGenerators
{
    public static readonly System.Random rand = new System.Random();
    private static byte[] buffer1Byte = new byte[1];

    public static int RandomValueFrom(ReadOnlySpan<int> values) => values[rand.Next(0, values.Length)];
    public static T RandomValueFrom<T>(ReadOnlySpan<T> values) => values[rand.Next(0, values.Length)];

    public static (int a, long b)[] RandTupleArray(int count, int amin, int amax, long bmin, long bmax)
    {
        if (bmin < int.MinValue || bmax > int.MaxValue)
            throw new ArgumentException("b type is long but min max should be within int borders");
        if (count < 0 || amin > amax || bmin > bmax)
            throw new ArgumentException();
        (int a, long b)[] result = new (int a, long b)[count];
        for (int i = 0; i < count; i++)
            result[i] = RandTuple(amin, amax, bmin, bmax);
        return result;
    }
    public static int[] RandArray(int count, int min, int max)
    {
        if (count < 0 || min > max)
            throw new ArgumentException();
        int[] result = new int[count];
        for (int i = 0; i < count; i++)
            result[i] = rand.Next(min, max);
        return result;
    }
    public static short[] RandShortArray(int count, short min, short max)
    {
        if (count < 0 || min > max)
            throw new ArgumentException();
        short[] result = new short[count];
        for (int i = 0; i < count; i++)
            result[i] = (short)rand.Next(min, max);
        return result;
    }
    public static byte RandByte() 
    {
        rand.NextBytes(buffer1Byte);
        return buffer1Byte[0];
    }

    public static byte[] RandBytes(int length)
    {
        byte[] result = new byte[length];
        rand.NextBytes(result);
        return result;
    }
    public static int RandInt(int min, int max) => rand.Next(min, max);
    public static long RandPositiveLong() =>
        Math.Abs(rand.NextInt64());

    public static double RandDouble(System.Random rand = null, double somewhatMin = -100, double somewhatMax = 100)
    {
        double mul = 100;
        int minInt = (int)(somewhatMin * mul);
        int maxInt = (int)(somewhatMax * mul);
        if (rand == null) rand = TestGenerators.rand;
        return rand.Next(minInt, maxInt) / mul;
    }
    public static double RandDouble()=>rand.NextDouble();
    public static (int a, int b) RandTuple(int amin, int amax, long bmin, long bmax) => (rand.Next(amin, amax), rand.Next((int)bmin, (int)bmax));
    public static Vec3D RandVector3D(System.Random rand = null, double somewhatMin = -100, double somewhatMax = 100)
    {
        double mul = 100;
        int minInt = (int)(somewhatMin * mul);
        int maxInt = (int)(somewhatMax * mul);
        if (rand == null) rand = TestGenerators.rand;
        return new Vec3D(rand.Next(minInt, maxInt) / mul, rand.Next(minInt, maxInt) / mul, rand.Next(minInt, maxInt) / mul);
    }

    public static Vec4D RandVector4D(System.Random rand = null, double somewhatMin = -100, double somewhatMax = 100)
    {
        double mul = 10000;
        int minInt = (int)(somewhatMin * mul);
        int maxInt = (int)(somewhatMax * mul);
        if (rand == null) rand = TestGenerators.rand;
        return new Vec4D(
                rand.Next(minInt, maxInt) / mul, 
                rand.Next(minInt, maxInt) / mul, 
                rand.Next(minInt, maxInt) / mul, 
                rand.Next(minInt, maxInt) / mul);
    }
}