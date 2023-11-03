using CollectionLike.Comparers.SetsWithCustomComparer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionLike.Comparers;

public static class SpanContainsVectors
{
    public static bool ContainsVec3D_2em5(this Span<int> listOfIndices, Vec3D vector, ReadOnlySpan<Vec3D> vertices) =>
        ContainsVec3D_2em5((ReadOnlySpan<int>)listOfIndices, vector, vertices);
    public static bool ContainsVec3D_2em5(this ReadOnlySpan<int> listOfIndices, Vec3D vector, ReadOnlySpan<Vec3D> vertices)
    {
        for (int i = 0; i < listOfIndices.Length; ++i)
            if (Vec3DComparer_2em5.StaticEquals(vector, vertices[listOfIndices[i]]))
                return true;
        return false;
    }
}
