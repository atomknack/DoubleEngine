using System;
using CollectionLike.Pooled;


namespace DoubleEngine
{
    public static partial class PolyHelpers
    {
        //WIP
        //[Obsolete("need testing")]
        public static void RemoveMiddlePointsThatLieOnSameLineOfPoly(this PoolListVec2D poly)
        {
            if ( poly == null )
                throw new ArgumentNullException("poly should not be null");
            if ( poly.Count < 3 )
                throw new ArgumentException("number of poly vertices should be more than 3");
            bool removedPoints = false;
            do
            {
                removedPoints = false;
                Vec2D start = poly[poly.IndexOfLast()-1];
                Vec2D end = poly[0];
                Vec2D possibleMiddle = poly.Last();
                if (TryRemove(poly, poly.IndexOfLast()-1, 0, poly.IndexOfLast()))
                {
                    removedPoints = true;
                    continue;
                }
                for(int i = poly.IndexOfLast(); i > 1; --i)
                {
                    if (TryRemove(poly, i-2, i, i-1)) //if (removedPoints = TryRemove(poly, i-2, i, i-1))
                    {
                        removedPoints = true;
                        continue;
                    }
                }
                if (TryRemove(poly, poly.IndexOfLast(), 1, 0))
                {
                    removedPoints = true;
                    continue;
                }
            } while (removedPoints);
        }
        private static bool TryRemove(PoolListVec2D poly, int start, int end, int point)
        {
            if (EdgeVec2D.PointBelongsToEdge(poly[start], poly[end], poly[point]))
            {
                poly.RemoveAt(point);
                return true;
            }
            return false;
        }

        //PointBelongsToEdge
    }
}
