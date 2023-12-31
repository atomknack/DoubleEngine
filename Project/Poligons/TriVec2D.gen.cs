//----------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a Tri.tt
//     Changes will be lost if the code is regenerated.
// </auto-generated>
//----------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace DoubleEngine
{
    public readonly partial struct TriVec2D
    {        
        public readonly Vec2D v0;
        public readonly Vec2D v1;
        public readonly Vec2D v2;

        public TriVec2D(Vec2D v0, Vec2D v1, Vec2D v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }
        public TriVec2D(ReadOnlySpan<Vec2D> vertices, IndexedTri iTri)
        {
            this.v0 = vertices[iTri.v0];
            this.v1 = vertices[iTri.v1];
            this.v2 = vertices[iTri.v2];
        }

        public void Deconstruct(out Vec2D v0, out Vec2D v1, out Vec2D v2)
        {
            v0 = this.v0;
            v1 = this.v1;
            v2 = this.v2;
        }
        public override string ToString()
        {
            return $"{v0}, {v1}, {v2}";
        }

        public readonly bool VerticesEqual_Manhattan(TriVec2D other, double epsilon = 0.000001d) //TODO: need testing
        {
            if( (v0.DistanceManhattan(other.v0) <= epsilon) && (v1.DistanceManhattan(other.v1) <= epsilon) && (v2.DistanceManhattan(other.v2) <= epsilon) )
                return true;
            return false;
        }
        public readonly TriVec2D ShiftOnce() => new TriVec2D(v1, v2, v0); //TODO: need testing
        public readonly TriVec2D ShiftTwice() => new TriVec2D(v2, v0, v1); //TODO: need testing

        public static double DotBetweenSides(Vec2D sideAvector, Vec2D center, Vec2D sideBvector)=>
            Vec2D.Dot((sideAvector-center).Normalized(), (sideBvector-center).Normalized());

        /*
        private const double THIRD = 1d / 3d;
        public static (Vec2D center, double maxDistanceSqr) CalcSqrSphere(Vec2D v0, Vec2D v1, Vec2D v2)
        {
            var center = (v0+v1+v2)*THIRD;
            var maxDistanceSqr = MathU.Max(center.DistanceSqr(v0), center.DistanceSqr(v1));
            maxDistanceSqr = MathU.Max(maxDistanceSqr, center.DistanceSqr(v2));
            return (center, maxDistanceSqr);
        }
        */
    }

}

