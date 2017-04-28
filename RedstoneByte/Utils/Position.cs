using System;

namespace RedstoneByte.Utils
{
    public struct Position : IEquatable<Position>, IComparable<Position>
    {
        public static readonly Position Zero = new Position(0, 0, 0);

        public readonly int X;
        public readonly short Y;
        public readonly int Z;

        public Position(int x, short y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(long value)
        {
            X = (int) (value >> 38);
            Y = (short) ((value >> 26) & 0xFFF);
            Z = (int) ((value << 38) >> 38);
        }

        public long ToLong()
        {
            return ((X & 0x3FFFFFFL) << 38) | ((Y & 0xFFFL) << 26) | (Z & 0x3FFFFFFL);
        }

        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public int CompareTo(Position other)
        {
            return Compare(this, other);
        }

        public override bool Equals(object obj)
        {
            return obj is Position && Equals((Position) obj);
        }

        public override string ToString()
        {
            return $"({X}|{Y}|{Z})";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }

        private static int Compare(Position l, Position r)
        {
            if (l.X > r.X) return 1;
            if (l.X < r.X) return -1;

            if (l.Y > r.Y) return 1;
            if (l.Y < r.Y) return -1;

            if (l.Z > r.Z) return 1;
            if (l.Z < r.Z) return -1;

            return 0;
        }

        public static bool operator ==(Position l, Position r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Position l, Position r)
        {
            return !l.Equals(r);
        }

        public static bool operator >(Position l, Position r)
        {
            return Compare(l, r) > 0;
        }

        public static bool operator <(Position l, Position r)
        {
            return Compare(l, r) < 0;
        }

        public static bool operator >=(Position l, Position r)
        {
            return Compare(l, r) >= 0;
        }

        public static bool operator <=(Position l, Position r)
        {
            return Compare(l, r) <= 0;
        }
    }
}