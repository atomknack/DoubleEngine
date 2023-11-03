using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Collections.Pooled;
using DoubleEngine.CollectionLike;
using DoubleEngine.Enumerables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{

    //[ShortRunJob]
    [MemoryDiagnoser]
    public class SimpleBenchmark
    {
        [Params(10, 100)]
        public int N;

        private int[] lengths;

        [GlobalSetup]
        public void GlobalSetup()
        {
            lengths = TestGenerators.RandArray(N, 1000, 10000);
        }
        private static readonly int length = 1000;

        [Benchmark]
        public long Array_Bench()
        {
            long result = 0;
            foreach (int len in lengths)
                result += Array_Bench_Inner(len);
            return result;
        }
        private int Array_Bench_Inner(int len)
        {
            Span<int> span = new int[len].AsSpan();
            return FillAndSum(span);
            //return Enumerable.Range(0, 1000).Sum();
        }

        [Benchmark]
        public long Stackalloc_Bench()
        {
            long result = 0;
            foreach (int len in lengths)
                result += Stackalloc_Bench_Inner(len);
            return result;
        }
        private int Stackalloc_Bench_Inner(int len)
        {
            Span<int> span = stackalloc int[len];
            return FillAndSum(span);
            //return Enumerable.Range(0, 1000).Sum();
        }

        [Benchmark]
        public long PooledArray_Bench()
        {
            long result = 0;
            foreach (int len in lengths)
                result += PooledArray_Bench_Inner(len);
            return result;
        }
        public int PooledArray_Bench_Inner(int len)
        {
            using PooledArrayStruct<int> arr = new PooledArrayStruct<int>(len);
            return FillAndSum(arr.AsSpan());
        }

        ThreadLocal<PooledArrayStruct<int>> threadLocal = new ThreadLocal<PooledArrayStruct<int>>();

        [Benchmark]
        public long ThreadLocalPooledArray_Bench()
        {
            threadLocal.Value.Dispose();
            threadLocal.Value = new PooledArrayStruct<int>();
            long result = 0;
            foreach (int len in lengths)
                result += ThreadLocalPooledArray_Bench_Inner(len);
            return result;
        }
        public int ThreadLocalPooledArray_Bench_Inner(int len)
        {
            PooledArrayStruct<int> arr = threadLocal.Value;
            if (arr.Count != len)
            {
                arr.Dispose();
                arr = new PooledArrayStruct<int>(len);
            }
  
            int result = FillAndSum(arr.AsSpan());
            threadLocal.Value = arr;
            return result;
        }

        [Benchmark]
        public long ThreadLocalPooledArray_Bench_AllocOnlyIfLengthNotEnough()
        {
            threadLocal.Value.Dispose();
            threadLocal.Value = new PooledArrayStruct<int>();
            long result = 0;
            foreach (int len in lengths)
                result += ThreadLocalPooledArray_Bench_AllocOnlyIf_Inner(len);
            return result;
        }
        public int ThreadLocalPooledArray_Bench_AllocOnlyIf_Inner(int len)
        {
            PooledArrayStruct<int> arr = threadLocal.Value;
            if (arr.Count < len)
            {
                arr.Dispose();
                arr = new PooledArrayStruct<int>(len);
            }
            Span<int> span = arr.AsSpan().Slice(0, len);

            int result = FillAndSum(span);
            threadLocal.Value = arr;
            return result;
        }


        public int FillAndSum(Span<int> span)
        {
            span.FillAsRange(0);
            return span.Sum();
        }

    }
    /*
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            BenchmarkRunner.Run<SimpleBenchmark>();
        }
    }
    */
}
