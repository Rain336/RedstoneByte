using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public sealed class NbtDouble : NbtTag, IEquatable<NbtDouble>
    {
        public readonly double Value;

        public NbtDouble(double value = 0) : base(NbtType.Double)
        {
            Value = value;
        }

        public bool Equals(NbtDouble other)
        {
            return base.Equals(other) && Value == other.Value;
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteDouble(Value);
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

        public static implicit operator double(NbtDouble value)
        {
            return value.Value;
        }

        public static implicit operator NbtDouble(double value)
        {
            return new NbtDouble(value);
        }

        public static bool operator ==(NbtDouble l, NbtDouble r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtDouble l, NbtDouble r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}