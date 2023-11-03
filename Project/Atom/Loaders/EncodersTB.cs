using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleEngine.Atom.Loaders;

public static class EncodersTB
{
    private static readonly byte[] Enc2BytesHeader = UTF8Encoding.UTF8.GetBytes("Enc2");
    private static readonly byte[] Enc3BytesHeader = UTF8Encoding.UTF8.GetBytes("Enc3");
    private static readonly int bytesHeaderLength = 4;
    private static readonly byte SuperSecretKey = 42;

    public static byte[] EncodeAsENC2(string text)
    {
        var encoding = UTF8Encoding.UTF8;
        //var bytes = encoding.GetBytes(text);
        int count = encoding.GetByteCount(text);
        byte[] bytes = new byte[count + bytesHeaderLength];
        WriteHeader(bytes, Enc2BytesHeader);
        encoding.GetBytes(text, 0, text.Length, bytes, bytesHeaderLength);
        SimpleXorSpan(bytes, bytesHeaderLength, SuperSecretKey);
        return bytes;
    }
    public static string DecodeAsENC2(ReadOnlySpan<byte> bytes)
    {
        var encoding = UTF8Encoding.UTF8;

        if (!bytes.Slice(0, bytesHeaderLength).SequenceEqual(Enc2BytesHeader.AsSpan().Slice(0, bytesHeaderLength)))
            throw new ArgumentException($"{nameof(bytes)} is not enc2 bytes");
        byte[] copy = bytes.Slice(bytesHeaderLength).ToArray();
        SimpleXorSpan(copy, 0, SuperSecretKey);
        return encoding.GetString(copy);
    }

    public static byte[] EncodeAsENC3(string text)
    {
        var encoding = UTF8Encoding.UTF8;
        //var bytes = encoding.GetBytes(text);
        int count = encoding.GetByteCount(text);
        byte[] bytes = new byte[count + bytesHeaderLength];
        WriteHeader(bytes, Enc3BytesHeader);
        encoding.GetBytes(text, 0, text.Length, bytes, bytesHeaderLength);
        ProgressiveXorSpan(bytes, bytesHeaderLength, SuperSecretKey);
        return bytes;
    }
    public static string DecodeAsENC3(ReadOnlySpan<byte> bytes)
    {
        var encoding = UTF8Encoding.UTF8;

        if (!bytes.Slice(0, bytesHeaderLength).SequenceEqual(Enc3BytesHeader.AsSpan().Slice(0, bytesHeaderLength)))
            throw new ArgumentException($"{nameof(bytes)} is not enc3 bytes");
        byte[] copy = bytes.Slice(bytesHeaderLength).ToArray();
        ProgressiveXorSpan(copy, 0, SuperSecretKey);
        return encoding.GetString(copy);
    }

    internal static void SimpleXorSpan(Span<byte> bytes, int start, byte xorKey)
    {
        for (var i = start; i < bytes.Length; ++i)
            bytes[i] = (byte)(bytes[i] ^ xorKey);
    }

    internal static void ProgressiveXorSpan(Span<byte> bytes, int start, byte xorKey)
    {
        int progress = 0;
        for (var i = start; i < bytes.Length; ++i)
        {
            byte key = (byte)(xorKey * progress + xorKey + progress);
            bytes[i] = (byte)(bytes[i] ^ key);
            ++progress;
        }
    }

    private static void WriteHeader(Span<byte> bytes, ReadOnlySpan<byte> header)
    {
        ReadOnlySpan<byte> slicedHeader = header.Slice(0, bytesHeaderLength);
        slicedHeader.CopyTo(bytes);
    }
}
