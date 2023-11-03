using System;

namespace DoubleEngine.Atom
{


    public static partial class FlatNodes
    {
        private enum FlatNodeFilter : int
        {
            DoubleRound = 10,
            DeleteNode = 32
        }
        public static void ApplyFilterToBase(int index, int filterInt)
        {
            FlatNodeFilter filter = (FlatNodeFilter)filterInt;
            if (index < 0 || index >= _allDefaultNodes.Count)
                throw new ArgumentException($"{nameof(index)}: {index} is outside 0..{_allDefaultNodes.Count - 1}");
            switch (filter)
            {
                case FlatNodeFilter.DoubleRound:
                    FilterDoubleRound(index);
                    break;
                case FlatNodeFilter.DeleteNode:
                    FilterDeleteNode(index);
                    break;
                default:
                    throw new ArgumentException($"unknown filter {filterInt}");
            }
        }
        private static void FilterDeleteNode(int index)
        {
            _allDefaultNodes.RemoveAt(index);
        }
        private static void FilterDoubleRound(int index)
        {
            var node = _allDefaultNodes[index];
            _allDefaultNodes[index] = new FlatNode(node.id, 
                MeshFragmentVec2D.CreateMeshFragment(node.form.vertices.AsSpan().ToArrayTwiceRoundedVec2D(), node.form.triangles));
        }
    }

}