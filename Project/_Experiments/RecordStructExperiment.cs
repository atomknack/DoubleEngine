using System;
using System.Collections.Generic;
using System.Text;

//namespace DoubleEngine.Experiments
//{
internal readonly record struct RecordStructExperiment
{
    internal readonly int x;
    internal readonly int y;

    internal RecordStructExperiment(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    internal void Deconstruct(out int x, out int y)
    {
        x = this.x;
        y = this.y;
    }
    internal RecordStructExperiment Test_CreateRS(int x, int y)
    {
        return new RecordStructExperiment(x, y);
    }
    internal (int x, int y) Test_ReturnTupleAfterDeconstruction()
    {
        (int c, int d) = this;
        return (c, d);
    }
}
//}
