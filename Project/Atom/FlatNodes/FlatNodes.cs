#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DoubleEngine.Atom
{
    public static partial class FlatNodes
    {
        private static readonly Dictionary<(int id, FlatNodeTransform transform), FlatNode> _allRotationsNodes;
        private static readonly Dictionary<(int id, FlatNodeTransform transform), MeshFragmentVec2D> _allRotationsFragments;
        private static readonly List<FlatNode> _allDefaultNodes;
        public static ReadOnlyCollection<FlatNode> AllDefaultNodes => _allDefaultNodes.AsReadOnly();


        public static FlatNode GetFlatNode(FlatNode deshelledSide, GridSide orientationSide) =>
            GetFlatNode((deshelledSide.id, deshelledSide.flatTransform.Transform(orientationSide.flatTransform)));

        public static FlatNode GetFlatNodeInvertedX(FlatNode deshelledSide, GridSide orientationSide) =>
            GetFlatNode((deshelledSide.id, deshelledSide.flatTransform.Transform(orientationSide.flatTransform).InvertX()));
        public static FlatNode GetFlatNodeInvertedY(FlatNode deshelledSide, GridSide orientationSide) =>
            GetFlatNode((deshelledSide.id, deshelledSide.flatTransform.Transform(orientationSide.flatTransform).InvertY()));



        public static FlatNode GetFlatNode(int id, FlatNodeTransform transform) =>
            GetFlatNode((id, transform));
        public static FlatNode GetFlatNode((int id, FlatNodeTransform transform) key) =>
            _allRotationsNodes.TryGetValue(key, out var node) ? node : throw new KeyNotFoundException();

        private static int MaxId { get { return _allDefaultNodes.Select(x => x.id).DefaultIfEmpty().Max(); } }
        /*public static void Add(int id, MeshFragment form)
        {
            if (TryFind(id, out var _))
            {
                throw new ArgumentException($"Flat node with id:{id} already exists");
            }

            FlatNode newDefaultTransformNode = new FlatNode(id, form);
            _allDefaultNodes.Add(newDefaultTransformNode);
            AddAllRotations(newDefaultTransformNode);// id, form);
        }*/
        private static void CheckExistingAndAddAnotherWithDefaultTransform(FlatNode flatNodeWithDefaultTransform)
        {
            //if(flatNodeWithDefaultTransform == null || flatNodeWithDefaultTransform.id==0)
            //    return;
            if (flatNodeWithDefaultTransform == null)
                throw new ArgumentNullException(nameof(flatNodeWithDefaultTransform));
            if (flatNodeWithDefaultTransform.flatTransform != FlatNodeTransform.Default)
                throw new ArgumentException("to add all rotations flat node must be in default transform");
            if (TryFind(flatNodeWithDefaultTransform.id, out var _))
            {
                throw new ArgumentException($"Flat node with id:{flatNodeWithDefaultTransform.id} already exists");
            }
            AddAnotherWithDefaultTransform2(flatNodeWithDefaultTransform);
        }

        //after all tests rename to AddAnotherWithDefaultTransform
        private static void AddAnotherWithDefaultTransform2(FlatNode flatNodeWithDefaultTransform)
        {
            if (flatNodeWithDefaultTransform.id != _allDefaultNodes.Count)
                throw new Exception("Trying add flatnode in wrong order");
            _allDefaultNodes.Add(flatNodeWithDefaultTransform);
            AddAllRotations(flatNodeWithDefaultTransform);
        }

        private static void AddAllRotations(FlatNode flatNodeWithDefaultTransform) //need test
        {
            if (flatNodeWithDefaultTransform.flatTransform != FlatNodeTransform.Default)
                throw new ArgumentException("to add all rotations flat node must be in default transform");

            foreach (var flatTransform in FlatNodeTransform.allFlatNodeTransforms)
            {
                if (flatTransform == FlatNodeTransform.Default)
                {
                    _allRotationsNodes.Add((flatNodeWithDefaultTransform.id, flatNodeWithDefaultTransform.flatTransform), flatNodeWithDefaultTransform);
                    _allRotationsFragments.Add((flatNodeWithDefaultTransform.id, flatNodeWithDefaultTransform.flatTransform), flatNodeWithDefaultTransform.Transformed2D);
                }
                else
                {
                    FlatNode newRotation = flatNodeWithDefaultTransform.TransformedByFlatNodeTransform(flatTransform);
                    if (newRotation.flatTransform != flatTransform)
                        throw new Exception("something went terrible wrong rotating FlatTransform");
                    _allRotationsNodes.Add((newRotation.id, newRotation.flatTransform), newRotation);
                    _allRotationsFragments.Add((newRotation.id, newRotation.flatTransform), newRotation.Transformed2D);
                }
            }
        }

        private static void Clear()
        {
            _allRotationsNodes.Clear();
            _allRotationsFragments.Clear();
            _allDefaultNodes.Clear();
        }

        /*private static void AddAllRotations(int id, MeshFragment form) //need test
        {
            foreach (var flatTransform in FlatNodeTransform.allFlatNodeTransforms)
                _allRotationsFragments.Add((id, flatTransform), FlatNode.ApplyTransformationToFragment(form, flatTransform));
        }*/

        public static int CreateNewAndAdd(MeshFragmentVec3D form)
        {
            try
            {
                return CreateNewAndAdd(ConvertMesh2DtoOnePolyVec2D(form.To2D()).TriangulateSinglePolyWithoutHoles());
            }
            catch (Exception ex)
            {
                //Debug.Log($"Error Creating new and adding FlatNode: mesh vertices length: {form.vertices.Length}, triangles: {form.triangles.Length}");
                //Debug.LogException(ex);
                throw;
            }
        }


        public static int CreateNewAndAdd(MeshFragmentVec2D form)
        {
            var newId = MaxId + 1;
            FlatNode newDefaultTransformNode = new FlatNode(newId, form);
            CheckExistingAndAddAnotherWithDefaultTransform(newDefaultTransformNode);// newId, form);
            return newId;
        }
        //public static bool TryFind(int id, [MaybeNullWhen(returnValue: false), NotNullWhen(returnValue: true)] out FlatNode node)

        public static Vec2D[] ConvertMesh2DtoOnePolyVec2D(MeshFragmentVec2D mesh)
        {
            mesh = mesh.JoinedClosestVerticesIfNeeded();
            IndexedPolyVec2D fullMeshPoly = IndexedPolyVec2D.CreateIndexedPolyVec2D(mesh);
            if (fullMeshPoly._slivers.Length != 1)
                throw new ArgumentException("fullMeshPoly._slivers.Length != 1");
            if (fullMeshPoly._slivers[0]._holes != null && fullMeshPoly._slivers[0]._holes.Length != 0)
                throw new ArgumentException("fullMeshPoly._slivers[0]._holes != null && fullMeshPoly._slivers[0]._holes.Length != 0");

            Vec2D[] poly = fullMeshPoly._slivers[0]._poly.CornersVec2D(fullMeshPoly.GetVertices());
            using (PoolListVec2D simplifiedPoly = new Collections.Pooled.PooledList<Vec2D>(poly))
            {
                simplifiedPoly.RemoveMiddlePointsThatLieOnSameLineOfPoly();
                poly = simplifiedPoly.ToArray();
            }
            return poly;
        }

        public static bool TrySlowlyFindWithSameVertexPositions(MeshFragmentVec2D mesh, out int id, out FlatNodeTransform transform)
        {
            FlatNode flatNode;
            bool result = TrySlowlyFindWithSameVertexPositions(mesh, out flatNode);
            id = flatNode.id;
            transform = flatNode.flatTransform;
            return result;
        }
        public static bool TrySlowlyFindWithSameVertexPositions(MeshFragmentVec2D mesh, out FlatNode found) //need through testing
        {
            found = FlatNode.Empty;
            //id = 0;
            //transform = FlatNodeTransform.Default;
            if (mesh is null || mesh == MeshFragmentVec2D.Empty)
                return true; //for empty mesh return empty (id 0) transform

            Vec2D[] poly;
            try
            {
                poly = ConvertMesh2DtoOnePolyVec2D(mesh);
            }
            catch
            {
                return false;
            }

            foreach (var (key, value) in _allRotationsFragments)
            {
                if (poly.Length == value.vertices.Length)//mesh.vertices.CloseEnough(value.vertices, 0.00001d))
                {
                    IndexedPolyVec2D existingFlatNodeIndexedPolyVec2D = IndexedPolyVec2D.CreateIndexedPolyVec2D(value);
                    Vec2D[] flatNodePoly = existingFlatNodeIndexedPolyVec2D._slivers[0]._poly.CornersVec2D(existingFlatNodeIndexedPolyVec2D.GetVertices());
                    if (poly.PolyVec2DShiftedEqual(flatNodePoly))
                    {
                        found = _allRotationsNodes[(key.id, key.transform)];
                        //id = key.id;
                        //transform = key.transform;
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool TryFind(int id, [MaybeNullWhen(false)] out FlatNode node)
        {
            for (var i = 0; i < _allDefaultNodes.Count; i++)
                if (_allDefaultNodes[i].id == id)
                {
                    node = _allDefaultNodes[i];
                    return true;
                }
            //node = null!;
            node = null;
            return false;
            //throw new ArgumentException($"no element with {id}");
        }

        static FlatNodes()
        {
            _allRotationsNodes = new();
            _allRotationsFragments = new();
            _allDefaultNodes = new();

            Reload();
            //LoadFromJsonFile();
        }
        public static void Reload()
        {
            ReloadFromJson(Loaders.EngineLoader.LoadFlatNodes());
        }

    }

}