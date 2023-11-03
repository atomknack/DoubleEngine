using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    [Obsolete("Dropping support of internal structures that use Unity types")]
    public partial record MeshFragmentVector3 { }

    [Obsolete("Dropping support of internal structures that use Unity types", true)]
    public readonly partial struct TriVector3 { }
    [Obsolete("Dropping support of internal structures that use Unity types", true)]
    public readonly partial struct TriVector2 { }


    [Obsolete("Dropping support of internal structures that use Unity types", true)]
    public readonly partial struct EdgeVector2 { }

    [Obsolete("Dropping support of internal structures that use Unity types", true)]
    public readonly partial struct EdgeVector3 { }
}
