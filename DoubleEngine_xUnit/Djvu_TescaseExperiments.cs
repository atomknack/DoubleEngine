using DjvuNet.Tests.Xunit;
using DoubleEngine;
using DoubleEngine_xUnit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit
{
    public class Djvu_TescaseExperiments
    {
        [DjvuTheory]
        [MemberData(nameof(ExampleOfMemberData_DjvuFormatter), parameters: 3)]
        public void TestCaseDjvu_XUnit(Vec3I expected, Vec3I actual) =>
    ActualTestCase(expected, actual);

        public static IEnumerable<object[]> ExampleOfMemberData_DjvuFormatter(int n)
        {
            yield return new object[] { new Vec3I(1, 0, 0), new Vec3I(1, 0, 0) };
            yield return new object[] { new Vec3I(7, 7, 1), new Vec3I(1, 7, 0) };
            yield return new object[] { new Vec3I(9, 0, 4), new Vec3I(1, 0, 0) };
            if (n == 3)
                yield break;
            yield return new object[] { new Vec3I(9, 9, 4), new Vec3I(1, 9, 0) };
        }

        [Theory]
        [MemberData(nameof(ExampleOfMemberData), parameters: 3)]
        public void TestCaseXUnit(X<Vec3I> expected, X<Vec3I> actual) =>
            ActualTestCase(expected.v, actual.v);
        private static void ActualTestCase(Vec3I expected, Vec3I actual)
        {
            Assert.Equal(expected.y, actual.y);
        }

        public static IEnumerable<object[]> ExampleOfMemberData(int n)
        {
            yield return new object[] { new X<Vec3I>(new Vec3I(1, 0, 0)), new X<Vec3I>(new Vec3I(1, 0, 0)) };
            yield return new object[] { new X<Vec3I>(new Vec3I(7, 7, 1)), new X<Vec3I>(new Vec3I(1, 7, 0)) };
            yield return new object[] { new X<Vec3I>(new Vec3I(9, 0, 4)), new X<Vec3I>(new Vec3I(1, 0, 0)) };
            if (n == 3)
                yield break;
            yield return new object[] { new X<Vec3I>(new Vec3I(9, 9, 4)), new X<Vec3I>(new Vec3I(1, 9, 0)) };
        }
    }
}
