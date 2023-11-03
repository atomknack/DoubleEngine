using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly partial struct EdgeVec2D
    {
		public static bool IsVerticeOnEdges(Vec2D[] vertices, EdgeIndexed[] edges, Vec2D toCheck, out EdgeIndexed edgeContainingPoint)// out int EdgeStart, out int EdgeEnd)
		{
			for (var i = 0; i < edges.Length; i++)
			{
				if (PointBelongsToEdge(vertices[edges[i].start], vertices[edges[i].end], toCheck))
				{
					edgeContainingPoint = edges[i];
					return true;
				}
			}
			edgeContainingPoint = new EdgeIndexed(); //point is not on any edge, just fill up out variable, so compiler wont complain
			return false;
		}
		public static bool IsVerticeOnAnyTriangleEdge(Vec2D[] vertices, int[] triangles, Vec2D toCheck, out EdgeIndexed edgeContainingPoint)//, out int EdgeStart, out int EdgeEnd)
		{
			for (var i = 0; i < triangles.Length; i += 3)
			{
				if (PointBelongsToEdge(vertices[triangles[i]], vertices[triangles[i + 1]], toCheck))
				{
					edgeContainingPoint = new EdgeIndexed(triangles[i], triangles[i + 1]);
					return true;
				}
				if (PointBelongsToEdge(vertices[triangles[i + 1]], vertices[triangles[i + 2]], toCheck))
				{
					edgeContainingPoint = new EdgeIndexed(triangles[i + 1], triangles[i + 2]);
					return true;
				}
				if (PointBelongsToEdge(vertices[triangles[i]], vertices[triangles[i + 2]], toCheck))
				{
					edgeContainingPoint = new EdgeIndexed(triangles[i], triangles[i + 2]);
					return true;
				}
			}
			edgeContainingPoint = new EdgeIndexed(); //point is not on any edge, just fill up out variable, so compiler wont complain
			return false;
		}
	}
}
