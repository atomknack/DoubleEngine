﻿//----------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool. Changes will be lost if the code is regenerated.
// </auto-generated>
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DoubleEngine
{
    public static partial class VectorUtil
    {
    public static bool In_CompareBySqrDistance(this Vec2D[] vertices, Vec2D vertex, double epsilonSqr = 1E-9d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                return true;
        return false;
    }

    public static bool In_CompareByEach(this Vec2D[] vertices, Vec2D vertex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                return true;
        return false;
    }
    public static bool In_CompareByEach(this Vec2D[] vertices, Vec2D vertex, out int verticeIndex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                {
                verticeIndex = i;
                return true;
                }
        verticeIndex=-1;
        return false;
    }

    public static bool InIndexes_CompareByEach(this int[] indexesToCompare, Vec2D[] vertices, Vec2D vertex, out int indexOfPositionInIndexed, double epsilon = 1E-5d)
    {
        for (var i = 0; i < indexesToCompare.Length; i++)
            if (vertex.CloseByEach(vertices[indexesToCompare[i]], epsilon))
                {
                indexOfPositionInIndexed = i;
                return true;
                }
        indexOfPositionInIndexed=-1;
        return false;
    }

    public static int Count_CompareBySqrDistance(this Vec2D[] vertices, Vec2D vertex, double epsilonSqr = 1E-9d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                count++;
        return count;
    }

    public static int Count_CompareByEach(this Vec2D[] vertices, Vec2D vertex, double epsilon = 1E-5d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                count++;
        return count;
    }
    public static bool In_CompareBySqrDistance(this ReadOnlySpan<Vec2D> vertices, Vec2D vertex, double epsilonSqr = 1E-9d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                return true;
        return false;
    }

    public static bool In_CompareByEach(this ReadOnlySpan<Vec2D> vertices, Vec2D vertex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                return true;
        return false;
    }
    public static bool In_CompareByEach(this ReadOnlySpan<Vec2D> vertices, Vec2D vertex, out int verticeIndex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                {
                verticeIndex = i;
                return true;
                }
        verticeIndex=-1;
        return false;
    }

    public static bool InIndexes_CompareByEach(this ReadOnlySpan<int> indexesToCompare, ReadOnlySpan<Vec2D> vertices, Vec2D vertex, out int indexOfPositionInIndexed, double epsilon = 1E-5d)
    {
        for (var i = 0; i < indexesToCompare.Length; i++)
            if (vertex.CloseByEach(vertices[indexesToCompare[i]], epsilon))
                {
                indexOfPositionInIndexed = i;
                return true;
                }
        indexOfPositionInIndexed=-1;
        return false;
    }

    public static int Count_CompareBySqrDistance(this ReadOnlySpan<Vec2D> vertices, Vec2D vertex, double epsilonSqr = 1E-9d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                count++;
        return count;
    }

    public static int Count_CompareByEach(this ReadOnlySpan<Vec2D> vertices, Vec2D vertex, double epsilon = 1E-5d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                count++;
        return count;
    }
    public static bool In_CompareBySqrDistance(this Vec3D[] vertices, Vec3D vertex, double epsilonSqr = 1E-9d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                return true;
        return false;
    }

    public static bool In_CompareByEach(this Vec3D[] vertices, Vec3D vertex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                return true;
        return false;
    }
    public static bool In_CompareByEach(this Vec3D[] vertices, Vec3D vertex, out int verticeIndex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                {
                verticeIndex = i;
                return true;
                }
        verticeIndex=-1;
        return false;
    }

    public static bool InIndexes_CompareByEach(this int[] indexesToCompare, Vec3D[] vertices, Vec3D vertex, out int indexOfPositionInIndexed, double epsilon = 1E-5d)
    {
        for (var i = 0; i < indexesToCompare.Length; i++)
            if (vertex.CloseByEach(vertices[indexesToCompare[i]], epsilon))
                {
                indexOfPositionInIndexed = i;
                return true;
                }
        indexOfPositionInIndexed=-1;
        return false;
    }

    public static int Count_CompareBySqrDistance(this Vec3D[] vertices, Vec3D vertex, double epsilonSqr = 1E-9d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                count++;
        return count;
    }

    public static int Count_CompareByEach(this Vec3D[] vertices, Vec3D vertex, double epsilon = 1E-5d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                count++;
        return count;
    }
    public static bool In_CompareBySqrDistance(this ReadOnlySpan<Vec3D> vertices, Vec3D vertex, double epsilonSqr = 1E-9d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                return true;
        return false;
    }

    public static bool In_CompareByEach(this ReadOnlySpan<Vec3D> vertices, Vec3D vertex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                return true;
        return false;
    }
    public static bool In_CompareByEach(this ReadOnlySpan<Vec3D> vertices, Vec3D vertex, out int verticeIndex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                {
                verticeIndex = i;
                return true;
                }
        verticeIndex=-1;
        return false;
    }

    public static bool InIndexes_CompareByEach(this ReadOnlySpan<int> indexesToCompare, ReadOnlySpan<Vec3D> vertices, Vec3D vertex, out int indexOfPositionInIndexed, double epsilon = 1E-5d)
    {
        for (var i = 0; i < indexesToCompare.Length; i++)
            if (vertex.CloseByEach(vertices[indexesToCompare[i]], epsilon))
                {
                indexOfPositionInIndexed = i;
                return true;
                }
        indexOfPositionInIndexed=-1;
        return false;
    }

    public static int Count_CompareBySqrDistance(this ReadOnlySpan<Vec3D> vertices, Vec3D vertex, double epsilonSqr = 1E-9d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                count++;
        return count;
    }

    public static int Count_CompareByEach(this ReadOnlySpan<Vec3D> vertices, Vec3D vertex, double epsilon = 1E-5d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                count++;
        return count;
    }
    public static bool In_CompareBySqrDistance(this Vec4D[] vertices, Vec4D vertex, double epsilonSqr = 1E-9d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                return true;
        return false;
    }

    public static bool In_CompareByEach(this Vec4D[] vertices, Vec4D vertex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                return true;
        return false;
    }
    public static bool In_CompareByEach(this Vec4D[] vertices, Vec4D vertex, out int verticeIndex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                {
                verticeIndex = i;
                return true;
                }
        verticeIndex=-1;
        return false;
    }

    public static bool InIndexes_CompareByEach(this int[] indexesToCompare, Vec4D[] vertices, Vec4D vertex, out int indexOfPositionInIndexed, double epsilon = 1E-5d)
    {
        for (var i = 0; i < indexesToCompare.Length; i++)
            if (vertex.CloseByEach(vertices[indexesToCompare[i]], epsilon))
                {
                indexOfPositionInIndexed = i;
                return true;
                }
        indexOfPositionInIndexed=-1;
        return false;
    }

    public static int Count_CompareBySqrDistance(this Vec4D[] vertices, Vec4D vertex, double epsilonSqr = 1E-9d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                count++;
        return count;
    }

    public static int Count_CompareByEach(this Vec4D[] vertices, Vec4D vertex, double epsilon = 1E-5d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                count++;
        return count;
    }
    public static bool In_CompareBySqrDistance(this ReadOnlySpan<Vec4D> vertices, Vec4D vertex, double epsilonSqr = 1E-9d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                return true;
        return false;
    }

    public static bool In_CompareByEach(this ReadOnlySpan<Vec4D> vertices, Vec4D vertex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                return true;
        return false;
    }
    public static bool In_CompareByEach(this ReadOnlySpan<Vec4D> vertices, Vec4D vertex, out int verticeIndex, double epsilon = 1E-5d)
    {
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                {
                verticeIndex = i;
                return true;
                }
        verticeIndex=-1;
        return false;
    }

    public static bool InIndexes_CompareByEach(this ReadOnlySpan<int> indexesToCompare, ReadOnlySpan<Vec4D> vertices, Vec4D vertex, out int indexOfPositionInIndexed, double epsilon = 1E-5d)
    {
        for (var i = 0; i < indexesToCompare.Length; i++)
            if (vertex.CloseByEach(vertices[indexesToCompare[i]], epsilon))
                {
                indexOfPositionInIndexed = i;
                return true;
                }
        indexOfPositionInIndexed=-1;
        return false;
    }

    public static int Count_CompareBySqrDistance(this ReadOnlySpan<Vec4D> vertices, Vec4D vertex, double epsilonSqr = 1E-9d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseBySqrDistance(vertices[i], epsilonSqr))
                count++;
        return count;
    }

    public static int Count_CompareByEach(this ReadOnlySpan<Vec4D> vertices, Vec4D vertex, double epsilon = 1E-5d)
    {
        int count = 0;
        for (var i = 0; i < vertices.Length; i++)
            if (vertex.CloseByEach(vertices[i], epsilon))
                count++;
        return count;
    }
    }
}