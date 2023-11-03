#if NETWORK 
using DoubleEngine.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public readonly partial struct ThreeDimensionalCell : IEquatable<ThreeDimensionalCell>
    {
        [Obsolete("Not tested")]
        public static ThreeDimensionalCell Create(NetGridCell cell) =>
    new ThreeDimensionalCell(
        cell.cellMesh,
        Grid6SidesCached.FromRotationAndScale(ScaleInversionPerpendicularRotation3.FromInt(cell.orientation))._allPositionsIndex,
        cell.material);
        [Obsolete("Not tested")]
        public readonly NetGridCell ToNetGridCell() =>
            new NetGridCell(
                meshID,
                ScaleInversionPerpendicularRotation3.ToInt(Grid6SidesCached.GetCached(grid6SidesCachedIndex)._orientation),
                material);
    }
}
#endif