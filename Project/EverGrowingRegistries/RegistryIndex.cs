using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleEngine
{
    public readonly struct RegistryIndex : IEquatable<RegistryIndex>
    {
        internal readonly int Value;

        internal RegistryIndex(int value) { this.Value = value; }
        public readonly override bool Equals(object obj) => obj is RegistryIndex index && Equals(index);
        public readonly bool Equals(RegistryIndex other) => Value == other.Value;
        public readonly override int GetHashCode() => Value;
        public static bool operator !=(RegistryIndex lhs, RegistryIndex other) => lhs.Value != other.Value;
        public static bool operator ==(RegistryIndex lhs, RegistryIndex other) => lhs.Value == other.Value;
    }
}
