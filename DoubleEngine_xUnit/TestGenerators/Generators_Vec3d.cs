using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using DoubleEngine;
using Newtonsoft.Json;
using static Assertions;

public partial class TestGenerators
{
    /*public static IEnumerable<(Vec3D a, Vec3D b, Vec3D c)> TriVec3D_Random_50()
    {
        for(int i = 0; i< 50; i++)
            yield return (RandVector3D(), RandVector3D(), RandVector3D());
    }*/


    public static IEnumerable Vec3D_Random_5 = new[] {
        RandVector3D(20, -10, 10),
        RandVector3D(-100, -20, 20),
        RandVector3D(5.5, -30, 30),
        RandVector3D(0, -50, 50),
        RandVector3D(1, -90, 90), };

    public class Vec3D_Random_50 : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            for (var i = 0; i < 50; i++)
                yield return new object[] { RandVector3D(null, -1000, 1000) };
        }
    }
    public class Vec3DData_1Vec3D : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Vec3DTestData.Init();
            for (var i = 0; i < Vec3DTestData.d_vectors.Count; i++)
                yield return new object[] { Vec3DTestData.d_vectors[i] };
        }
    }

    public class Vec3DData_Value : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Vec3DTestData.Init();
            for (var i = 0; i < Vec3DTestData.d_vectors.Count; i++)
                yield return new object[] { Vec3DTestData.d_vectors[i] };
        }
    }

    public class Vec3DData_Pairs : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Vec3DTestData.Init();
            var rand = new System.Random();
            var count = Vec3DTestData.d_vectors.Count;
            for (var i = 0; i < 50; i++)
            {
                var v1i = rand.Next(0, count);
                var v2i = rand.Next(0, count);
                yield return new object[] { Vec3DTestData.d_vectors[v1i], Vec3DTestData.d_vectors[v2i] };
            }
        }
    }

    public static class Vec3DTestData
    {
        public static List<Vec3D> d_vectors = new List<Vec3D>();
        public static List<double> dNumbers = new List<double>(new double[]{0, 1, -0.0000000001d, 42.005, -23.007, 0.00001d,
            0.0000000001d,0.005,0.00005, 1, 2,3, 7, 10, 14.5,51, 103.103103103, 200, -0.00001d,
            -0.0000000001d,-0.005,-0.00005, -1, -2,-3, -7, -10, -14.5,-51,42, -103.103103103, -200,});

        private static readonly object _initLock = new object();
        public static void Init()
        {
            lock (_initLock)
            {
                var rand = new System.Random();
                var count = dNumbers.Count;
                if (d_vectors.Count > 0)
                    return;
                for (var i = 0; i < 20; i++)
                {
                    var xi = rand.Next(0, count);
                    var yi = rand.Next(0, count);
                    var zi = rand.Next(0, count);
                    d_vectors.Add(new Vec3D(dNumbers[xi], dNumbers[yi], dNumbers[zi]));
                }
            }
        }
    }




}