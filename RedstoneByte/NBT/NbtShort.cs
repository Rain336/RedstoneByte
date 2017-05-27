using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public sealed class NbtShort : NbtTag, IEquatable<NbtShort>
    {
        public readonly short Value;

        public NbtShort(short value = 0) : base(NbtType.Short)
        {
            Value = value;
        }

        public bool Equals(NbtShort other)
        {
            return base.Equals(other) && Value == other.Value;
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as NbtShort);
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteShort(Value);
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

        public static implicit operator short(NbtShort value)
        {
            return value.Value;
        }

        public static implicit operator NbtShort(short value)
        {
            return new NbtShort(value);
        }

        public static bool operator ==(NbtShort l, NbtShort r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtShort l, NbtShort r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}