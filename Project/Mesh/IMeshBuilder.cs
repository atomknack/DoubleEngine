using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public interface IMeshBuilder<TMeshFragment, TVector, TQuaternion>
    {
        public void AddMeshFragment(TMeshFragment fragment);
        public void AddMeshFragment(TMeshFragment fragment, TVector fragmentTranslation);
        public void AddMeshFragment(TMeshFragment fragment, TQuaternion rotation, TVector fragmentTranslation);
        public void AddMeshFragment(TMeshFragment fragment, TVector scale, TQuaternion rotation, TVector fragmentTranslation);


        public TMeshFragment BuildFragment();
        public void Clear();
    }
}
