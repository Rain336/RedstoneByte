using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public sealed class NbtInt : NbtTag, IEquatable<NbtInt>
    {
        public readonly int Value;

        public NbtInt(int value = 0) : base(NbtType.Int)
        {
            Value = value;
        }

        public bool Equals(NbtInt other)
        {
            return base.Equals(other) && Value == other.Value;
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteInt(Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Value;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator int(NbtInt value)
        {
            return value.Value;
        }

        public static implicit operator NbtInt(int value)
        {
            return new NbtInt(value);
        }

        public static bool operator ==(NbtInt l, NbtInt r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtInt l, NbtInt r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}