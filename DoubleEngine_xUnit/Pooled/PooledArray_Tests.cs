using Collections.Pooled;
using DoubleEngine;
using CollectionLike;
using FluentAssertions;
using NUnit.Framework.Internal;
using Xunit.Abstractions;
using CollectionLike.Pooled;

namespace DoubleEngine_xUnit.Pooled;

public partial class PooledArray_Tests
{
    private readonly ITestOutputHelper TestContext;
    private static Randomizer rand = Randomizer.CreateRandomizer();
    public PooledArray_Tests(ITestOutputHelper output)
    {
        TestContext = output;
    }


    [Theory]
    [InlineData(10)]
    [InlineData(42)]
    [InlineData(500)]
    public static void Fill_Test(int arraySize)
    {
        PooledArrayStruct<int> pooled = new PooledArrayStruct<int>(arraySize);
        int a = rand.Next(1000);
        int b = rand.Next(1000);
        while (a == b) 
            b = rand.Next(1000);
        pooled.Fill(a);
        //a.Should().
        var temp = pooled.AsReadOnlySpan().ToArray();
        temp.Should().NotBeNullOrEmpty();
        temp.Should().HaveCount(arraySize);
        temp.Should().OnlyContain(x=> x == a);
        temp.Should().NotContain(x => x == b);

    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(123)]
    [InlineData(200)]
    [InlineData(255)]
    [InlineData(500)]
    [InlineData(3023)]
    [InlineData(13027)]
    public void CreateFromDictionary_Keys_Values_RandomFillDictionaryWithSByte(int count)
    {
        var random = new Randomizer(count);
        Dictionary<sbyte, int> dictionary = new Dictionary<sbyte, int>(20);
        using PooledDictionary<sbyte, int> pooled = Expendables.CreateDictionary<sbyte, int>(20);
        for (int i = 0; i < count; i++)
        {
            sbyte value = (sbyte)random.NextByte(); //because NextSByte is buggy: NextSByte it gives only values 0..127
                                                    //TestContext.Write($"{value} ");
            if (dictionary.ContainsKey(value))
                dictionary[value] = dictionary[value] + 1;
            else
                dictionary.Add(value, 1);
            if (pooled.ContainsKey(value))
                pooled[value] = pooled[value] + 1;
            else
                pooled.Add(value, 1);
        }
        sbyte[] dictKeys = new sbyte[dictionary.Count];
        dictionary.Keys.CopyTo(dictKeys, 0);
        using PooledArrayStruct<sbyte> pooledKeys = PooledArrayStruct<sbyte>.CreateFromDictionaryKeys(pooled);
        //TestContext.WriteLine(dictKeys.Length);
        //TestContext.WriteLine(String.Join(',', dictKeys));
        Assert.AreEqual<sbyte>(dictKeys, pooledKeys.AsReadOnlySpan());

        int[] dictValues = new int[dictionary.Count];
        dictionary.Values.CopyTo(dictValues, 0);
        using PooledArrayStruct<int> pooledValues = PooledArrayStruct<int>.CreateFromDictionaryValues(pooled);
        //TestContext.WriteLine(dictValues.Length);
        //TestContext.WriteLine(String.Join(',',dictValues));
        Assert.AreEqual<int>(dictValues, pooledValues.AsReadOnlySpan());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(123)]
    [InlineData(200)]
    [InlineData(255)]
    [InlineData(500)]
    [InlineData(3023)]
    [InlineData(13027)]
    public void CreateFromSet_RandomFillSetWithSByte(int count)
    {
        var random = new Randomizer(count);
        HashSet<sbyte> set = new HashSet<sbyte>(20);
        using PooledSet<sbyte> pooled = Expendables.CreateSet<sbyte>(20);
        for (int i = 0; i < count; i++)
        {
            sbyte value = (sbyte)random.NextByte(); //because NextSByte is buggy: NextSByte it gives only values 0..127
            set.Add(value);
            pooled.Add(value);
        }
        sbyte[] setAsArray = new sbyte[set.Count];
        set.CopyTo(setAsArray, 0);
        //TestContext.WriteLine(String.Join(',', setAsArray));
        using PooledArrayStruct<sbyte> pooledArray = PooledArrayStruct<sbyte>.CreateFromSet<sbyte>(pooled);
        //TestContext.WriteLine(String.Join(',', pooledArray.AsReadOnlySpan().ToArray()));
        TestContext.WriteLine("${pooledArray.AsSpan().Length}");
        //TestContext.WriteLine(String.Join(',', dictKeys));
        Assert.AreEqual<sbyte>(setAsArray, pooledArray.AsReadOnlySpan());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(123)]
    [InlineData(200)]
    [InlineData(255)]
    [InlineData(500)]
    [InlineData(3023)]
    [InlineData(13027)]
    public void AsSpan_RandomFillOfSpan(int count)
    {
        var random = new Randomizer(count);
        double[] array = new double[count];
        using PooledArrayStruct<double> pooled = new PooledArrayStruct<double>(count);
        var pooledSpan = pooled.AsSpan();
        for (int i = 0; i < count; ++i)
        {
            double value = random.NextDouble();
            array[i] = value;
            pooledSpan[i] = value;
        }
        //Assert.IsNotEmpty(array);
        Assert.AreEqual<double>(array, pooledSpan);

    }

}
