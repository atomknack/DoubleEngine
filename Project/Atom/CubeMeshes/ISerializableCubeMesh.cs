using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DoubleEngine.Atom
{
    //
    public interface ISerializableCubeMesh
    {
        //[JsonProperty(Required = Required.Always, PropertyName = "Mesh")]
        public MeshFragmentVec3D Mesh { get; }
        //[JsonProperty(Required = Required.Always, PropertyName = "SideIds")]
        int[] SideIds { get;}
        //[JsonProperty(Required = Required.Always, PropertyName = "SideTransforms")]
        FlatNodeTransform[] SideTransforms { get; }
    }
}
