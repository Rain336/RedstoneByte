using System;

namespace RedstoneByte.Utils
{
    public struct Rotation : IEquatable<Rotation>, IComparable<Rotation>
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Rotation(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(Rotation other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        private static int Compare(Rotation l, Rotation r)
        {
            if (l.X > r.X) return 1;
            if (l.X < r.X) return -1;

            if (l.Y > r.Y) return 1;
            if (l.Y < r.Y) return -1;

            if (l.Z > r.Z) return 1;
            if (l.Z < r.Z) return -1;

            return 0;
        }

        public int CompareTo(Rotation other)
        {
            return Compare(this, other);
        }

        public override bool Equals(object obj)
        {
            return obj is Rotation && Equals((Rotation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Rotation l, Rotation r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Rotation l, Rotation r)
        {
            return !l.Equals(r);
        }

        public static bool operator >(Rotation l, Rotation r)
        {
            return Compare(l, r) > 0;
        }

        public static bool operator <(Rotation l, Rotation r)
        {
            return Compare(l, r) < 0;
        }

        public static bool operator >=(Rotation l, Rotation r)
        {
            return Compare(l, r) >= 0;
        }

        public static bool operator <=(Rotation l, Rotation r)
        {
            return Compare(l, r) <= 0;
        }
    }
}