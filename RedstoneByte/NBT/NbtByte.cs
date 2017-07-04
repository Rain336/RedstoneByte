using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public sealed class NbtByte : NbtTag, IEquatable<NbtByte>
    {
        public readonly byte Value;

        public NbtByte(byte value = 0) : base(NbtType.Byte)
        {
            Value = value;
        }

        public bool Equals(NbtByte other)
        {
            return base.Equals(other) && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NbtByte);
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteByte(Value);
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

        public static implicit operator byte(NbtByte value)
        {
            return value.Value;
        }

        public static implicit operator NbtByte(byte value)
        {
            return new NbtByte(value);
        }

        public static bool operator ==(NbtByte l, NbtByte r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtByte l, NbtByte r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}