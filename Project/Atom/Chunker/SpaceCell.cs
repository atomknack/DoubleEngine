using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public readonly struct SpaceCell
    {
        public readonly Vec3I coords;
        public readonly ThreeDimensionalCell cell;

        public SpaceCell(Vec3I coords, ThreeDimensionalCell cell)
        {
            this.coords = coords;
            this.cell = cell;
        }
    }
}
