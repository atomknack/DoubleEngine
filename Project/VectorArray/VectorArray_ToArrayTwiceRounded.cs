using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DoubleEngine
    {
        public static partial class VectorArray
        {

            public static Vec2D[] ToArrayTwiceRoundedVec2D(this Span<Vec2D> vectors) =>
                ToArrayTwiceRoundedVec2D((ReadOnlySpan<Vec2D>)vectors);
            public static Vec2D[] ToArrayTwiceRoundedVec2D(this ReadOnlySpan<Vec2D> vectors)
            {
                const double FIRSTROUND = 100000;
                const double SECONDROUND = 10000;
                Vec2D[] newArray = new Vec2D[vectors.Length];
                for (int i = 0; i < vectors.Length; ++i)
                {
                    double x = (double)vectors[i].x;
                    x = Math.Round(x * FIRSTROUND) / FIRSTROUND;
                    x = Math.Round(x * SECONDROUND) / SECONDROUND;
                    double y = (double)vectors[i].y;
                    y = Math.Round(y * FIRSTROUND) / FIRSTROUND;
                    y = Math.Round(y * SECONDROUND) / SECONDROUND;
                    newArray[i] = new Vec2D(x, y);
                }
                return newArray;
            }

            public static Vec3D[] ToArrayTwiceRoundedVec3D(this Span<Vec3D> vectors)=>
                ToArrayTwiceRoundedVec3D((ReadOnlySpan<Vec3D>)vectors);
            public static Vec3D[] ToArrayTwiceRoundedVec3D(this ReadOnlySpan<Vec3D> vectors)
            {
                const double FIRSTROUND = 100000;
                const double SECONDROUND = 10000;
                Vec3D[] newArray = new Vec3D[vectors.Length];
                for (int i = 0; i < vectors.Length; ++i)
                {
                    double x = (double)vectors[i].x;
                    x = Math.Round(x * FIRSTROUND) / FIRSTROUND;
                    x = Math.Round(x * SECONDROUND) / SECONDROUND;
                    double y = (double)vectors[i].y;
                    y = Math.Round(y * FIRSTROUND) / FIRSTROUND;
                    y = Math.Round(y * SECONDROUND) / SECONDROUND;
                    double z = (double)vectors[i].z;
                    z = Math.Round(z * FIRSTROUND) / FIRSTROUND;
                    z = Math.Round(z * SECONDROUND) / SECONDROUND;
                    newArray[i] = new Vec3D(x, y, z);
                }
                return newArray;
            }

        }
    }
