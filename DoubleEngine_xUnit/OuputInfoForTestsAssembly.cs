using DoubleEngine;
using DoubleEngine_xUnit.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

public class OuputInfoForTestsAssembly
    {
        private readonly ITestOutputHelper _output;
        public OuputInfoForTestsAssembly(ITestOutputHelper output)
        {
            _output = output;
        }
    /*
    private static readonly Destructor Finalise = new Destructor();

    public virtual void OnDestruct()
    {
        Console.WriteLine("After All tests");
    }
    private sealed class Destructor
    {
        ~Destructor()
        {
            OnDestruct();
        }
    }*/



    [Fact]
    public void Helpers_Application_XUnitTestCasesPath_Test()
    {
        var path = Application.XUnitTestCasesPath;
        _output.WriteLine(path);
        var pathToCheckFile = path + "thisIsFileToCheck.CorrectXUnitTestCasesPath";
        _output.WriteLine(pathToCheckFile);

        FilePathChecker.File(pathToCheckFile).Should().Exist();
    }

    [Fact]
    public void Helpers_Application_Path_Test()
    {
        var expectedPath = AppDomain.CurrentDomain.BaseDirectory;
        _output.WriteLine($"{expectedPath}");
        Application.Path.Should().Be(expectedPath);
        //_output.WriteLine($"{JsonHelpers.AppendApplicationDataPath("")}");
    }
    [Fact]
    public void GetProject_Path_Test()
    {
        var path = System.Environment.CurrentDirectory;
        _output.WriteLine($"{path}");
        path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"..\..\..\"));
        _output.WriteLine($"{path}");
    }

    [Fact]
        public void DoubleEngine_Vector_TestBuildInfo()
        {
        var doubleEngine_Vector_Assembly = Assembly.GetAssembly(typeof(VectorCore.Vec3D));
        var compileDate = doubleEngine_Vector_Assembly?.Location;
        //_output.WriteLine($"{RetrieveLinkerTimestamp(compileDate)} - DoubleEngine compile date");
        _output.WriteLine($"{File.GetLastWriteTime(compileDate)} - DoubleEngine last Testing build date");
        //File.GetCreationTime(Assembly.GetExecutingAssembly().Location)
        _output.WriteLine($"{doubleEngine_Vector_Assembly.GetName().Version} - DoubleEngine version");
        //Assert.SkipIf(skip);
        //"aatbc".Should().ContainEquivalentOf("aTb");
    }

    /* not working:
    /// https://stackoverflow.com/questions/2050396/getting-the-date-of-a-net-assembly
    /// <summary>
    /// Retrieves the linker timestamp.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns></returns>
    /// <remarks>http://www.codinghorror.com/blog/2005/04/determining-build-date-the-hard-way.html</remarks>
    private static System.DateTime RetrieveLinkerTimestamp(string filePath)
    {
        const int peHeaderOffset = 60;
        const int linkerTimestampOffset = 8;
        var b = new byte[2048];
        System.IO.FileStream s = null;
        try
        {
            s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            s.Read(b, 0, 2048);
        }
        finally
        {
            if (s != null)
                s.Close();
        }
        var dt = new System.DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(System.BitConverter.ToInt32(b, System.BitConverter.ToInt32(b, peHeaderOffset) + linkerTimestampOffset));
        return dt.AddHours(System.TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
    }
    */
}
