using Xunit.Abstractions;
using CollectionLike.Comparers;
using DjvuNet.Tests.Xunit;
using CollectionLike.Pooled;

namespace DoubleEngine_xUnit.Pooled;

public class LookUp_Tests
{
    private readonly ITestOutputHelper TestContext;
    public LookUp_Tests(ITestOutputHelper output)
    {
        TestContext = output;
    }

    //private static Func<long, long, bool> s_longLongComparison = (x, y) => x == y;

    [Theory]
    [InlineData(100, 0, -20, 20)]
    [InlineData(1, 10, -20, 20)]
    [InlineData(2, 25, -20, 20)]
    [InlineData(100, 10, -20, 200)]
    [InlineData(10, 10, -20, 20)]
    [InlineData(10, 15, -20, 20)]
    [InlineData(10, 20, -20, 20)]
    [InlineData(10, 25, -20, 20)]
    [InlineData(10, 50, -20, 20)]
    [InlineData(33, 500, -20, 20)]
    [InlineData(330, 500, -20, 20)]
    [InlineData(330, 300, -20, 20)]
    [InlineData(330, 1500, -20, 20)]
    public void RandomLookUp_DistinctPairs_Test(int keysLength, int numberOfRandomKeyValuePairs, int minValue, int maxValue)
    {
        (int key, long value)[] tupleList = TestGenerators.RandTupleArray(numberOfRandomKeyValuePairs, 0, keysLength, minValue, maxValue);

        using var pooled = new LookUpTable<long>(keysLength, StaticComparers.LongLongDefault, 8);
        foreach (var pair in tupleList)
            pooled.AddItem(pair.key, pair.value);

        tupleList = tupleList.Distinct().ToArray();

        ILookup<int, long> lookUp = tupleList.ToLookup<(int key, long value), int, long>(tuple => tuple.key, tuple => tuple.value);

        var keys = tupleList.Select(x => x.key).Distinct();
        TestContext.WriteLine("distinct tuples:");
        TestContext.WriteLine(tupleList.ToStringHelperSorted());
        //TestContext.WriteLine("lookup:");
        //TestContext.WriteLine(lookUp.ToStringHelper());
        TestContext.WriteLine("pooled:");
        TestContext.WriteLine(pooled.ToStringHelper());
        var freeSlots = pooled.Debug_ElementsOcupiedMemory();
        TestContext.WriteLine("raw pooled internal:");
        TestContext.WriteLine($"{freeSlots.one} {freeSlots.two} {freeSlots.many}");
        TestContext.WriteLine(String.Join(',', pooled.Debug_GetInternalArray()));
        Assert.Equal(lookUp.Count, pooled.Debug_GetNumberOfUsedKeys());
        Assert.Equal(tupleList.Length, pooled.Debug_GetCount());

        foreach (var key in keys)
        {
            Assertions.SpanContainsAllIEnumerableElements(pooled.GetValues(key), lookUp[key]);
        }
    }

    public static IEnumerable<object[]> Example123 => new List<object[]>{
        new object[] {
            new (int key, string value)[] {(1,"A"), (2,"B") }
        },
        new object[] {
            new (int key, string value)[] {(1,"A"), (2,"B"), (1, "A"), (2, "B"), (1, "f")}
        },
        new object[] {
            new (int key, string value)[] {(1,"A"), (2,"B"), (3,"C"), (1,"C"), (0,"xxxxx"), (4,"C"), (5,"Z"), (2,"C2"), (1,"Zzz") }
        },
    };

    [DjvuTheory]
    [MemberData(nameof(Example123), parameters: 1)]
    public void CreateLookUp((int key, string value)[] tupleList)
    {
        //(int key, string value)[] tupleList = parameters as (int key, string value)[];
        TestContext.WriteLine(String.Join(',', tupleList));
        ILookup<int, string> lookUp = tupleList.ToLookup<(int key, string value), int, string>(tuple => tuple.key, tuple => tuple.value);
        TestContext.WriteLine("");
        for (int i = 0; i <= 5; ++i)
        {
            var lookupValues = lookUp[i];
            foreach (var value in lookupValues)
            {
                var reconstructedTuple = (i, value);
                Assert.Contains(reconstructedTuple, tupleList);
                TestContext.WriteLine($"{reconstructedTuple}, ");
            }
        }
        TestContext.WriteLine("");
        for (int i = 0; i <= 5; ++i)
        {
            var lookupValues = lookUp[i];
            var fromList = tupleList.Where(x => x.key == i);
            foreach (var tupleItem in fromList)
            {
                var reconstructedTuple = (i, tupleItem.value);
                Assert.Contains(tupleItem.value, lookupValues);
                TestContext.WriteLine($"{reconstructedTuple}, ");
            }
        }

    }
}
