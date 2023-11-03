using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;



namespace DoubleEngine
{
    using RegIndex = RegistryIndex;//EverGrowingVec3DVector3.RegistryIndex;

    public class MeshRegisteredBuilderVec3D : IMeshBuilder<MeshFragmentVec3D, Vec3D, QuaternionD> //need testing, not tested
    {
        private static ThreadLocal<List<RegIndex>> _registeredVerticesBuffer_For_RegisterVertices = 
            new ThreadLocal<List<RegIndex>>( ()=>new List<RegIndex>() );
        private static ThreadLocal<RemapperRegistryIndex> _remapper_For_Build = 
            new ThreadLocal<RemapperRegistryIndex>( ()=> new RemapperRegistryIndex() );
        private List<RegIndex> _registeredTriangles;


        private static EverGrowingVec3DVec3F register = EverGrowingVec3DVec3F.Shared;

        public MeshRegisteredBuilderVec3D()
        {
            _registeredTriangles = new List<RegIndex>();
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment)
        {
            List<RegIndex> verticesBuffer = ThreadInstancedClearedRegisteredVerticesBuffer();
            RegisterVertices(fragment.vertices, verticesBuffer);
            AddTriangles(fragment.triangles, verticesBuffer);
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment, Vec3D fragmentTranslation)
        {
            throw new NotImplementedException();
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment, QuaternionD rotation, Vec3D fragmentTranslation)
        {
            throw new NotImplementedException();
        }
        public void AddMeshFragment(MeshFragmentVec3D fragment, Vec3D scale, QuaternionD rotation, Vec3D fragmentTranslation)
        {
            throw new NotImplementedException();
        }


        public MeshFragmentVec3D BuildFragment() => BuildMeshFragmentVec3D();
        public MeshFragmentVec3D BuildMeshFragmentVec3D() //TODO: make copy for BuildMeshFragmentVector3
        {
            RemapperRegistryIndex remapper = ThreadInstancedRemapper();
            remapper.AddMany(_registeredTriangles);
            
            int[] newTriangles = remapper.RemapExistingItems(_registeredTriangles);
            Vec3D[] newVertices = remapper.BuildRemappedFromRegister<Vec3D>(register);
            return new MeshFragmentVec3D(newVertices, newTriangles, EverGrowingVec3DVec3F.EPSILON);
        }

        public void Clear()
        {
            _registeredTriangles.Clear();
        }
        private void AddTriangleTip(RegIndex vertexRegisterIndex)
        {
            //_remapper.Add(vertexRegisterIndex);
            _registeredTriangles.Add(vertexRegisterIndex);
        }
        private void AddRegisteredTriangles(
            RegIndex v0, 
            RegIndex v1, 
            RegIndex v2)
        {
            if ((v0 != v1) && (v1 != v2) && (v2 != v0))
            {
                AddTriangleTip(v0);
                AddTriangleTip(v1);
                AddTriangleTip(v2);
            }
        }
        private void AddTriangles(int[] triangles, List<RegIndex> verticesBuffer)
        {
            for (int i = 0; i < triangles.Length; i += 3)
                AddRegisteredTriangles(verticesBuffer[triangles[i]], verticesBuffer[triangles[i + 1]], verticesBuffer[triangles[i + 2]]);
        }
        private static void RegisterVertices(Vec3D[] vertices, List<RegIndex> verticesBuffer)
        {
            for (int i = 0; i < vertices.Length; i++)
                verticesBuffer.Add(register.GetOrAdd(vertices[i]));
        }

        private static List<RegIndex> ThreadInstancedClearedRegisteredVerticesBuffer()
        {
            List<RegIndex> registeredVerticesBuffer = _registeredVerticesBuffer_For_RegisterVertices.Value;
            registeredVerticesBuffer.Clear();
            return registeredVerticesBuffer;
        }
        private static RemapperRegistryIndex ThreadInstancedRemapper()
        {
            RemapperRegistryIndex remapper = _remapper_For_Build.Value;
            remapper.Clear_ToUseWithRelativerySameNumberOfItems();
            return remapper;
        }
    }
}
