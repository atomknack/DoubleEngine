using System;
using System.IO;

namespace DoubleEngine.Atom.Loaders
{
    internal static class VariableLengthEncoder
    {

        public static long DecodeVariableLengthPositiveLong(this Stream stream)//, Action<string> act)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            int readed;
            ulong value = 0;
            int shift = 0;

            do
            {
                readed = stream.ReadByte();
                if (readed < 0)
                    throw new ArgumentOutOfRangeException($"Cannot Decode Variable Length Long from stream {nameof(stream)}, stream prematurely ended");
                byte lower7bits = (byte)readed;
                value |= ((ulong)Lower7bits(readed)) << shift;
                shift += 7;
                //act($"{value}  {shift} {lower7bits} {Is8thBit(readed)} {readed}");
                if (value>long.MaxValue)
                    throw new ArgumentOutOfRangeException($"This metod could decode number no more than {long.MaxValue}, but it already is {value}");
            } while (Is8thBit(readed));
            return (long)value;
        }

        public static void EncodeVariableLengthPositiveLong(this Stream stream, long value)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (value < 0)
                throw new ArgumentOutOfRangeException($"{nameof(value)}: {value}, must NOT be negative");

            do
            {
                byte lower7bits = (Lower7bits(value));
                value >>= 7;
                if (value > 0)
                    lower7bits |= 128;
                stream.WriteByte(lower7bits);
            } while (value > 0);
        }
        private static bool Is8thBit(int value) => (value & 0b1000_0000) != 0;
        private static byte Lower7bits(long value) => (byte)(value & 0b0111_1111);
    }
}