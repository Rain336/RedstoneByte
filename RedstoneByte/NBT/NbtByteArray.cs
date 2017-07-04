using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public sealed class NbtByteArray : NbtTag, IEquatable<NbtByteArray>
    {
        public readonly byte[] Value;

        public NbtByteArray() : this(new byte[0])
        {
        }

        public NbtByteArray(byte[] value) : base(NbtType.ByteArray)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool Equals(NbtByteArray other)
        {
            return base.Equals(other) && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NbtByteArray);
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteInt(Value.Length);
            buffer.WriteBytes(Value);
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
            return BitConverter.ToString(Value);
        }

        public static implicit operator byte[](NbtByteArray value)
        {
            return value.Value;
        }

        public static implicit operator NbtByteArray(byte[] value)
        {
            return new NbtByteArray(value);
        }

        public static bool operator ==(NbtByteArray l, NbtByteArray r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtByteArray l, NbtByteArray r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}