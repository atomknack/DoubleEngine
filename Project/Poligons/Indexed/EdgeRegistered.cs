using System;
using System.Collections.Generic;

namespace DoubleEngine
{
    public readonly partial struct EdgeRegistered
    {
        public EdgeVec2D ToEdgeVec2D(IRegistry<Vec2D> vertices) => new EdgeVec2D(vertices.GetItem(start), vertices.GetItem(end));
    }
}
