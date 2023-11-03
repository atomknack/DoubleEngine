using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace DoubleEngine.Atom
{

    public partial record FlatNode
    {
        [JsonIgnore] public static readonly FlatNode Empty = new FlatNode(0, MeshFragmentVec2D.Empty);

        public readonly int id;
        public readonly FlatNodeTransform flatTransform;
        public readonly MeshFragmentVec2D form;
        [JsonIgnore]
        public readonly EdgeIndexed[] singleEdges;
        [JsonIgnore]
        public MeshFragmentVec3D Transformed => _transformed ??= To3D(ApplyTransformationToFragment());
        [JsonIgnore]
        private MeshFragmentVec3D _transformed = null;

        public static MeshFragmentVec3D To3D(MeshFragmentVec2D mesh2D) => mesh2D.To3D(-0.5);

        [JsonIgnore]
        public MeshFragmentVec2D Transformed2D => ApplyTransformationToFragment();

        [JsonConstructor]
        public FlatNode(int id, MeshFragmentVec2D form): this(id, form, 1e-05f) {}
        private FlatNode(int id, MeshFragmentVec2D form, FlatNodeTransform transform) : this(id, form, 1e-05f)
        {
            //this.id = id;
            this.flatTransform = transform;
            //this.form = form;
            //this._transformed = null;
        }
        private FlatNode (int id, MeshFragmentVec2D form, float joinVerticesEpsilon = 1e-05f)
        {
            this.id = id;
            this.flatTransform = FlatNodeTransform.Default;
            this.form = form.JoinedClosestVerticesIfNeeded(joinVerticesEpsilon);
            this._transformed = null;
            this.singleEdges = EdgeIndexed.SingleEdgesFromTriangles(this.form.triangles);
            //Debug.Log($"{this.id} {this.flatTransform} single Edges Tuple Length {singleEdges.Length}");
            //this.singleEdges = MeshUtil.SingleEdgesToArray(singleEdgesTuples);
            //Debug.Log($"single Edges array Length {singleEdges.Length}");
        }




        ////////////////////////////////////////////

        public FlatNode TransformedByFlatNodeTransform(FlatNodeTransform applyTransformation) =>
            new (id, form, flatTransform.Transform(applyTransformation));

        public FlatNode InvertX()=> new (id, form, flatTransform.InvertX()); //Not Tested
        public FlatNode InvertY() => new (id, form, flatTransform.InvertY());//Not Tested
        public FlatNode Rotate(PerpendicularAngle angle)=>new (id, form, flatTransform.Rotate(angle)); //Not Tested


        private MeshFragmentVec2D ApplyTransformationToFragment()
        {
            return ApplyTransformationToFragment(form, flatTransform);
        }


        public static MeshFragmentVec2D ApplyTransformationToFragment(MeshFragmentVec2D f, FlatNodeTransform t)
        {
            MeshFragmentVec3D temp=f.To3D(-0.5);
            if (t.inverted)
                temp = temp.Scaled(new Vec3D(-1, 1, 1));
            //else temp = f;
            temp = temp.Rotated(QuaternionD.Euler(0, 0, t.rotation.Float()));
            return temp.To2D();
        }

    }
}
