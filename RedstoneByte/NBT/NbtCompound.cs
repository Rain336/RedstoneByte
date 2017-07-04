using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.NBT
{
    public sealed class NbtCompound : NbtTag, IEquatable<NbtCompound>, IDictionary<string, NbtTag>
    {
        public readonly IDictionary<string, NbtTag> Value = new Dictionary<string, NbtTag>();
        public string Name { get; set; }

        public NbtCompound() : base(NbtType.Compound)
        {
        }

        public bool Equals(NbtCompound other)
        {
            return base.Equals(other) && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NbtCompound);
        }

        public void ReadFromBuffer(IByteBuffer buffer)
        {
            var type = (NbtType) buffer.ReadByte();
            while (type != NbtType.End)
            {
                var name = Encoding.UTF8.GetString(buffer.ReadBytes(buffer.ReadUnsignedShort()).ToArray());
                var item = type.ReadFromBuffer(buffer);
                Value.Add(name, item);
                type = (NbtType) buffer.ReadByte();
            }
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            foreach (var tag in Value)
            {
                buffer.WriteByte((byte) tag.Value.Type);
                buffer.WriteUnsignedShort((ushort) tag.Key.Length);
                buffer.WriteBytes(Encoding.UTF8.GetBytes(tag.Key));
                tag.Value.WriteToBuffer(buffer);
            }
        }

        public IEnumerator<KeyValuePair<string, NbtTag>> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(NbtCompound l, NbtCompound r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtCompound l, NbtCompound r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }

        public void Add(KeyValuePair<string, NbtTag> item)
        {
            Value.Add(item);
        }

        public void Clear()
        {
            Value.Clear();
        }

        public bool Contains(KeyValuePair<string, NbtTag> item)
        {
            return Value.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, NbtTag>[] array, int arrayIndex)
        {
            Value.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, NbtTag> item)
        {
            return Value.Remove(item);
        }

        public int Count => Value.Count;
        public bool IsReadOnly => Value.IsReadOnly;

        public void Add(string key, NbtTag value)
        {
            Value.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return Value.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return Value.Remove(key);
        }

        public bool TryGetValue(string key, out NbtTag value)
        {
            return Value.TryGetValue(key, out value);
        }

        public NbtTag this[string key]
        {
            get => Value[key];
            set => Value[key] = value;
        }

        public ICollection<string> Keys => Value.Keys;
        public ICollection<NbtTag> Values => Value.Values;
    }
}