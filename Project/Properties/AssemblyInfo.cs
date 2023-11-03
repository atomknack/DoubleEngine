#if TESTING

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DoubleLibraryUTests")]

[assembly: InternalsVisibleTo("DoubleEngine_xUnit")]
[assembly: InternalsVisibleTo("DoubleEngine_xUnit.Events")]
[assembly: InternalsVisibleTo("DoubleEngine_xUnit.TestingInternal")]
[assembly: InternalsVisibleTo("DoubleEngine_xUnit.Pooled")]

#endif