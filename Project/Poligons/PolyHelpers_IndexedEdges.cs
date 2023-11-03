using CollectionLike.Enumerables;
using System;
using System.Collections.Generic;
//using System.Collections.Immutable;


namespace DoubleEngine
{
    public static partial class PolyHelpers
    {
        public static IEnumerable<EdgeRegistered> RegisteredEdgesEnumerableFromCorners(this IReadOnlyList<RegistryIndex> corners) //should be correct enclosed poly 
        {
            int indexOfLastElement = corners.IndexOfLast();
            for (int i = 0; i < indexOfLastElement; i++)
                yield return new EdgeRegistered(corners[i], corners[i + 1]);
            yield return new EdgeRegistered(corners[indexOfLastElement], corners[0]);
        }

        public static IEnumerable<EdgeIndexed> IndexedEdgesEnumerableFromCorners(this IReadOnlyList<int> corners) //should be correct enclosed poly 
        {
            int indexOfLastElement = corners.IndexOfLast();
            for (int i = 0; i < indexOfLastElement; i++)
                yield return new EdgeIndexed(corners[i], corners[i + 1]);
            yield return new EdgeIndexed(corners[indexOfLastElement], corners[0]);
        }

        public static EdgeIndexed[] IndexedEdgesFromCorners(this int[] corners) //should be correct enclosed poly
            => IndexedEdgesFromCorners(new ReadOnlySpan<int>(corners));
        public static EdgeIndexed[] IndexedEdgesFromCorners(this ReadOnlySpan<int> corners) //should be correct enclosed poly 
        {
            EdgeIndexed[] edges = new EdgeIndexed[corners.Length];
            int indexOfLastElement = corners.IndexOfLast();
            for (int i = 0; i < indexOfLastElement; i++)
                edges[i] = new EdgeIndexed(corners[i], corners[i + 1]);
            edges[indexOfLastElement] = new EdgeIndexed(corners[indexOfLastElement], corners[0]);
            return edges;
        }
        

    }
}
