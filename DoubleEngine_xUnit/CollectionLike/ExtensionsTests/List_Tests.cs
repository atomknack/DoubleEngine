using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DjvuNet.Tests.Xunit;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;

namespace DoubleEngine_xUnit.CollectionLike.ExtensionsTests
{
    public class List_Tests
    {
        private static readonly short[][] testLists = new short[][] {
            new short[] { }, //0
            new short[] { 1 }, //1
            new short[] { 2, 7 }, //2
            new short[] { 2, 0, 1 }, //3
            new short[] { 2, 0}, //4
            new short[] { 1, 2, 3, 4, 5 }, //5
            new short[] { 2, 7, 1, 2, 3, 4, 5 }, // 6
            new short[] { 1, 2, 3}, //7
            new short[] { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 }, //8
            new short[] { 1, 2, 3, 4, 5, 1, 2}, //9
            new short[] { 7 }, //10
            new short[] { 0, 1 }, //11
            new short[] { 1, 3, 4, 5 }, //12
            new short[] { 1, -2, 3, 4, 5 }, //13
            new short[] { 0 }, //14
            new short[] { 2 }, //15
            new short[] { 1, -2, 3, 4, 5 }, //16
            new short[] { 7, 2}, //17
        };

        public static IEnumerable<object[]> RemoveAllElementsStartingFromIndex_positive_TestCases => new object[][] {
            RAFITC(2,1,15), RAFITC(8,5,5), RAFITC(6,2,2), RAFITC(0,0,0),RAFITC(0,2,0), 
            RAFITC(8,3,7), RAFITC(8,7,9), //negative example: RAFITC(0,-2,0),
        };
        public static IEnumerable<object[]> removeAll_positive_TestCases => new object[][] {
            RATC(1, 1, 0), RATC(0, 0, 0), RATC(0, 9, 0), RATC(0, -100, 0), RATC(2, 2, 10),
            RATC(2, 7, 15), RATC(3, 1, 4), RATC(3, 2, 11), RATC(4, 2, 14), RATC(5, 2, 12),
            RATC(8, 29, 8), RATC(13, 2, 13), RATC(13, 2, 16), RATC(16, 2, 16)};

        public static IEnumerable<object[]> InsertAtStart_positive_TestCases => new object[][] {
            IASTC(0,1,1),IASTC(0,0,14),  IASTC(0,2,15), IASTC(11,2,3), IASTC(10,2,2), 
            IASTC(14,2,4), IASTC(15,7,17), 
        };

        [DjvuTheory]
        [MemberData(nameof(InsertAtStart_positive_TestCases))]
        public void InsertAtStart_Test(short[] array, short ItemToInsert, short[] expected)
        {
            List<short> list = new List<short>(array);
            list.InsertAtStart(ItemToInsert);
            list.Should().Equal(expected);
        }

        [DjvuTheory]
        [MemberData(nameof(removeAll_positive_TestCases))]
        public void RemoveAll_Test(short[] array, short itemToRemove, short[] expected)
        {
            List<short> list = new List<short>(array);
            list.RemoveAll(itemToRemove);
            list.Should().Equal(expected);
        }
        [Theory]
        [InlineData(2, 2, 10)]
        [InlineData(1, 1, 0)]
        [InlineData(4, 2, 14)]
        public void RemoveAll_Tests(int arrayIndex, short itemToRemove, int resultArrayIndex)
        {
            RemoveAll_Test(testLists[arrayIndex], itemToRemove, testLists[resultArrayIndex]);
        }
        [DjvuTheory]
        [MemberData(nameof(RemoveAllElementsStartingFromIndex_positive_TestCases))]
        public void RemoveAllElementsStartingFromIndex_Test(short[] array, int startingIndex, short[] expected)
        {
            List<short> list = new List<short>(array);
            list.RemoveAllElementsStartingFromIndex(startingIndex);
            list.Should().Equal(expected);
        }

        private static object[] IASTC(int arrayIndex, short ItemToInsert, int resultArrayIndex) => //InsertAtStartTestCase
            new object[] { testLists[arrayIndex], ItemToInsert, testLists[resultArrayIndex] };
        private static object[] RAFITC(int arrayIndex, int startingIndex, int resultArrayIndex) => //RemoveAllElementsStartingFromIndexTestCase
            new object[] { testLists[arrayIndex], startingIndex, testLists[resultArrayIndex] };

        private static object[] RATC(int arrayIndex, short itemToRemove, int resultArrayIndex) => //RemoveAllTestCase
            new object[] { testLists[arrayIndex], itemToRemove, testLists[resultArrayIndex] };
    }
}
