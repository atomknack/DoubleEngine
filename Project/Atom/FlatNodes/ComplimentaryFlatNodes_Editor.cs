using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace DoubleEngine.Atom
{

    public static class ComplimentaryFlatNodes_Editor
    {
        public record ComplimentaryFlatNode
        {
            public readonly FlatNode current;
            public readonly FlatNode complimentary;
            public readonly MeshFragmentVec3D fragmentToAddToMesh;
            public readonly Vec3D[] splitterVertices; //todo: make this functionality
        }

        private readonly static List<ComplimentaryFlatNode> _elements = new();
        private readonly static HashSet<(FlatNode,FlatNode)> _set = new();
        public static ReadOnlyCollection<ComplimentaryFlatNode> DefaultToComplimentaryNodes => _elements.AsReadOnly();

        private const string ComplimentaryFlatnodesFILENSNAME_Editor = @".\ComplimentaryFlatNode_Editor_Data.json_";

        private static (FlatNode,FlatNode) ToTuple(ComplimentaryFlatNode node) => (node.current,node.complimentary);
        private static void Clear()
        {
            _elements.Clear();
            _set.Clear();
        }

        public static void Add(ComplimentaryFlatNode node)
        {
            if (_set.Contains(ToTuple(node)))
            {
                _set.Remove(ToTuple(node));
                _elements.RemoveAll(x=>x.current==node.current && x.complimentary==node.complimentary);
            }
            AddNew(node);
        }
        private static void AddNew(ComplimentaryFlatNode node)
        {
            if (node.current.flatTransform != FlatNodeTransform.Default)
                throw new ArgumentException("Edited node current must be FlatNodeTransform.Default");
            if (_set.Contains(ToTuple(node)))
                throw new ArgumentException("trying to add duplicate node");

            _elements.Add(node);
            _set.Add(ToTuple(node));
        }

        public static void SaveToJsonFile()
        {
            File.WriteAllText(ComplimentaryFlatnodesFILENSNAME_Editor, JsonConvert.SerializeObject(_elements, Formatting.Indented));
        }
        public static void LoadFromJsonFile()
        {
            string ComplimentaryflatNodesSource;
            try
            {
                ComplimentaryflatNodesSource = File.ReadAllText(ComplimentaryFlatnodesFILENSNAME_Editor);
                var tempList = (List<ComplimentaryFlatNode>)JsonConvert.DeserializeObject(ComplimentaryflatNodesSource, typeof(List<ComplimentaryFlatNode>));
                if (tempList != null && tempList.Count > 0)
                {
                    Clear();
                    foreach (var temp in tempList)
                        Add(temp);
                }
            }
            catch
            {
                //Debug.Log("Cannot find ComplimentaryFlatNode_Editor json file");
                //Debug.Log("Cannot Serialize ComplimentaryFlatNode_Editor json file");
            }
        }
    }

}