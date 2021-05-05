using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using Neo.VM.Types;
using Neo.IO.Json;
using System.Linq;
using System.Text;

namespace Neo.VM
{
    public static class MyHelper
    {
        internal sealed class SpencersReferenceEqualityComparer : IEqualityComparer, IEqualityComparer<object>
        {
            public static readonly SpencersReferenceEqualityComparer Default = new SpencersReferenceEqualityComparer();

            private SpencersReferenceEqualityComparer()
            {
            }

            public new bool Equals(object x, object y)
            {
                return x == y;
            }

            public int GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }


        public static SJObject ToSJson(this StackItem item)
        {
            return ToSJson(item, null);
        }

        private static SJObject ToSJson(StackItem item, HashSet<StackItem> context)
        {
            SJObject json = new SJObject();
            json["type"] = item.Type;
            switch (item)
            {
                case Array array:
                    context ??= new HashSet<StackItem>(SpencersReferenceEqualityComparer.Default);
                    if (!context.Add(array)) break;
                    json["value"] = new SJArray(array.Select(p => ToSJson(p, context)));
                    break;
                case Boolean boolean:
                    json["value"] = boolean.GetBoolean();
                    break;
                case Buffer _:
                case ByteString _:
                    json["value"] = Encoding.UTF8.GetString(item.GetSpan());
                    break;
                case Integer integer:
                    json["value"] = integer.GetInteger().ToString();
                    break;
                case Map map:
                    context ??= new HashSet<StackItem>(SpencersReferenceEqualityComparer.Default);
                    if (!context.Add(map)) break;
                    json["value"] = new SJArray(map.Select(p =>
                    {
                        SJObject item = new SJObject();
                        item["key"] = ToSJson(p.Key, context);
                        item["value"] = ToSJson(p.Value, context);
                        return item;
                    }));
                    break;
                case Pointer pointer:
                    json["value"] = pointer.Position;
                    break;
            }
            return json;
        }
    }
}
