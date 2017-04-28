using System;
using RedstoneByte.NBT;

namespace RedstoneByte.Utils
{
    public struct Slot : IEquatable<Slot>
    {
        public readonly short Id;
        public byte Count { get; set; }
        public short Metadata { get; set; }
        public readonly NbtCompound NbtCompound;

        public Slot(short id) : this()
        {
            Id = id;
            NbtCompound = new NbtCompound();
        }

        public bool Equals(Slot other)
        {
            return Id == other.Id && Count == other.Count && Metadata == other.Metadata;
        }

        public override bool Equals(object obj)
        {
            return obj is Slot && Equals((Slot) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id;
                hashCode = (hashCode * 397) ^ Count;
                hashCode = (hashCode * 397) ^ Metadata;
                return hashCode;
            }
        }

        public static bool operator ==(Slot l, Slot r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Slot l, Slot r)
        {
            return !l.Equals(r);
        }
    }
}