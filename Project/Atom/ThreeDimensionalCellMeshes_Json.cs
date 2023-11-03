using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoubleEngine.Atom
{
    public static partial class ThreeDimensionalCellMeshes
    {
        //private const string CELLMESHES_FileName = @".\cellMeshesData.json_";
        [JsonObject(MemberSerialization.Fields)]
        private readonly struct SerializableIntactCubeMesh
        {
            internal readonly int id;
            internal readonly IntactCubeMesh mesh;
            [JsonConstructor] internal SerializableIntactCubeMesh(int id, IntactCubeMesh mesh){ this.id = id; this.mesh = mesh;}
        }

#if INHOUSE
        public static void INHOUSE_ExportToJsonFile(string fileName = null)//CELLMESHES_FileName)
        {
            if (fileName == null)
                fileName = Loaders.EngineLoader.DefaultLoader.TDCellMeshesPath;
            SerializableIntactCubeMesh[] serializableMeshes = new SerializableIntactCubeMesh[_intactMeshes.Length];
            for(int i = 0; i < serializableMeshes.Length; ++i)
            {
                serializableMeshes[i] = new SerializableIntactCubeMesh(i, _intactMeshes[i]);
            }
            File.WriteAllText(fileName, JsonConvert.SerializeObject(serializableMeshes, Formatting.Indented));
        }
#endif

        private static IntactCubeMesh[] ReloadIntactsFromJson(string data)
        {
            IntactCubeMesh[] result;
            try
            {
                var temp = (List<SerializableIntactCubeMesh>)JsonConvert.DeserializeObject(data, typeof(List<SerializableIntactCubeMesh>));
                temp.Sort((a, b) => a.id - b.id);
                result = new IntactCubeMesh[temp.Count];
                for (int i = 0; i < temp.Count; ++i)
                {
                    if (temp[i].id != i)
                        throw new Exception($"{i} intactCube has wrong id {temp[i].id}");
                    result[i] = temp[i].mesh;
                }

            }
            catch //(Exception ex)
            {
                //Debug.Log(ex);
                throw;
            }
            return result;
        }

        /*
        private static IntactCubeMesh[] LoadIntactsFromJsonFile(string fileName = CELLMESHES_FileName)
        {
            IntactCubeMesh[] result;
            string cellMeshesSource;
            try
            {
                cellMeshesSource = File.ReadAllText(fileName);
                var temp = (List<SerializableIntactCubeMesh>)JsonConvert.DeserializeObject(cellMeshesSource, typeof(List<SerializableIntactCubeMesh>));
                temp.Sort((a, b) => a.id - b.id);
                result = new IntactCubeMesh[temp.Count];
                for (int i = 0; i < temp.Count; ++i)
                {
                    if (temp[i].id != i)
                        throw new Exception($"{i} intactCube has wrong id {temp[i].id}");
                    result[i] = temp[i].mesh;
                }

            }
            catch(Exception ex)
            {
                Debug.Log(ex);
                Debug.Log($"Cannot find or serialize CellMeshes json file: {fileName}");
                throw;
            }
            return result;
        }
        */

        private static IntactCubeMesh[] SetIntactMeshesForEmptySpaceAndFullyFilledCube(IntactCubeMesh[] intactMeshes)
        {
            var result = intactMeshes;
            if (result == null || result.Length < 2)
                result = new IntactCubeMesh[2];
            result[0] = IntactCubeMesh.Create(MeshFragmentVec3D.Empty,
                FlatNodes.GetFlatNode(0, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(0, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(0, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(0, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(0, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(0, FlatNodeTransform.Default)
                );
            string defaultCubeJson = "{\"vertices\":[{\"x\":-0.5,\"y\":0.5,\"z\":0.5},{\"x\":-0.5,\"y\":-0.5,\"z\":0.5},{\"x\":0.5,\"y\":0.5,\"z\":0.5},{\"x\":-0.5,\"y\":-0.5,\"z\":-0.5},{\"x\":0.5,\"y\":-0.5,\"z\":-0.5},{\"x\":0.5,\"y\":0.5,\"z\":-0.5},{\"x\":0.5,\"y\":-0.5,\"z\":0.5},{\"x\":-0.5,\"y\":0.5,\"z\":-0.5}],\"triangles\":[0,1,2,3,4,1,4,5,6,5,4,7,7,3,0,5,7,2,1,6,2,4,6,1,5,2,6,4,3,7,3,1,0,7,0,2]}";
            MeshFragmentVec3D defaultCube = (MeshFragmentVec3D)JsonConvert.DeserializeObject(defaultCubeJson, typeof(MeshFragmentVec3D));

            result[1] = IntactCubeMesh.Create(defaultCube,
                FlatNodes.GetFlatNode(1, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(1, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(1, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(1, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(1, FlatNodeTransform.Default),
                FlatNodes.GetFlatNode(1, FlatNodeTransform.Default)
                );
            return result;
        }
    }
}
