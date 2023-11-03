using System;
using System.Collections.Generic;
using System.Text;


//it is not working as intended - only show warnings in current build, not when library is used
//instead currently use directive #if Testing 
namespace DoubleEngine
{
#if TESTING
    public class TestingOnlyAttribute : Attribute
    {
    }
#else
    [Obsolete("Use this only in testing build")]
    public class TestingOnlyAttribute : Attribute
    {
    } 
#endif
}
