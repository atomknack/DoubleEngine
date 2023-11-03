using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine.Atom
{
    public sealed class ThreeDimensionalBuilder: IDisposable, IMeshFragmentWithMaterials<Vec3D>
    {
        private InternalLayeredBuilderWithMaterials _builder;

        public ReadOnlySpan<Vec3D> Vertices => _builder.Vertices;

        public ReadOnlySpan<int> Triangles => _builder.Triangles;

        public ReadOnlySpan<IndexedTri> Faces => _builder.Faces;

        public ReadOnlySpan<byte> FaceMaterials => _builder.FaceMaterials;

        public static ThreeDimensionalBuilder Create(int lengthX, int lengthY, int lengthZ)
        {
            ThreeDimensionalBuilder created = new ThreeDimensionalBuilder( lengthX, lengthY, lengthZ);
            return created;
        }
        public void Build(IThreeDimensionalGrid grid)
        {
            _builder.Build(grid);
        }

        public void Dispose()
        {
            _builder.Dispose();
        }

        /*
public MeshFragmentVec3D BuildMesh(IThreeDimensionalGrid grid)
{
   _builder.Build(grid);
   return _builder.BuildedFragment();
}
public MeshFragmentVec3DWithMaterials BuildMeshWithMaterials(IThreeDimensionalGrid grid)
{
   _builder.Build(grid);
   return _builder.BuildedFragmentWithMaterials();
}
*/
        internal ThreeDimensionalBuilder(int lengthX, int lengthY, int lengthZ)
        {
            _builder = InternalLayeredBuilderWithMaterials.Create(lengthX, lengthY, lengthZ); ;
        }
    }
}