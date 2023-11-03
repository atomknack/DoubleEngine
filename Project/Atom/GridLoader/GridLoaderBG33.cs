using DoubleEngine.Atom.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoubleEngine.Atom.GridLoader
{
    public sealed class GridLoaderBG33 : IGridStreamLoader
    {
        private static readonly byte BG33SecretKey = 133;
        private static readonly byte BG33FirstByteAdditionalSecretKey = 33;

        public static void LoadGrid(IThreeDimensionalGridElementsProvider grid, string filepath)
        {
            var loader = new GridLoaderBG33();
            using var stream = File.Open(filepath, FileMode.Open);
            loader.LoadGridFromStream(grid, stream);
        }

        public void LoadGridFromStream(IThreeDimensionalGridElementsProvider grid, Stream stream)
        {
            byte[] xorAndEncoded = StreamToByteArray(stream);
            xorAndEncoded[0] ^= BG33FirstByteAdditionalSecretKey;
            EncodersTB.ProgressiveXorSpan(xorAndEncoded, 0, BG33SecretKey);
            byte[] encoded = xorAndEncoded;
            byte[] decoded = Simplefied7z.DecodeLZMA_VLE(encoded);
            using MemoryStream ms = new MemoryStream(decoded);
            var loader = new GridLoaderBinary();
            loader.LoadGridFromStream(grid, ms);
        }

        public static void SaveGrid(IThreeDimensionalGridElementsProvider grid, string path)
        {
            var loader = new GridLoaderBG33();
            using var stream = File.Open(path, FileMode.Create);
            loader.SaveGridToStream(grid, stream);
        }

        public void SaveGridToStream(IThreeDimensionalGridElementsProvider grid, Stream stream)
        {
            var loader = new GridLoaderBinary();
            using MemoryStream ms = new MemoryStream();
            loader.SaveGridToStream(grid, ms);
            byte[] raw = ms.ToArray();
            byte[] encoded = Simplefied7z.EncodeLZMA_VLE(raw);
            EncodersTB.ProgressiveXorSpan(encoded, 0, BG33SecretKey);
            byte[] encodedAndXored = encoded;
            encodedAndXored[0] ^= BG33FirstByteAdditionalSecretKey;
            stream.Write(encodedAndXored);
        }

        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream is MemoryStream)
                return ((MemoryStream)stream).ToArray();

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static MemoryStream StreamFromByteArray(byte[] bytes) => 
            new MemoryStream(bytes);
    }
}
