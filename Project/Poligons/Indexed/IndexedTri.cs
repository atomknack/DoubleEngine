using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DoubleEngine
{
    [StructLayout(LayoutKind.Sequential, Pack = sizeof(int))]
    public readonly struct IndexedTri
    {
        public readonly int v0;
        public readonly int v1;
        public readonly int v2;

        public readonly override string ToString() =>
        $"({v0}, {v1}, {v2})";
        public readonly IndexedTri Backwards() => new IndexedTri(v2, v1, v0);

        public readonly bool Equals(IndexedTri other) => v0 == other.v0 && v1 == other.v1 && v2 == other.v2;
        public static bool operator !=(IndexedTri lhs, IndexedTri rhs) => !(lhs == rhs);
        public static bool operator ==(IndexedTri lhs, IndexedTri rhs) => lhs.Equals(rhs);
        public readonly override int GetHashCode() => ((v0 << 20) ^ (v1 << 10)) ^ v2;
        public readonly override bool Equals(object other) => (other is IndexedTri) ? Equals((IndexedTri)other) : false;

        public readonly bool IsCorner(int index) => v0 == index || v1 == index || v2 == index;
        public readonly bool IsNotCorner(int index) => ! IsCorner(index);
        //same as // public readonly bool IsNotCorner(int index) => v0 != index && v1 != index && v2 != index;

        public readonly bool NotDegenerate() => ! Degenerate();
        public readonly bool Degenerate() => v0 == v1 || v1 == v2 || v2==v0;
        public IndexedTri ReplacedVertice(int oldVertice, int newVertice) => 
            new IndexedTri(v0==oldVertice?newVertice:v0, v1==oldVertice?newVertice:v1, v2==oldVertice?newVertice:v2);
        public IndexedTri((int v0, int v1, int v2) tri) : this(tri.v0, tri.v1, tri.v2) { }
        public IndexedTri(int v0, int v1, int v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }

        public void Deconstruct(out int v0, out int v1, out int v2)
        {
            v0 = this.v0;
            v1 = this.v1;
            v2 = this.v2;
        }

        public readonly IndexedTri ShiftOnce() => new IndexedTri(v1, v2, v0); //TODO: need testing
        public readonly IndexedTri ShiftTwice() => new IndexedTri(v2, v0, v1); //TODO: need testing
    }
}
