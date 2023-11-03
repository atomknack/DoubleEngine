using DjvuNet.Tests.Xunit;
using DoubleEngine.Atom.Loaders;
using DoubleEngine_xUnit.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DoubleEngine_xUnit.Encoders;

public class EncodeVariableLengthLong
{
    //private readonly ITestOutputHelper _output;
    //public EncodeVariableLengthLong(ITestOutputHelper output){_output = output;}

    static long[] numOfElements = new long[] { 0, long.MaxValue, 8, 1778, 18, 29955676, 28, 38, 9957, 113, 58, 260, 68, 
         396495193049897381L, 96495193049897381L, 6495193049897381L, 495193049897381L, 95193049897381L, 
        5193049897381L, 193049897381L, 93049897381L, 3049897381L, 2049897381L, 249897381L};
    public static IEnumerable<object[]> NumOfElements => numOfElements.WrapAs1Parameter();

    [Fact]
    public void EncodeNegativeShouldThrow()
    {
        long neg = -1L;
        Action act = () => EncodeDecodeLong(neg);
        act.Should().Throw<ArgumentException>();
    }

    [DjvuTheory]
    [InlineData(0, false)]
    [InlineData(2, false)]
    [InlineData(3, false)]
    [InlineData(4, false)]
    [InlineData(10, false)]
    [InlineData(10,true)]
    [InlineData(100, false)]
    [InlineData(100, true)]
    public void EncodeDecodeNRandomPositiveLongsInSameStream(int n, bool multiplyRandByCoeff)
    {
        long[] randLongs = new long[n];
        for(int i = 0; i< n; i++)
        {
            randLongs[i] = TestGenerators.RandPositiveLong();
            if (multiplyRandByCoeff)
            {
                int randInt = TestGenerators.RandInt(1, 100);
                //_output.WriteLine($"{randLongs[i]} {randInt}");
                randLongs[i] = (long)(((ulong)randLongs[i]) >> randInt);
            }

        }
        using var stream = new MemoryStream();
        for (int i = 0; i < n; ++i)
            VariableLengthEncoder.EncodeVariableLengthPositiveLong(stream, randLongs[i]);
        stream.Position = 0;
        for (int i = 0; i< n; ++i)
        {
            long actual = VariableLengthEncoder.DecodeVariableLengthPositiveLong(stream);
            //_output.WriteLine($"{actual} {randLongs[i]}");
            actual.Should().Be(randLongs[i]);
        }
        //_output.WriteLine($"{stream.Position}");
        stream.Position.Should().Be(stream.Length);
    }

    [DjvuTheory]
    [InlineData(10)]
    public void EncodeDecodeNRandomPositiveLongs(int n)
    {
        if (n<0)
            throw new ArgumentOutOfRangeException(nameof(n));
        for (int i = 0; i < n; ++i)
            EncodeDecodeLong(TestGenerators.RandPositiveLong());
    }

    [DjvuTheory]
    [MemberData(nameof(EncodeVariableLengthLong.NumOfElements),
        MemberType = typeof(EncodeVariableLengthLong))]
    public void EncodeDecodeLong(long value)
    {
        long expected = value;
        //_output.WriteLine(expected.ToString());
        using MemoryStream stream= new MemoryStream();
        VariableLengthEncoder.EncodeVariableLengthPositiveLong(stream, expected);
        stream.Seek(0, SeekOrigin.Begin);
        long actual = VariableLengthEncoder.DecodeVariableLengthPositiveLong(stream);//, _output.WriteLine);
        //_output.WriteLine($"{stream.Position} {stream.Length}");
        actual.Should().Be(expected);
        //_output.WriteLine(expected.ToString());
    }
}
