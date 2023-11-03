/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DoubleEngine.CollectionLike.Comparers
{

    [Obsolete("Do not use Unity types")]
    public class Vector2Comparer_1em5 : IEqualityComparer<Vector2>
    {
        public static readonly Vector2Comparer_1em5 ComparerInstance = new Vector2Comparer_1em5();

        private const float EPSILON = 2e-5f; //0,00002
        private const float MULTIPLIER = 1e+4f; //10000
        public bool Equals(Vector2 a, Vector2 b) => a.CloseByEach(b, EPSILON);

        public int GetHashCode(Vector2 obj)
        {
            int hash = FLOATtoRoundedINTdirtyFastHash(obj.x);
            hash = ((hash << 9) ^ hash) ^ FLOATtoRoundedINTdirtyFastHash(obj.y);
            return hash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FLOATtoRoundedINTdirtyFastHash(float f) => Mathf.RoundToInt(f * MULTIPLIER);
    }

}
*/