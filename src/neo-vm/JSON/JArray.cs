using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Neo.IO.Json
{
    public class SJArray : SJObject, IList<SJObject>
    {
        private readonly List<SJObject> items = new List<SJObject>();

        public SJArray(params SJObject[] items) : this((IEnumerable<SJObject>)items)
        {
        }

        public SJArray(IEnumerable<SJObject> items)
        {
            this.items.AddRange(items);
        }

        public SJObject this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(SJObject item)
        {
            items.Add(item);
        }

        public override string AsString()
        {
            return string.Join(",", items.Select(p => p?.AsString()));
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(SJObject item)
        {
            return items.Contains(item);
        }

        public void CopyTo(SJObject[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SJObject> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(SJObject item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, SJObject item)
        {
            items.Insert(index, item);
        }

        public bool Remove(SJObject item)
        {
            return items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        internal override void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (SJObject item in items)
            {
                if (item is null)
                    writer.WriteNullValue();
                else
                    item.Write(writer);
            }
            writer.WriteEndArray();
        }

        public override SJObject Clone()
        {
            var cloned = new SJArray();

            foreach (SJObject item in items)
            {
                cloned.Add(item.Clone());
            }

            return cloned;
        }

        public static implicit operator SJArray(SJObject[] value)
        {
            return new SJArray(value);
        }
    }
}
