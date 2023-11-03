using DoubleEngine.Atom;
using System;


namespace DoubleEngine.Network
{
    public readonly struct NetGridCell: IEquatable<NetGridCell>
    {
        public static readonly NetGridCell Empty = new NetGridCell(0, 0, 0);

        public readonly short cellMesh; // 0 empty //negatives: special	//-32,768 .. 32,767
        public readonly int orientation; //ScaleInversionPerpendicularRotation3
        public readonly byte material; //0..255

        public readonly override string ToString() => $"NetGridCell({cellMesh}, {orientation}, {material})";

        public NetGridCell(short cellMesh, int orientation, byte material)
        {
            ValidateParameters(cellMesh, orientation, material);
            this.cellMesh = cellMesh;
            this.orientation = orientation;
            this.material = material;
        }

        public static void ValidateParameters(short cellMesh, int orientation, byte material)
        {
            if (!ScaleInversionPerpendicularRotation3.IsValid(orientation))
                throw new Exception($"orientation {orientation} is not valid");
            ThreeDimensionalCellMeshes.ValidateMeshId(cellMesh);
            DEMaterials.ValidateMaterial(material);
        }

        public bool Equals(NetGridCell other) => 
            cellMesh == other.cellMesh && 
            orientation == other.orientation && 
            material == other.material;
    }
}