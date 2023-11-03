using CollectionLike.Enumerables;
using CollectionLike.Pooled;
using System;
using System.Collections.Generic;

namespace DoubleEngine
{
    public static partial class PolyHelpers
    {
        public static EdgeVec2D[] EdgesVec2DFromCornersVec2D(this Vec2D[] corners) //should be correct enclosed poly
            => EdgesVec2DFromCornersVec2D(new ReadOnlySpan<Vec2D>(corners));
        public static EdgeVec2D[] EdgesVec2DFromCornersVec2D(this ReadOnlySpan<Vec2D> corners) //should be correct enclosed poly 
        {
            EdgeVec2D[] edges = new EdgeVec2D[corners.Length];
            int indexOfLastElement = corners.IndexOfLast();
            for (int i = 0; i < indexOfLastElement; i++)
                edges[i] = new EdgeVec2D(corners[i], corners[i + 1]);
            edges[indexOfLastElement] = new EdgeVec2D(corners[indexOfLastElement], corners[0]);
            return edges;
        }

        public static PoolListEdgeVec2D PoolListEdgesVec2DFromCornersVec2D(this Span<Vec2D> corners) =>
            PoolListEdgesVec2DFromCornersVec2D((ReadOnlySpan<Vec2D>)corners);
        public static PoolListEdgeVec2D PoolListEdgesVec2DFromCornersVec2D(this ReadOnlySpan<Vec2D> corners) //should be correct enclosed poly 
        {
            //Debug.Log("readonlyspan PoolListEdgesVec2DFromCornersVec2D");
            PoolListEdgeVec2D edges = Expendables.CreateList<EdgeVec2D>(corners.Length, true);// new EdgeVec2D[corners.Length];
            int indexOfLastElement = corners.IndexOfLast();
            for (int i = 0; i < indexOfLastElement; i++)
                edges[i] = new EdgeVec2D(corners[i], corners[i + 1]);
            edges[indexOfLastElement] = new EdgeVec2D(corners[indexOfLastElement], corners[0]);
            return edges;
        }
    }
}
