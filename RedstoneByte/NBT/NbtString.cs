using System;
using System.Text;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public sealed class NbtString : NbtTag, IEquatable<NbtString>
    {
        public readonly string Value;

        public NbtString(string value = "") : base(NbtType.String)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool Equals(NbtString other)
        {
            return base.Equals(other) && Value == other.Value;
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as NbtString);
        }

        public override void WriteToBuffer(IByteBuffer buffer)
        {
            buffer.WriteUnsignedShort((ushort) Value.Length);
            buffer.WriteBytes(Encoding.UTF8.GetBytes(Value));
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
            return Value;
        }

        public static implicit operator string(NbtString value)
        {
            return value.Value;
        }

        public static implicit operator NbtString(string value)
        {
            return new NbtString(value);
        }

        public static bool operator ==(NbtString l, NbtString r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtString l, NbtString r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}