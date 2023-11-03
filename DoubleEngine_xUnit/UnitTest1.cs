using DoubleEngine;
using DoubleEngine.AtomEvents;

namespace DoubleEngine_xUnit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var a = true;
            a = !(!a);
            Assert.Equal(true, a);
        }

        [Theory]
        [InlineData(8,3,5)]
        [InlineData(-100,200,100)]
        public void LoggedMethod_Test_Test(int a, short b, long d)
        {
            //QuaternionD q = QuaternionD
            //var logger = new DoubleEngine.Helpers.MethodLoggerToFile(DoubleEngine_xUnit.Helpers.Application.Path, "testLogTest");
            //logger.StartLogMethod(a, b, d);
            //logger.EndLogMethod(new Dictionary<string, object>());
        }
    }
}