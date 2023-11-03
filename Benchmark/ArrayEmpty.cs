using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Collections.Pooled;
using DoubleEngine.CollectionLike;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark;

//[ShortRunJob]
[MemoryDiagnoser]
public class ArrayEmpty
{
    [Benchmark]
    public void Array_Empty_Bench()
    {
        int[] empty = Array.Empty<int>();
    }

    [Benchmark]
    public void Array_New_Bench()
    {
        int[] empty = new int[0];
    }
}
