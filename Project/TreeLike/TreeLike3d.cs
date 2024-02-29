using DoubleEngine.Atom;
using JsonKnownTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Linq;


namespace DoubleEngine.TreeLike;


[JsonConverter(typeof(JsonKnownTypesConverter<BaseBranch>))]
[JsonDiscriminator(Name = "branchType")] 
public abstract record BaseBranch
{
    public readonly int Id;

    protected BaseBranch(int id)
    {
        Id = id;
    }
}

public sealed record OneOutBranch : BaseBranch
{
    public readonly MeshFragmentVec3D mesh;
    public readonly int branchInFlatNodeId;

    public readonly int branchOutFlatNodeId;
    public readonly TRS3D branchOutTRS;

    [JsonConstructor]
    public OneOutBranch(int Id, MeshFragmentVec3D mesh, int branchInFlatNodeId, 
        int branchOutFlatNodeId, TRS3D branchOutTRS): base(Id)
    {
        this.mesh = mesh;
        this.branchInFlatNodeId = branchInFlatNodeId;
        this.branchOutFlatNodeId = branchOutFlatNodeId;
        this.branchOutTRS = branchOutTRS;
    }
}

public sealed record ThreeOutBranch : BaseBranch
{
    public readonly MeshFragmentVec3D mesh;
    public readonly int branchInFlatNodeId;

    public readonly int branchOutFlatNodeId_0;
    public readonly TRS3D branch0OutTRS_0;
    public readonly int branchOutFlatNodeId_1;
    public readonly TRS3D branch1OutTRS_1;
    public readonly int branchOutFlatNodeId_2;
    public readonly TRS3D branchOutTRS_2;

    [JsonConstructor]
    public ThreeOutBranch(int Id, MeshFragmentVec3D mesh, int branchInFlatNodeId, 
        int branchOutFlatNodeId_0, TRS3D branch0OutTRS_0, 
        int branchOutFlatNodeId_1, TRS3D branch1OutTRS_1, 
        int branchOutFlatNodeId_2, TRS3D branchOutTRS_2) : base(Id)
    {
        this.mesh = mesh;
        this.branchInFlatNodeId = branchInFlatNodeId;
        this.branchOutFlatNodeId_0 = branchOutFlatNodeId_0;
        this.branch0OutTRS_0 = branch0OutTRS_0;
        this.branchOutFlatNodeId_1 = branchOutFlatNodeId_1;
        this.branch1OutTRS_1 = branch1OutTRS_1;
        this.branchOutFlatNodeId_2 = branchOutFlatNodeId_2;
        this.branchOutTRS_2 = branchOutTRS_2;
    }
}