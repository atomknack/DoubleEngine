using CollectionLike.Enumerables;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DoubleEngine
{
    public partial record IndexedPolyVec2D
    {
        public readonly record struct Sliver
        {
            internal readonly Subpoly _poly; //Poly is Clockwise
            internal readonly Subpoly[] _holes; // any hole is CounterClockwise
            public Subpoly GetBodyPoly() => _poly;
            public ReadOnlySpan<Subpoly> GetHoles() => new ReadOnlySpan<Subpoly>(_holes);
            
            internal Sliver(Subpoly poly, List<Subpoly> holes = null) : this(poly, holes?.ToArray()) { }
            private Sliver(Subpoly poly, Subpoly[] ImmutableHoles = null)
            {
                this._poly = poly;
                if (ImmutableHoles == null || ImmutableHoles.Length == 0)
                    this._holes = null;
                else
                    this._holes = ImmutableHoles;
            }

            public List<List<int>> HolesCornersToAsNewLists()
            {
                if (_holes == null)
                    return new List<List<int>>();
                List<List<int>> result = new List<List<int>>(_holes.Length);
                foreach (var hole in _holes)
                    result.Add(hole.Corners.CreateNewListFromSpanElements());
                return result;
            }
        }

    }
}
