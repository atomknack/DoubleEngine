using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DoubleEngine_xUnit.Enumerables;

public class Range_Tests
{
    private readonly ITestOutputHelper _output;
    public Range_Tests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void RangeFrom0To10()
    {
        var arr = new int[11];
        for(int i=0;i<arr.Length;++i)
            arr[i] = i;

        var rangedArr = arr[0..10];
        _output.WriteLine(String.Join(',', rangedArr));
    }
}
