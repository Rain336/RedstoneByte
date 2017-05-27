using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public class NbtLong : NbtTag, IEquatable<NbtLong>
    {
        public readonly long Value;

        public NbtLong(long value = 0) : base(NbtType.Long)
        {
            Value = value;
        }

        public bool Equals(NbtLong other)
        {
            return base.Equals(other) && Value == other.Value;
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as NbtLong);
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteLong(Value);
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

        public static implicit operator long(NbtLong value)
        {
            return value.Value;
        }

        public static implicit operator NbtLong(long value)
        {
            return new NbtLong(value);
        }

        public static bool operator ==(NbtLong l, NbtLong r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtLong l, NbtLong r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}