using Xunit;
using SevenZip;
using SevenZip.Compression.LZMA;
using System.Text;
using Xunit.Abstractions;

namespace DoubleEngine_xUnit.Encoders;

public class Lzma2201Tests
{
    private readonly ITestOutputHelper _output;
    public Lzma2201Tests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void EncodeDecodeExampleStringAsBytes()
    {
        string s = "Hello world 8, wow, 4 Double 2 wow,  5 4 wow, wowow!@#$%^&*";
        _output.WriteLine(s);
        byte[] inBytes = Encoding.UTF8.GetBytes(s);
        _output.WriteLine(BytesJoinedAsString(inBytes));
        byte[] encoded = DoubleEngine.Atom.Loaders.Simplefied7z.EncodeLZMA_VLE(inBytes);
        _output.WriteLine(BytesJoinedAsString(encoded));
        byte[] decoded = DoubleEngine.Atom.Loaders.Simplefied7z.DecodeLZMA_VLE(encoded);
        _output.WriteLine(BytesJoinedAsString(decoded));
        Assert.Equal(decoded.Length, inBytes.Length);
        Assert.Equal(decoded, inBytes);
        _output.WriteLine(Encoding.UTF8.GetString(decoded));
    }



    public static string BytesJoinedAsString(byte[] inBytes) => String.Join(',', inBytes);

    /*
    public static Stream GenerateStreamFromString_Imperative(string s)
    {
        var memoryStream = new MemoryStream();
        var writer = new StreamWriter(memoryStream);
        writer.Write(s);
        writer.Flush();
        memoryStream.Position = 0;
        return memoryStream;
    }
    */
}
