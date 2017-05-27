using System;
using DotNetty.Buffers;
using RedstoneByte.Networking;

namespace RedstoneByte.NBT
{
    public sealed class NbtFloat : NbtTag, IEquatable<NbtFloat>
    {
        public readonly float Value;

        public NbtFloat(float value = 0) : base(NbtType.Float)
        {
            Value = value;
        }

        public bool Equals(NbtFloat other)
        {
            return base.Equals(other) && Value == other.Value;
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as NbtFloat);
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteFloat(Value);
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

        public static implicit operator float(NbtFloat value)
        {
            return value.Value;
        }

        public static implicit operator NbtFloat(float value)
        {
            return new NbtFloat(value);
        }

        public static bool operator ==(NbtFloat l, NbtFloat r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtFloat l, NbtFloat r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}