using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionLike.Pooled;
using DjvuNet.Tests.Xunit;
using DoubleEngine;
using DoubleEngine.Atom;
using FluentAssertions;

namespace DoubleEngine_xUnit.Expendables_Tests
{
    public class Normals_Tests
    {
        [DjvuTheory]
        [MemberData(nameof(Edges_Tests.MultiMaterialEdges_Tests.MultiMaterialFilenames),
            MemberType = typeof(Edges_Tests.MultiMaterialEdges_Tests))]
        public void FaceNormals_SameAsLoadedFromJson(string fileNameShort)
        {
            var path = Helpers.Application.XUnitTestCasesPath + "ColoredMeshes\\" + fileNameShort;
            IMeshFragmentWithMaterials<Vec3D> mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path + ".mesh3d12");
            var normals = Expendables.CreateFaceNormalsFromMeshFragment3D(mesh);
            normals.Count.Should().Be(mesh.Faces.Length);
            Vec3D[] expected = JsonHelpers.LoadFromJsonFile<Vec3D[]>(path + "_FacesNormals.normalsJson");
            normals.Should().Equal(expected);
        }
        /*
        [DjvuTheory]
        [MemberData(nameof(Edges_Tests.MultiMaterialEdges_Tests.MultiMaterialFilenames),
            MemberType = typeof(Edges_Tests.MultiMaterialEdges_Tests))]
        public void CreateFaceNormals_JsonFile_FromMeshFragment3D(string fileNameShort)
        {
            var path = Helpers.Application.XUnitTestCasesPath + "ColoredMeshes\\" + fileNameShort;
            IMeshFragmentWithMaterials<Vec3D> mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path + ".mesh3d12");
            var normals = Expendables.CreateFaceNormalsFromMeshFragment3D(mesh);
            JsonHelpers.SaveToJsonFile(normals.ToArray(), path + "_FacesNormals.normalsJson");
        }
        */
    }
}
