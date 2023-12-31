//----------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a _MeshFragment_Constructor_Json.tt
//     Changes will be lost if the code is regenerated.
// </auto-generated>
//----------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace DoubleEngine
{
    [JsonObject(MemberSerialization.OptIn)]
    sealed public partial record MeshFragmentVec3D : IMeshFragment<Vec3D>
    {
        [JsonIgnore] internal static readonly Vec3D[] EmptyVertices = new Vec3D[0];
        [JsonIgnore] internal static readonly int[] EmptyTriangles = new int[0];

        [JsonIgnore] public static readonly MeshFragmentVec3D Empty = new MeshFragmentVec3D();

        private MeshFragmentVec3D()
        {
            this.vertices = EmptyVertices;
            this.triangles = EmptyTriangles;
            this._joinedVertices = (double) Single.MaxValue;
        }

        public bool Equals(MeshFragmentVec3D other)
        {
        if(this is null || other is null)
            return false;
        if(vertices.Length == 0 && other.vertices.Length == 0)
            return true;
        //if ( (this?.vertices?.Length ?? 0) == 0 && (other?.vertices?.Length ?? 0) == 0 )
        //    return true;
	    return EqualityComparer<Vec3D[]>.Default.Equals(vertices, other.vertices) && 
            EqualityComparer<int[]>.Default.Equals(triangles, other.triangles);
        }

        public override int GetHashCode()
        {
	        return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<Vec3D[]>.Default.GetHashCode(vertices)) * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(triangles));
        }



        [JsonProperty(Required = Required.Always)] internal readonly Vec3D[] vertices;
        //[Obsolete("if you need to fiddle with vertices then probably is something wrong")]
        [JsonIgnore] public ReadOnlySpan<Vec3D> Vertices { get {return new ReadOnlySpan<Vec3D>(vertices);} }

        [JsonProperty(Required = Required.Always)] internal readonly int[] triangles;
        //[Obsolete("if you need to fiddle with triangles then probably is something wrong")]
        [JsonIgnore] public ReadOnlySpan<int> Triangles { get {return new ReadOnlySpan<int>(triangles);} }
        [JsonIgnore] public ReadOnlySpan<IndexedTri> Faces { get {return new ReadOnlySpan<int>(triangles).CastToIndexedTri_ReadOnlySpan();} }
        

        [JsonIgnore] internal readonly double _joinedVertices = -1;

        private MeshFragmentVec3D(Vec3D[] vertices, int[] triangles)
        {
            this.vertices = vertices;//(Vector3[])vertexes.Clone();
            this.triangles = triangles;//(int[])triangles.Clone();
            Check();
        }

        [JsonConstructor]
        internal MeshFragmentVec3D(Vec3D[] vertices, int[] triangles, double joinedVertices): this(vertices, triangles)
        {
            if(vertices is null)
                vertices = EmptyVertices;
            if(triangles is null)
                triangles = EmptyTriangles;
            _joinedVertices = joinedVertices;
        }
/*
        public MeshFragmentVector3 ToMeshFragmentVector3() =>
            new MeshFragmentVector3(this);

        internal MeshFragmentVec3D(MeshFragmentVector3 from)
        {
            this.vertices = from.vertices.ToArrayVec3D();
            this.triangles = from.triangles;//(int[])triangles.Clone();
            this._joinedVertices = (double)from._joinedVertices;

            Check();
        }
*/
    }
}
