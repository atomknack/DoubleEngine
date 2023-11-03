using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CollectionLike.Pooled;

namespace DoubleEngine
{
    public partial record MeshFragmentVec3D
    {
        public static string SerializeAsOBJFormatString(MeshFragmentVec3D fragment, int facesStartFrom = 1)
        {
            if (fragment is null)
                throw new ArgumentNullException("Cannot serialize null");
            StringBuilder sb = new StringBuilder();

            var vertices = fragment.Vertices;
            foreach (Vec3D v in vertices)
            {
                sb.AppendFormat("v {0} {1} {2}\n", v.x, v.y, -v.z);
            }
            ReadOnlySpan<IndexedTri> faces = fragment.Triangles.CastToIndexedTri_ReadOnlySpan();
            for (int i = 0; i < faces.Length; ++i)
                sb.AppendFormat("f {0} {1} {2}\n", faces[i].v2 + facesStartFrom, faces[i].v1 + facesStartFrom, faces[i].v0 + facesStartFrom);
            return sb.ToString();
        }

        [Obsolete("primitive, only vertices and faces, for testing only")]
        public static MeshFragmentVec3D DeserializeFromOBJString(string obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Cannot deserialize null");

            using StringReader reader = new StringReader(obj);
            using PoolListVec3D vertices = Expendables.CreateList<Vec3D>(500);
            using PoolListIndexedTri faces = Expendables.CreateList<IndexedTri>(500);

            while (reader.Peek() != -1)
            {
                var s = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 0)
                    continue;
                switch (s[0])
                {
                    case "v":
                        vertices.Add( new Vec3D(double.Parse(s[1]), double.Parse(s[2]), - double.Parse(s[3])) );
                        break;
                    case "f":
                        faces.Add( new IndexedTri(int.Parse(s[3])-1, int.Parse(s[2])-1, int.Parse(s[1])-1) );
                        break;
                    default: 
                        throw new Exception();
                }
            }
            return MeshFragmentVec3D.CreateMeshFragment(vertices.AsReadOnlySpan(), faces.AsReadOnlySpan());
        }
    }
}
