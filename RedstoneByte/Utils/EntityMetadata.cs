using System.Collections;
using System.Collections.Generic;
using DotNetty.Buffers;

namespace RedstoneByte.Utils
{
    public sealed class EntityMetadata : IList<EntityMetadata.Entry>
    {
        public readonly List<Entry> Entries = new List<Entry>();

        public void ReadFromBuffer(IByteBuffer buffer)
        {
            var index = buffer.ReadByte();
            while (index != 0xFF)
            {
                var type = (EntryType) buffer.ReadByte();
                Entries.Insert(index, new Entry(type, type.ReadFromBuffer(buffer)));
                index = buffer.ReadByte();
            }
        }

        public sealed class Entry
        {
            public readonly EntryType Type;
            public readonly object Value;

            public Entry(EntryType type, object value)
            {
                Type = type;
                Value = value;
            }

            public T GetValue<T>()
            {
                return (T) Value;
            }
        }

        public enum EntryType
        {
            Byte,
            VarInt,
            Float,
            String,
            Chat,
            Slot,
            Boolean,
            Rotation,
            Position,
            OptPosition,
            Direction,
            OptGuid,
            OptBlockId
        }

        public IEnumerator<Entry> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Entry item)
        {
            Entries.Add(item);
        }

        public void Clear()
        {
            Entries.Clear();
        }

        public bool Contains(Entry item)
        {
            return Entries.Contains(item);
        }

        public void CopyTo(Entry[] array, int arrayIndex)
        {
            Entries.CopyTo(array, arrayIndex);
        }

        public bool Remove(Entry item)
        {
            return Entries.Remove(item);
        }

        public int Count => Entries.Count;
        public bool IsReadOnly => false;

        public int IndexOf(Entry item)
        {
            return Entries.IndexOf(item);
        }

        public void Insert(int index, Entry item)
        {
            Entries.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Entries.RemoveAt(index);
        }

        public Entry this[int index]
        {
            get => Entries[index];
            set => Entries[index] = value;
        }
    }
}