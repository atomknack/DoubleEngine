using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DoubleEngine_xUnit.Helpers
{
    //XUnit value Serializer Stub
    /// <summary>
    /// Stub because XUnit cannot serialize objects normal way
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class X<T>: IXunitSerializable
    {
        public T v;
        public X()
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            v = default(T);
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        public X(T item0)
        {
            v = item0;
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(typeof(T).Name, JsonConvert.SerializeObject(v));
        }

        public void Deserialize(IXunitSerializationInfo info) 
        {
            v = JsonConvert.DeserializeObject<T>(info.GetValue<string>(typeof(T).Name));
        }
    }
}
