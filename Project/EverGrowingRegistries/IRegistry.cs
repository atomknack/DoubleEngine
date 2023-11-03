using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public interface IRegistry<T>
    {
        public RegistryIndex GetOrAdd(T item);
        public T GetItem(RegistryIndex index);
        public ReadOnlySpan<T> Snapshot();

        public T[] AssembleIndices(ReadOnlySpan<RegistryIndex> indexes);
        public T[] AssembleIndices(RegistryIndex[] indexes);
        public T[] AssembleIndices(List<RegistryIndex> indexes);
    }
}
