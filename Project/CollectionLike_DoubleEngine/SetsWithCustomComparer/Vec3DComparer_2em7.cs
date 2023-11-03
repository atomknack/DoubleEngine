using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CollectionLike.Comparers.SetsWithCustomComparer;

public class Vec3DComparer_2em7 : IEqualityComparer<Vec3D>
{
    public static readonly Vec3DComparer_2em7 ComparerInstance = new Vec3DComparer_2em7();
    public const double EPSILON = 2e-7d; //0,0000002

    private const double MULTIPLIER = 1e+6d; //1000000

    public bool Equals(Vec3D a, Vec3D b) => StaticEquals(a, b);
    [MethodImpl(256)] internal static bool StaticEquals(Vec3D a, Vec3D b) => a.CloseByEach(b, EPSILON); // [MethodImpl(256)] //[MethodImpl(MethodImplOptions.AggressiveInlining)] //MethodImplOptions.AggressiveInlining = 256

    public int GetHashCode(Vec3D vec)
    {
        int hash = FLOATtoRoundedINTdirtyFastHash(vec.x);
        hash = hash << 9 ^ hash ^ FLOATtoRoundedINTdirtyFastHash(vec.y);
        return hash << 9 ^ hash ^ FLOATtoRoundedINTdirtyFastHash(vec.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int FLOATtoRoundedINTdirtyFastHash(double f) => Convert.ToInt32(f * MULTIPLIER);
}