#if TESTING
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DoubleEngine
{

public static partial class MeshUtil
{
        //public static (List<TVertice>, List<int>) old_testingOnly_RemoveUnusedFacesAndVertices<TVertice>(IReadOnlyList<int> _triangles, IReadOnlyList<TVertice> _vertexes, IEnumerable<int> newMeshFaceIdexes) where TVertice : struct
        //public static (List<TVertice>, List<int>) old_testingOnly_RemoveUnusedFacesAndVertices<TVertice>(ROSpanInt triangles, ReadOnlySpan<TVertice> vertexes, IEnumerable<int> newMeshFaceIdexes) where TVertice : struct


        [Obsolete("Only for testing new version")]
        public static (List<TVertice>, List<int>) _old_testingOnly_RemoveUnusedFacesAndVertices<TVertice>(ROSpanInt triangles, ReadOnlySpan<TVertice> vertexes, IEnumerable<int> newMeshFaceIdexes) where TVertice : struct
        {
            List<int> newVerticesIndexes = new List<int>();
            Dictionary<int, int> oldNewVerticesMap = new Dictionary<int, int>();
            int newVerticesCount = 0;
            List<int> newTriangles = new List<int>();
            if (newMeshFaceIdexes != null)
                foreach (int faceIndex in newMeshFaceIdexes)
                {
                    AddTriangleVertice(triangles[faceIndex * 3 + 0]);
                    AddTriangleVertice(triangles[faceIndex * 3 + 1]);
                    AddTriangleVertice(triangles[faceIndex * 3 + 2]);
                }
            else
                foreach (var verticeIndex in triangles)
                    AddTriangleVertice(verticeIndex);

            return (RemapVertices(vertexes), newTriangles);

            void AddTriangleVertice(int oldVerticeIndex)
            {
                if (oldNewVerticesMap.ContainsKey(oldVerticeIndex) == false)
                {
                    newVerticesIndexes.Add(oldVerticeIndex);
                    oldNewVerticesMap.Add(oldVerticeIndex, newVerticesCount);
                    newVerticesCount++;
                }
                newTriangles.Add(oldNewVerticesMap[oldVerticeIndex]);
            }

            List<TVertice> RemapVertices(ReadOnlySpan<TVertice> _vertexes)
            {
                List<TVertice> newVertices = new List<TVertice>(newVerticesIndexes.Count);
                foreach (int oldplace in newVerticesIndexes)
                    newVertices.Add(_vertexes[oldplace]);
                return newVertices;
            }
        }
        [Obsolete("Only for testing new version")]
        public static (List<TVertice>, List<int>) _old_testingOnly_RemoveUnusedFacesAndVertices<TVertice>(IReadOnlyList<int> _triangles, IReadOnlyList<TVertice> _vertexes, IEnumerable<int> newMeshFaceIdexes) where TVertice : struct
        {
        List<int> newVerticesIndexes = new List<int>();
        Dictionary<int, int> oldNewVerticesMap = new Dictionary<int, int>();
        int newVerticesCount = 0;
        List<int> newTriangles = new List<int>();
        if (newMeshFaceIdexes != null)
            foreach (int faceIndex in newMeshFaceIdexes)
            {
                AddTriangleVertice(_triangles[faceIndex * 3 + 0]);
                AddTriangleVertice(_triangles[faceIndex * 3 + 1]);
                AddTriangleVertice(_triangles[faceIndex * 3 + 2]);
            }
        else
            foreach (var verticeIndex in _triangles)
                AddTriangleVertice(verticeIndex);

            var remappedVertices = RemapVertices();
            //var faceIndexesAsString = newMeshFaceIdexes == null ? "null" : String.Join(',', newMeshFaceIdexes);
            //var vertexesAsString = String.Join(',', _vertexes.Select(x=>$"new Vec3D{x}"));
            //var remappedVerticesAsString = String.Join(',', remappedVertices.Select(x => $"new Vec3D{x}"));
            //Debug.Log($"object[] abcde = new object[] {{ new int[]{{ {String.Join(',',_triangles)} }}, new Vec3D[]{{ {vertexesAsString} }}, new int[]{{ {faceIndexesAsString} }}, new int[]{{ {String.Join(',', newTriangles)} }}, new Vec3D[]{{ {remappedVerticesAsString} }} }}");
        return (remappedVertices, newTriangles);

        void AddTriangleVertice(int oldVerticeIndex)
        {
            if (oldNewVerticesMap.ContainsKey(oldVerticeIndex) == false)
            {
                newVerticesIndexes.Add(oldVerticeIndex);
                oldNewVerticesMap.Add(oldVerticeIndex, newVerticesCount);
                newVerticesCount++;
            }
            newTriangles.Add(oldNewVerticesMap[oldVerticeIndex]);
        }

        List<TVertice> RemapVertices()
        {
            List<TVertice> newVertices = new List<TVertice>(newVerticesIndexes.Count);
            foreach (int oldplace in newVerticesIndexes)
                newVertices.Add(_vertexes[oldplace]);
            return newVertices;
        }
    }

    }

}
#endif