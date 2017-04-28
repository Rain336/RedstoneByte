using System;

namespace RedstoneByte.Utils
{
    public struct BlockId : IEquatable<BlockId>
    {
        public static readonly BlockId Zero = new BlockId(0, 0);

        public readonly byte Id;
        public readonly byte Meatdata;

        public BlockId(byte id, byte meatdata)
        {
            Id = id;
            Meatdata = meatdata;
        }

        public BlockId(int value)
        {
            Id = (byte) (value >> 4);
            Meatdata = (byte) (value & 0xF);
        }

        public bool Equals(BlockId other)
        {
            return Id == other.Id && Meatdata == other.Meatdata;
        }

        public override bool Equals(object obj)
        {
            return obj is BlockId && Equals((BlockId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ Meatdata.GetHashCode();
            }
        }

        public static bool operator ==(BlockId l, BlockId r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(BlockId l, BlockId r)
        {
            return !l.Equals(r);
        }
    }
}