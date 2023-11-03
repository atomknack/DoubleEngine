using System;
using System.Collections;
using System.Collections.Generic;
using Collections.Pooled;
using System.Linq;
using DoubleEngine;
using NUnit.Framework;
using CollectionLike;
using CollectionLike.Pooled;
using CollectionLike.Enumerables;
using DjvuNet.Tests.Xunit;
using FluentAssertions;

namespace DoubleEngine_xUnit.CollectionLike.ExtensionsTests
{

    public class Contains_Tests
    {
        public static Vec3D[] vertices = new Vec3D[] {
            new Vec3D(0, -1, 8), new Vec3D(-1,-1, 1), new Vec3D(-1, 1, 0), new Vec3D(1,1, 8), //0-3
            new Vec3D(1, -1, 2), new Vec3D(0.7, -8, 0), new Vec3D(0,0,9), new Vec3D(2, 2.0005,9), //4-7
            new Vec3D(5,5,9), new Vec3D(5,-5,9) //8,9
        };
        public static int[][] listIndices = new int[][] {
            new int[]{ 0, 1, 2 }, //0
            new int[]{ 2, 0, 1 }, //1
            new int[]{ 2, 1, 0 }, //2
            new int[]{ 0, 3, 4, 8, 2}, //3
            new int[]{ 5,4, 8, 2, 9}, //4
            new int[]{ }, //5 empty
        };
        public static IEnumerable<object[]> PooledListIndicesContainVertice = new object[][] {
            new object[]{new int[]{ 0, 1, 2 }, new Vec3D(-1,-1, 1) }, //1
            new object[]{new int[]{ 0, 3, 4, 8, 2}, new Vec3D(5,5,9) }, //8
            new object[]{new int[]{ 5,4, 8, 2, 9}, new Vec3D(-1, 1, 0) }, //2
        };

        [DjvuTheory]
        [MemberData(nameof(PooledListIndicesContainVertice))]
        public void IndicesContainsVec3D_Tests(int[] indices, Vec3D vertice)
        {
            using PooledList<int> list = new PooledList<int>(indices);
            list.ContainsVec3D_2em5(vertice, vertices.AsSpan()).Should().BeTrue();

            int index = Array.IndexOf(vertices, vertice);
            index.Should().BeGreaterThan(-1);
            list.Contains(index).Should().BeTrue();
        }

        public static int[][] intArrs = new int[][] {
            new int[]{ 0, 1, 2 }, //0
            new int[]{ 2, 0, 1 }, //1
            new int[]{ 2, 1, 0 }, //2
            new int[]{ 0, 3, 4, 8, 2}, //3
            new int[]{ 5,4, 8, 2, -19}, //4
            new int[]{ }, //5 empty
        };

        [DjvuTheory]
        [InlineData(0, 2)]
        [InlineData(1, 0)]
        [InlineData(3, 0)]
        [InlineData(3, 8)]
        [InlineData(4, -19)]
        public void IntsContains_Tests(int arrIndex, int SearchValue)
        {
            intArrs[arrIndex].AsSpan().Contains(SearchValue, (x, y) => x == y).Should().BeTrue();
        }

        [DjvuTheory]
        [InlineData(0, 2)]
        [InlineData(1, 0)]
        [InlineData(3, 0)]
        [InlineData(3, 8)]
        [InlineData(4, -19)]
        public void IntsPooledListContains_Tests(int arrIndex, int SearchValue)
        {
            using PooledList<int> list = new PooledList<int>(intArrs[arrIndex]);
            list.Contains(SearchValue, (x, y) => x == y).Should().BeTrue();
        }

        [DjvuTheory]
        [InlineData(0, 3)]
        [InlineData(3, -1)]
        [InlineData(3, 1)]
        [InlineData(4, -18)]
        [InlineData(5, 0)]
        [InlineData(5, 10)]
        public void Ints_Not_Contains_Tests(int arrIndex, int SearchValue)
        {
            intArrs[arrIndex].AsSpan().Contains(SearchValue, (x, y) => x == y).Should().BeFalse();
        }

        [DjvuTheory]
        [InlineData(0, 3)]
        [InlineData(3, -1)]
        [InlineData(3, 1)]
        [InlineData(4, -18)]
        [InlineData(5, 0)]
        [InlineData(5, 10)]
        public void IntsPooledList_Not_Contains_Tests(int arrIndex, int SearchValue)
        {
            using PooledList<int> list = new PooledList<int>(intArrs[arrIndex]);
            list.Contains(SearchValue, (x, y) => x == y).Should().BeFalse();
        }
    }

}