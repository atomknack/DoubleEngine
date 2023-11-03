using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DoubleEngine.Atom
{


    public static partial class FlatNodes
    {
        //private const string FLATNODESFILENSNAME = @".\flatnodesData.json_";

#if INHOUSE
        public static void INHOUSE_SaveToJsonFile(string fileName = null)
        {
            if (fileName == null)
                fileName = Loaders.EngineLoader.DefaultLoader.FlatNodesPath;
            File.WriteAllText(fileName, JsonConvert.SerializeObject(FlatNodes._allDefaultNodes, Formatting.Indented));
        }
#endif
        private static void ReloadFromJson(string data)
        {
            try
            {
                var temp = (List<FlatNode>)JsonConvert.DeserializeObject(data, typeof(List<FlatNode>));
                if (temp != null && temp.Count > 0)
                {
                    Clear();

                    //AddAnotherWithDefaultTransform2(FlatNode.Empty);

                    foreach (FlatNode node in temp)
                        CheckExistingAndAddAnotherWithDefaultTransform(node);
                    /*_allDefaultNodes.AddRange(temp);

                        //_elements = temp;
                        foreach (var element in _allDefaultNodes)
                            AddAllRotations();// element.id, element.form);*/
                }
            }
            catch
            {
                //Debug.Log("Cannot Serialize FlatNodes json");
            }
        }
        /*
        public static void LoadFromJsonFile(string fileName = FLATNODESFILENSNAME)
        {
            string flatNodesSource;
            try
            {
                flatNodesSource = File.ReadAllText(fileName);
                var temp = (List<FlatNode>)JsonConvert.DeserializeObject(flatNodesSource, typeof(List<FlatNode>));
                if (temp != null && temp.Count > 0)
                {
                    Clear();

                    //AddAnotherWithDefaultTransform2(FlatNode.Empty);

                    foreach (FlatNode node in temp)
                        CheckExistingAndAddAnotherWithDefaultTransform(node);
                    //_allDefaultNodes.AddRange(temp);

                        //_elements = temp;
                    //    foreach (var element in _allDefaultNodes)
                    //        AddAllRotations();// element.id, element.form);
                }
            }
            catch
            {
                Debug.Log($"Cannot find FlatNodes json file: {fileName}");
                Debug.Log("Cannot Serialize FlatNodes json file");
            }
        }
*/

    }

}