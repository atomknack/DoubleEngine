using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoubleEngine.Atom.Loaders
{
    public static class Simplefied7z
    {
        static readonly CoderPropID[] propIDs =
{
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker
        };
        static readonly object[] properties =
        {
            (Int32)(1 << 23),//(dictionary),
            (Int32)2,//(posStateBits),
            (Int32)3,//(litContextBits),
            (Int32)0,//(litPosBits),
            (Int32)2,//(algorithm),
            (Int32)128,//(numFastBytes),
            "bt4",//mf,
            false,//eos
        };

        public static byte[] EncodeLZMA_VLE(byte[] raw)
        {
            SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
            using var inStream = new MemoryStream(raw);
            using var outStream = new MemoryStream();

            if (inStream.Length > Int32.MaxValue)
                throw new Exception($"Encode for bytes array have limit for Stream length {Int32.MaxValue}");

            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outStream);
            //outStream.Write(BitConverter.GetBytes((Int32)inStream.Length), 0, 4);
            VariableLengthEncoder.EncodeVariableLengthPositiveLong(outStream, inStream.Length);

            encoder.Code(inStream, outStream, inStream.Length, -1, null);

            return outStream.ToArray();
        }

        public static byte[] DecodeLZMA_VLE(byte[] encoded)
        {
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            using var inStream = new MemoryStream(encoded);
            using var outStream = new MemoryStream();

            byte[] properties = new byte[5];
            inStream.Read(properties, 0, 5);

            //byte[] decompressedSizeAsBytes = new byte[4];
            //inStream.Read(decompressedSizeAsBytes, 0, 4);
            //Int32 decompressedSize = BitConverter.ToInt32(decompressedSizeAsBytes, 0);
            long decompressedSize = VariableLengthEncoder.DecodeVariableLengthPositiveLong(inStream);

            decoder.SetDecoderProperties(properties);
            decoder.Code(inStream, outStream, inStream.Length, decompressedSize, null);

            return outStream.ToArray();
        }
        /*
        public static byte[] EncodeLZMA_length4bytes(byte[] raw)
        {
            SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
            using var inStream = new MemoryStream(raw);
            using var outStream = new MemoryStream();

            if (inStream.Length > Int32.MaxValue)
                throw new Exception($"Encode for bytes array have limit for Stream length {Int32.MaxValue}");

            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outStream);
            outStream.Write(BitConverter.GetBytes((Int32)inStream.Length), 0, 4);

            encoder.Code(inStream, outStream, inStream.Length, -1, null);

            return outStream.ToArray();
        }

        public static byte[] DecodeLZMA_VRLlength4bytes(byte[] encoded)
        {
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            using var inStream = new MemoryStream(encoded);
            using var outStream = new MemoryStream();

            byte[] properties = new byte[5];
            inStream.Read(properties, 0, 5);

            byte[] decompressedSizeAsBytes = new byte[4];
            inStream.Read(decompressedSizeAsBytes, 0, 4);
            Int32 decompressedSize = BitConverter.ToInt32(decompressedSizeAsBytes, 0);

            decoder.SetDecoderProperties(properties);
            decoder.Code(inStream, outStream, inStream.Length, decompressedSize, null);

            return outStream.ToArray();
        }*/
    }
}
