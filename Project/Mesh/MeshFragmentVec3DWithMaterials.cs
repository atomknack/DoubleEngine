using Collections.Pooled;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoubleEngine.Atom;
using CollectionLike;

namespace DoubleEngine
{
    [JsonObject(MemberSerialization.OptIn)]
    public record MeshFragmentVec3DWithMaterials: IMeshFragmentWithMaterials<Vec3D>
    {
        [JsonProperty(Required = Required.Always, PropertyName = "fragment")]
        public readonly MeshFragmentVec3D fragment;
        //[JsonProperty(Required = Required.Always, PropertyName = "faceMaterials")]
        public readonly MaterialByte[] faceMaterials;
        [JsonProperty(Required = Required.Always, PropertyName = "faceMaterials")]
        public int[] FaceMaterialsAsIntsArray => faceMaterials.Select(b => (int) b).ToArray();

        [JsonIgnore] public ReadOnlySpan<MaterialByte> FaceMaterials => faceMaterials.AsReadOnlySpan();
        [JsonIgnore] public ReadOnlySpan<Vec3D> Vertices => fragment.Vertices;
        [JsonIgnore] public ReadOnlySpan<int> Triangles => fragment.Triangles;
        [JsonIgnore] public ReadOnlySpan<IndexedTri> Faces => fragment.Faces;
        public static MeshFragmentVec3DWithMaterials Create(IMeshFragmentWithMaterials<Vec3D> source)=>
            Create(MeshFragmentVec3D.CreateMeshFragment(source), source.FaceMaterials);
        public static MeshFragmentVec3DWithMaterials Create(MeshFragmentVec3D fragment, ReadOnlySpan<MaterialByte> faceMaterials) =>
            new MeshFragmentVec3DWithMaterials(fragment, faceMaterials.ToArray());
        public static MeshFragmentVec3DWithMaterials Create(MeshFragmentVec3D fragment, PooledList<MaterialByte> faceMaterials) =>
            new MeshFragmentVec3DWithMaterials(fragment, faceMaterials.ToArray());

        //public void UpdateUnityMesh(Mesh unityMesh, ReadOnlySpan<Color32> albedos = default(ReadOnlySpan<Color32>)) =>
        //    MeshUtil.UpdateUnityMesh(this, ref unityMesh, albedos);

        public string SerializeAsOBJFormatString(ReadOnlySpan<DEMaterial> materials = default(ReadOnlySpan<DEMaterial>))
        {
            if (materials.Length == 0)
                materials = DEMaterials.GetAllMaterials();

            StringBuilder sb = new StringBuilder();
            sb.Append("mtllib colors.mtl\n");
            var vertices = fragment.Vertices;
            foreach (Vec3D v in vertices)
            {
                sb.AppendFormat("v {0} {1} {2}\n", v.x, v.y, -v.z);
            }
            int materialId = -1;
            ReadOnlySpan<IndexedTri> faces = fragment.Triangles.CastToIndexedTri_ReadOnlySpan();
            for (int i = 0; i < faces.Length; ++i)
            {
                if (faceMaterials[i] != materialId)
                {
                    materialId = faceMaterials[i];
                    sb.AppendFormat("usemtl {0}\n", DEMaterials.GetMaterialName((MaterialByte)materialId));
                }
                sb.AppendFormat("f {0} {1} {2}\n", faces[i].v2 + 1, faces[i].v1 + 1, faces[i].v0 + 1);
            }

            return sb.ToString();
        }

        [JsonConstructor]
        internal MeshFragmentVec3DWithMaterials(MeshFragmentVec3D fragment, MaterialByte[] faceMaterials)
        {
            if (fragment.Faces.Length != faceMaterials.Length)
                throw new ArgumentException("faceMaterials Length should be equal to fragment.Faces length");
            this.fragment = fragment;
            this.faceMaterials = faceMaterials;
        }
    }
}
