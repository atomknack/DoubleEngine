using DjvuNet.Tests.Xunit;
using DoubleEngine;
using DoubleEngine_xUnit.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine_xUnit.Edges_Tests
{
    public class MultiMaterialEdges_Tests
    {
        public readonly static IEnumerable<string> MultiMateriaFilenamesStrings = Enumerable.Range(1, 8).Select(s => $"{s:00}");
        public static IEnumerable<string> WithDecimated(IEnumerable<string> names) => 
            names.Select(s => new[] {s, s+"_Decimated"}).SelectMany(x=>x); //SelectMany is for flattening
            //{ foreach (var name in names) { yield return name; yield return name + "_Decimated";}}
        
        public static IEnumerable<object[]> MultiMaterialFilenames => WithDecimated(MultiMateriaFilenamesStrings).WrapAs1Parameter();
        [DjvuTheory]
        [MemberData(nameof(MultiMaterialFilenames))]
        public void MultiMaterial_EdgesSameAsLoadedFromJson(string fileNameShort)
        {
            var path = Helpers.Application.XUnitTestCasesPath + "ColoredMeshes\\" + fileNameShort;
            IMeshFragmentWithMaterials<Vec3D> mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path + ".mesh3d12");
            EdgeIndexed[] multiMaterialAsArray = new EdgeIndexed.MultiMaterialEdges(mesh.Faces, mesh.FaceMaterials).TESTING_EdgesAsArray();
            EdgeIndexed[] expected = JsonHelpers.LoadFromJsonFile<EdgeIndexed[]>(path + "_MultiMaterial.edgesJson");
            multiMaterialAsArray.Should().Equal(expected);
        }
        [DjvuTheory]
        [MemberData(nameof(MultiMaterialFilenames))]
        public void MultiMaterial_EdgesShouldBeOnlyOneDirected(string fileNameShort)
        {
            var path = Helpers.Application.XUnitTestCasesPath + "ColoredMeshes\\" + fileNameShort;
            IMeshFragmentWithMaterials<Vec3D> mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path + ".mesh3d12");
            EdgeIndexed.MultiMaterialEdges multiMaterial = new EdgeIndexed.MultiMaterialEdges(mesh.Faces, mesh.FaceMaterials);
            EdgeIndexed[] expected = JsonHelpers.LoadFromJsonFile<EdgeIndexed[]>(path + "_MultiMaterial.edgesJson");
            foreach(EdgeIndexed edge in expected)
            {
                expected.Should().NotContain(edge.Backwards());
                multiMaterial.IsEdgeMultiMaterial(edge.start, edge.end).Should().BeTrue();
                multiMaterial.IsEdgeMultiMaterial(edge.end, edge.start).Should().BeTrue();
            }
        }

        [DjvuTheory]
        [MemberData(nameof(MultiMaterialFilenames))]
        public void MultiMaterial_EdgesShouldOnlyContainMultimaterial_TestByRandom(string fileNameShort)
        {
            var path = Helpers.Application.XUnitTestCasesPath + "ColoredMeshes\\" + fileNameShort;
            IMeshFragmentWithMaterials<Vec3D> mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path + ".mesh3d12");
            EdgeIndexed.MultiMaterialEdges multiMaterial = new EdgeIndexed.MultiMaterialEdges(mesh.Faces, mesh.FaceMaterials);
            EdgeIndexed[] expected = JsonHelpers.LoadFromJsonFile<EdgeIndexed[]>(path + "_MultiMaterial.edgesJson");

            for (int i = 0; i < 1000; i++)
            {
                int start = TestGenerators.rand.Next(20);
                int end = TestGenerators.rand.Next(start + 1, start + 20);
                if (multiMaterial.IsEdgeMultiMaterial(start, end))
                    expected.Should().Contain(new EdgeIndexed(start, end));
                else
                    expected.Should().NotContain(new EdgeIndexed(start, end));
            }
        }

        [DjvuTheory]
        [MemberData(nameof(MultiMaterialFilenames))]
        public void MultiMaterial_EdgesUsingDispose(string fileNameShort)
        {
            var path = Helpers.Application.XUnitTestCasesPath + "ColoredMeshes\\" + fileNameShort;
            IMeshFragmentWithMaterials<Vec3D> mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path + ".mesh3d12");
            EdgeIndexed.MultiMaterialEdges multiMaterial = new EdgeIndexed.MultiMaterialEdges(mesh.Faces, mesh.FaceMaterials);
            multiMaterial.TESTING_EdgesAsArray().Should().NotBeEmpty();
            multiMaterial.Dispose();
            multiMaterial.TESTING_EdgesAsArray().Should().BeEmpty();

        }

        /*
        [Theory]
        [InlineData("06")]
        [InlineData("07")]
        [InlineData("08")]

        public void OpenMeshFile_TO_Save_MultiMateriaEdgesAsJSonArray(string fileNameShort)
        {
            var path = Helpers.Application.XUnitTestCasesPath + "ColoredMeshes\\" + fileNameShort + "_Decimated";
            IMeshFragmentWithMaterials<Vec3D> mesh = JsonHelpers.LoadFromJsonFile<MeshFragmentVec3DWithMaterials>(path + ".mesh3d12");
            mesh.Vertices.Length.Should().BeGreaterThan(0);
            var multiMaterial = new EdgeIndexed.MultiMaterialEdges(mesh.Faces, mesh.FaceMaterials).TESTING_EdgesAsArray();
            JsonHelpers.SaveToJsonFile(multiMaterial, path + "_MultiMaterial.edgesJson");
        }
        */

    }
}
