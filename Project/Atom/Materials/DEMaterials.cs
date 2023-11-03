using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public static class DEMaterials
    {
        private readonly static DEMaterial[] s_materials;

        private static int s_count;
        public static ReadOnlySpan<DEMaterial> GetAllMaterials() => new ReadOnlySpan<DEMaterial>(s_materials, 0, s_count);
        public static string GetMaterialName(MaterialByte id) => s_materials[id].name;

        public static int Count => s_count;

        //[Obsolete("TODO: Move to some external place")]
        public static bool MaterialsEqual(MaterialByte a, MaterialByte b) => a == b;
        public static MaterialByte NextMaterialId(MaterialByte materialId) => materialId.NextByteCyclic(s_count);

        static DEMaterials()
        {
            s_materials = new DEMaterial[256];
            s_count = 0;
            //Debug.Log(s_count);

            /*
            _unityMaterials = new Material[] {
            Resources.Load<Material>("Materials/Blackish"),
            Resources.Load<Material>("Materials/WhiteTwinkle"),
            Resources.Load<Material>("Materials/GreenSlowTrinkle"),
            Resources.Load<Material>("Materials/BlueSlowTwinkle"),
            Resources.Load<Material>("Materials/Red"),
            */
        }

        public static int Add(string name, ColorRGBA32 albedo)
        {
            if(s_count>250)
                throw new IndexOutOfRangeException("currently there could be only 250 different materials");
            for (int i = 0; i < s_count; i++)
            {
                if (s_materials[i].name == name)
                    throw new ArgumentException($"material with name {name} already exists");
            }
            s_materials[s_count] = new DEMaterial((MaterialByte)s_count, name, albedo);
            int current = s_count;
            ++s_count;
            return current;
        }



        public static MaterialByte ValidateMaterial(MaterialByte material)
        {
            if (material == 0)
                return material;
            int count = DEMaterials.Count;
            if (material >= count)
                throw new Exception($"Material id {material} should be less than {count}");
            return material;
        }

    }
}
