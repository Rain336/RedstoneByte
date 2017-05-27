using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.NBT
{
    public sealed class NbtList : NbtTag, IEquatable<NbtList>, IList<NbtTag>
    {
        private readonly List<NbtTag> _value = new List<NbtTag>();

        public NbtList() : base(NbtType.List)
        {
        }

        public bool Equals(NbtList other)
        {
            return base.Equals(other) && _value.SequenceEqual(other._value);
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as NbtList);
        }

        public void ReadFromBuffer(IByteBuffer buffer)
        {
            var type = (NbtType) buffer.ReadByte();
            if (_value.Count > 0 && _value[0].Type != type)
                throw new InvalidOperationException("Type Mismatch! Expected: " + _value[0].Type + " Got: " + type);
            var count = buffer.ReadInt();
            if (count <= 0) return;
            for (var i = 0; i < count; i++)
            {
                _value.Add(type.ReadFromBuffer(buffer));
            }
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            if (_value.Count == 0)
            {
                buffer.WriteByte(0);
                buffer.WriteInt(0);
                return;
            }
            buffer.WriteByte((byte) _value[0].Type);
            buffer.WriteInt(_value.Count);
            foreach (var tag in _value)
            {
                tag.WriteToBuffer(buffer);
            }
        }

        public IEnumerator<NbtTag> GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ _value.GetHashCode();
            }
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator List<NbtTag>(NbtList value)
        {
            return value._value;
        }

        public static implicit operator NbtList(List<NbtTag> value)
        {
            var result = new NbtList();
            result._value.AddRange(value);
            return result;
        }

        public static bool operator ==(NbtList l, NbtList r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtList l, NbtList r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }

        public void Add(NbtTag item)
        {
            if (_value.Count > 0 && _value[0].Type != item.Type)
                throw new InvalidOperationException("Type Mismatch! Expected: " + _value[0].Type + " Got: " +
                                                    item.Type);
            _value.Add(item);
        }

        public void Clear()
        {
            _value.Clear();
        }

        public bool Contains(NbtTag item)
        {
            return _value.Contains(item);
        }

        public void CopyTo(NbtTag[] array, int arrayIndex)
        {
            _value.CopyTo(array, arrayIndex);
        }

        public bool Remove(NbtTag item)
        {
            return _value.Remove(item);
        }

        public int Count => _value.Count;
        public bool IsReadOnly => false;

        public int IndexOf(NbtTag item)
        {
            return _value.IndexOf(item);
        }

        public void Insert(int index, NbtTag item)
        {
            if (_value.Count > 0 && _value[0].Type != item.Type)
                throw new InvalidOperationException("Type Mismatch! Expected: " + _value[0].Type + " Got: " +
                                                    item.Type);
            _value.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _value.RemoveAt(index);
        }

        public NbtTag this[int index]
        {
            get => _value[index];
            set
            {
                if (_value.Count > 0 && _value[0].Type != value.Type)
                    throw new InvalidOperationException("Type Mismatch! Expected: " + _value[0].Type + " Got: " +
                                                        value.Type);
                _value[index] = value;
            }
        }
    }
}