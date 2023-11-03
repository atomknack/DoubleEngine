using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public readonly record struct DEMaterial
    {
        public readonly MaterialByte id;
        public readonly string name;
        public readonly ColorRGBA32 albedo;

        public DEMaterial Create(MaterialByte id, string name, ColorRGBA32 albedo) => new DEMaterial(id, name, albedo);
        internal DEMaterial(MaterialByte id, string name, ColorRGBA32 albedo)
        {
            this.id = id;
            this.name = name;
            this.albedo = albedo;
        }
    }
}
