using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DoubleEngine.Atom
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly record struct ColorRGBA32
    {
        public static readonly ColorRGBA32 black = new ColorRGBA32(0,0,0,255);
        public static readonly ColorRGBA32 red = new ColorRGBA32(255, 0, 0, 255);
        public static readonly ColorRGBA32 green = new ColorRGBA32(0, 255, 0, 255);
        public static readonly ColorRGBA32 blue = new ColorRGBA32(0, 0, 255, 255);
        public static readonly ColorRGBA32 blackTransparent = new ColorRGBA32(0, 0, 0, 0);
        public static readonly ColorRGBA32 redTransparent = new ColorRGBA32(255, 0, 0, 0);
        public static readonly ColorRGBA32 greenTransparent = new ColorRGBA32(0, 255, 0, 0);
        public static readonly ColorRGBA32 blueTransparent = new ColorRGBA32(0, 0, 255, 0);

        [FieldOffset(0)]
        public readonly int rgba;
        [FieldOffset(0)]
        public readonly byte r;
        [FieldOffset(1)]
        public readonly byte g;
        [FieldOffset(2)]
        public readonly byte b;
        [FieldOffset(3)]
        public readonly byte a;

        public static ColorRGBA32 Create(byte r, byte g, byte b, byte a = 255) => new ColorRGBA32(r, g, b, a);
        public static ColorRGBA32 Create(int rgba) => new ColorRGBA32(rgba);
        public ColorRGBA32 InvertRGB()
        {
            const int AMASK = unchecked((int)0xFF_00_00_00);
            return new ColorRGBA32((~rgba & 0x00_FF_FF_FF) | (rgba & AMASK));
        }
        public ColorRGBA32 InvertAll()
        {
            return new ColorRGBA32(~rgba);
        }
        public int ToInt() => rgba;
        public void Deconstruct(out byte r, out byte g, out byte b, out byte a)
        {
            r = this.r;
            g = this.g;
            b = this.b;
            a = this.a;
        }
        public void Deconstruct(out byte r, out byte g, out byte b)
        {
            r = this.r;
            g = this.g;
            b = this.b;
        }
        public ColorRGBA32(int rgba) : this()
        {
            this.rgba = rgba;
        }
        public ColorRGBA32(byte r, byte g, byte b, byte a) : this()
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

    }
}
