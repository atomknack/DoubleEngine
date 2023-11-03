﻿//     This code was generated by a tool. Changes will be lost if the code is regenerated.

using DoubleEngine;
using FluentAssertions;
using DoubleEngine_xUnit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DjvuNet.Tests.Xunit;
using CollectionLike.Comparers;
using CollectionLike.Comparers.SetsWithCustomComparer;

namespace DoubleEngine_xUnit.CollectionLike.Comparers
{
    public class Comparers_Test
    {
        static Vec3D[] vectors3D = new[] {  new Vec3D(0, 0, 0), new Vec3D(8, 9, 100), new Vec3D(80, -9, -100) };
        public static IEnumerable<object[]> Vectors3Dtestcases => vectors3D.WrapAs1Parameter();

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em5_Test_x(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em5.EPSILON;
            epsilon.Should().Be(0.00002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em5();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(smaller, 0, 0);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(bigger, 0, 0);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em5_Test_y(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em5.EPSILON;
            epsilon.Should().Be(0.00002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em5();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(0, smaller, 0);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(0, bigger, 0);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em5_Test_z(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em5.EPSILON;
            epsilon.Should().Be(0.00002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em5();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(0, 0, smaller);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(0, 0, bigger);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em5_Test_xyz(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em5.EPSILON;
            epsilon.Should().Be(0.00002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em5();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(smaller, smaller, smaller);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(bigger, bigger, bigger);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em7_Test_x(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em7.EPSILON;
            epsilon.Should().Be(0.0000002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em7();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(smaller, 0, 0);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(bigger, 0, 0);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em7_Test_y(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em7.EPSILON;
            epsilon.Should().Be(0.0000002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em7();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(0, smaller, 0);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(0, bigger, 0);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em7_Test_z(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em7.EPSILON;
            epsilon.Should().Be(0.0000002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em7();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(0, 0, smaller);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(0, 0, bigger);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors3Dtestcases))]
        public void Vec3DComparer_2em7_Test_xyz(Vec3D vec)
        {
            double epsilon = Vec3DComparer_2em7.EPSILON;
            epsilon.Should().Be(0.0000002);
            IEqualityComparer<Vec3D> comparer = new Vec3DComparer_2em7();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec3D close = vec + new Vec3D(smaller, smaller, smaller);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec3D tooFar = vec + new Vec3D(bigger, bigger, bigger);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }
        static Vec2D[] vectors2D = new[] {  new Vec2D(0, 0), new Vec2D(8, 9), new Vec2D(80, -9) };
        public static IEnumerable<object[]> Vectors2Dtestcases => vectors2D.WrapAs1Parameter();

        [DjvuTheory]
        [MemberData(nameof(Vectors2Dtestcases))]
        public void Vec2DComparer_2em5_Test_x(Vec2D vec)
        {
            double epsilon = Vec2DComparer_2em5.EPSILON;
            epsilon.Should().Be(0.00002);
            IEqualityComparer<Vec2D> comparer = new Vec2DComparer_2em5();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec2D close = vec + new Vec2D(smaller, 0);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec2D tooFar = vec + new Vec2D(bigger, 0);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors2Dtestcases))]
        public void Vec2DComparer_2em5_Test_y(Vec2D vec)
        {
            double epsilon = Vec2DComparer_2em5.EPSILON;
            epsilon.Should().Be(0.00002);
            IEqualityComparer<Vec2D> comparer = new Vec2DComparer_2em5();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec2D close = vec + new Vec2D(0, smaller);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec2D tooFar = vec + new Vec2D(0, bigger);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }

        [DjvuTheory]
        [MemberData(nameof(Vectors2Dtestcases))]
        public void Vec2DComparer_2em5_Test_xy(Vec2D vec)
        {
            double epsilon = Vec2DComparer_2em5.EPSILON;
            epsilon.Should().Be(0.00002);
            IEqualityComparer<Vec2D> comparer = new Vec2DComparer_2em5();
            double smaller = epsilon * 0.99;
            double bigger = epsilon * 1.01;

            Vec2D close = vec + new Vec2D(smaller, smaller);
            comparer.GetHashCode(vec).Should().Be(comparer.GetHashCode(close));
            comparer.Equals(vec, close).Should().BeTrue($"{epsilon} {smaller} {vec} {close}");
            Vec2D tooFar = vec + new Vec2D(bigger, bigger);
            comparer.Equals(vec, tooFar).Should().BeFalse($"{epsilon:F8} {bigger:F8} {vec:F7} {tooFar:F7}");
            //comparer.GetHashCode(vec).Should().NotBe(comparer.GetHashCode(close));
        }
    }
}
