using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CollectionLike.Comparers.SetsWithCustomComparer;



public class Vec2DComparer_2em5 : IEqualityComparer<Vec2D>
{
    public static readonly Vec2DComparer_2em5 ComparerInstance = new Vec2DComparer_2em5();
    public const double EPSILON = 2e-5d; //0,00002

    private const double MULTIPLIER = 1e+4f; //10000
    public bool Equals(Vec2D a, Vec2D b) => StaticEquals(a, b);
    [MethodImpl(256)] internal static bool StaticEquals(Vec2D a, Vec2D b) => a.CloseByEach(b, EPSILON);

    public int GetHashCode(Vec2D vec)
    {
        int hash = FLOATtoRoundedINTdirtyFastHash(vec.x);
        hash = hash << 9 ^ hash ^ FLOATtoRoundedINTdirtyFastHash(vec.y);
        return hash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int FLOATtoRoundedINTdirtyFastHash(double f) => Convert.ToInt32(f * MULTIPLIER);
}