using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public sealed class NbtIntArray : NbtTag, IEquatable<NbtIntArray>
    {
        public readonly int[] Value;

        public NbtIntArray() : this(new int[0])
        {
        }

        public NbtIntArray(int[] value) : base(NbtType.IntArray)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool Equals(NbtIntArray other)
        {
            return base.Equals(other) && Value == other.Value;
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteInt(Value.Length);
            foreach (var i in Value)
            {
                buffer.WriteInt(i);
            }
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

        public static implicit operator int[](NbtIntArray value)
        {
            return value.Value;
        }

        public static implicit operator NbtIntArray(int[] value)
        {
            return new NbtIntArray(value);
        }

        public static bool operator ==(NbtIntArray l, NbtIntArray r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtIntArray l, NbtIntArray r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}