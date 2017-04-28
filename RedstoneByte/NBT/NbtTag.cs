using System;
using DotNetty.Buffers;

namespace RedstoneByte.NBT
{
    public abstract class NbtTag : IEquatable<NbtTag>
    {
        public readonly NbtType Type;

        protected NbtTag(NbtType type)
        {
            Type = type;
        }

        public bool Equals(NbtTag other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Type == other.Type;
        }

        public abstract void WriteToBuffer(IByteBuffer buffer);

        public override bool Equals(object obj)
        {
            return Equals(obj as NbtTag);
        }

        public override int GetHashCode()
        {
            return (int) Type;
        }

        public static bool operator ==(NbtTag l, NbtTag r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(NbtTag l, NbtTag r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}