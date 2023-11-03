using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    //special: 0 empty
    //-1..-255 fullyFilledCubes, negative index is x coord of origin complex model, grid6SidesCachedIndex is y coord and material is z coord
    public readonly partial struct ThreeDimensionalCell: IEquatable<ThreeDimensionalCell>
    {
        public static readonly MaterialByte DefaultMaterial = 0;
        public static readonly ThreeDimensionalCell Empty = new ThreeDimensionalCell(0,0,0);

        internal readonly short meshID; // 0 empty //negatives: special	//-32,768 .. 32,767
        internal readonly byte grid6SidesCachedIndex;//0..255
        internal readonly MaterialByte material; //0..255
        public short GetMeshId() => meshID;

        public readonly override string ToString() => $"Cell(id:{meshID},rotation:{grid6SidesCachedIndex},material:{material},hash:{GetHashCode()})";
        public bool HasNotEmptyMesh() => meshID != 0;
        public static ThreeDimensionalCell Create(short meshID, byte Grid6SidesCachedIndex, byte material = 0) => 
            new ThreeDimensionalCell(meshID, Grid6SidesCachedIndex, material);
        public static ThreeDimensionalCell Create(short meshID, ScaleInversionPerpendicularRotation3 orientation, byte material = 0) =>
            new ThreeDimensionalCell(meshID, Grid6SidesCached.FromRotationAndScale(orientation)._allPositionsIndex, material);

        private ThreeDimensionalCell(short meshID, byte Grid6SidesCachedIndex, byte material)
        {
            this.meshID = meshID;
            this.grid6SidesCachedIndex = Grid6SidesCachedIndex;
            this.material = material;
        }

        public bool Equals(ThreeDimensionalCell other) =>
            meshID == other.meshID && grid6SidesCachedIndex == other.grid6SidesCachedIndex && material == other.material;
        public static bool operator !=(in ThreeDimensionalCell lhs, in ThreeDimensionalCell rhs) => !(lhs == rhs);
        public static bool operator ==(in ThreeDimensionalCell lhs, in ThreeDimensionalCell rhs) => lhs.Equals(rhs);
        public readonly override int GetHashCode()
        {
            int hash = 5903;
            hash = ((hash << 7) ^ hash) ^ material;
            hash = ((hash << 9) ^ hash) ^ grid6SidesCachedIndex;
            hash = ((hash << 5) ^ hash) ^ meshID;
            return hash;
        }
        public readonly override bool Equals(object other)
        {
            //Debug.Log("object equals was called");
            if (!(other is ThreeDimensionalCell))
            {
                return false;
            }
            return Equals((ThreeDimensionalCell)other);
        }
    }
}
