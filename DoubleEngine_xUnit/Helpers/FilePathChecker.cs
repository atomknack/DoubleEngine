using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// from https://github.com/fluentassertions/fluentassertions/issues/193
namespace DoubleEngine_xUnit.Helpers
{
    public static class FilePathChecker
    {

        public static FileToTest File(string fName) => new FileToTest(fName);
        public static FileAssertion Should(this FileToTest file) => new FileAssertion { File = file };
        public class FileAssertion
        {
            public FileToTest File;
            public void NotExist(string because = "", params object[] reasonArgs)
                => System.IO.File.Exists(File.Path).Should().BeFalse(because, reasonArgs);
            public void Exist(string because = "", params object[] reasonArgs)
                => System.IO.File.Exists(File.Path).Should().BeTrue(because, reasonArgs);
        }
        public class FileToTest 
        { 
            public string Path;
            public FileToTest(string path)
            {
                Path = path;
            }
        }
    }
}
