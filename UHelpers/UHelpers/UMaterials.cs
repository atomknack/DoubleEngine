using DoubleEngine.Atom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;


using static DoubleEngine.Atom.DEMaterials;

namespace DoubleEngine.UHelpers;

public static class UMaterials
{
    public static MaterialByte Default => 0;
    private readonly static Color32[] s_uAlbedos;
    private readonly static Material[] s_unityMaterials;
    public static ReadOnlySpan<Color32> UnsafeAlbedos() => new ReadOnlySpan<Color32>(s_uAlbedos, 0, Count);
    public static Material GetUnityMaterial(MaterialByte id) => s_unityMaterials[id];
    public static Color32 GetUnityAlbedo(MaterialByte id) => s_uAlbedos[id];
    public static Color32 ToColor32(this ColorRGBA32 color) => new Color32(color.r, color.g, color.b, color.a);
    static UMaterials()
    {
        s_uAlbedos = new Color32[256];
        s_unityMaterials = new Material[256];

        Add("black", new ColorRGBA32(0, 0, 0, 255), "Materials/Blackish");
        Add("white", new ColorRGBA32(255, 255, 255, 255), "Materials/WhiteTwinkle");
        Add("red", new ColorRGBA32(255, 0, 0, 255), "Materials/Red");
        Add("green", new ColorRGBA32(0, 255, 0, 255), "Materials/GreenSlowTrinkle");
        Add("blue", new ColorRGBA32(0, 0, 255, 255), "Materials/BlueSlowTwinkle");
        Add("yellow", new ColorRGBA32(255, 255, 0, 255), "Materials/YellowSlowTwinkle");
    }
    public static void Add(string name, ColorRGBA32 albedo, string unityMaterialName)
    {
        int added = DEMaterials.Add(name, albedo);
        s_uAlbedos[added] = albedo.ToColor32();
        s_unityMaterials[added] = Resources.Load<Material>(unityMaterialName);
    }
}