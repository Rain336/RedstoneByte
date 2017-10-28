using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RedstoneByte.Utils
{
    /// <summary>
    /// This class defins a Version of the Minecraft Protocol.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class ProtocolVersion : IEquatable<ProtocolVersion>, IComparable<ProtocolVersion>
    {
        private static readonly Dictionary<int, ProtocolVersion> _versions = new Dictionary<int, ProtocolVersion>();

        /// <summary>
        /// Protocol Version for Mincraft 1.12.2.
        /// </summary>
        public static readonly ProtocolVersion V1122 = GetOrCreate("1.12.2", 340);

        /// <summary>
        /// Protocol Version for Mincraft 1.12.1.
        /// </summary>
        public static readonly ProtocolVersion V1121 = GetOrCreate("1.12.1", 338);

        /// <summary>
        /// Protocol Version for Mincraft 1.12.
        /// </summary>
        public static readonly ProtocolVersion V112 = GetOrCreate("1.12", 335);

        /// <summary>
        /// Protocol Version for Mincraft 1.11.1 to 1.11.2.
        /// </summary>
        public static readonly ProtocolVersion V1112 = GetOrCreate("1.11.2", 316);

        /// <summary>
        /// Protocol Version for Mincraft 1.11.
        /// </summary>
        public static readonly ProtocolVersion V111 = GetOrCreate("1.11", 315);

        /// <summary>
        /// Protocol Version for Mincraft 1.10 to 1.10.2.
        /// </summary>
        public static readonly ProtocolVersion V1102 = GetOrCreate("1.10.2", 210);

        /// <summary>
        /// Protocol Version for Mincraft 1.9.3 to 1.9.4.
        /// </summary>
        public static readonly ProtocolVersion V194 = GetOrCreate("1.9.4", 110);

        /// <summary>
        /// Protocol Version for Mincraft 1.9.2.
        /// </summary>
        public static readonly ProtocolVersion V192 = GetOrCreate("1.9.2", 109);

        /// <summary>
        /// Protocol Version for Mincraft 1.9.1.
        /// </summary>
        public static readonly ProtocolVersion V191 = GetOrCreate("1.9.1", 108);

        /// <summary>
        /// Protocol Version for Mincraft 1.9.
        /// </summary>
        public static readonly ProtocolVersion V19 = GetOrCreate("1.9", 107);

        /// <summary>
        /// Protocol Version for Mincraft 1.8 to 1.8.9.
        /// </summary>
        public static readonly ProtocolVersion V189 = GetOrCreate("1.8.9", 47);

        /// <summary>
        /// Gets a <see cref="ProtocolVersion"/> by it's id.
        /// If it dosn't exsists, it gets created.
        /// </summary>
        /// <param name="name">The name of the ProtocolVersion.</param>
        /// <param name="id">The id of the ProtocolVersion.</param>
        /// <returns>The ProtocolVersion.</returns>
        public static ProtocolVersion GetOrCreate(string name, int id)
        {
            return Get(id) ?? new ProtocolVersion(name, id);
        }

        /// <summary>
        /// Creates a Range of <see cref="ProtocolVersion"/>s with no limit.
        /// Aka all versions.
        /// </summary>
        /// <returns>All versions in an array.</returns>
        public static ProtocolVersion[] Range()
        {
            return _versions.Values.ToArray();
        }

        /// <summary>
        /// Creates a range of <see cref="ProtocolVersion"/>s.
        /// </summary>
        /// <param name="start">The start of the Range.</param>
        /// <returns>All <see cref="ProtocolVersion"/>s from <see cref="start"/> to the newet version.</returns>
        public static ProtocolVersion[] Range(ProtocolVersion start)
        {
            return _versions.Values.Where(it => it >= start).ToArray();
        }

        /// <summary>
        /// Creates a range of <see cref="ProtocolVersion"/>s.
        /// </summary>
        /// <param name="start">The start of the Range.</param>
        /// <param name="end">The end of the Range.</param>
        /// <returns>All <see cref="ProtocolVersion"/>s from <see cref="start"/> to <see cref="end"/>.</returns>
        public static ProtocolVersion[] Range(ProtocolVersion start, ProtocolVersion end)
        {
            return _versions.Values.Where(it => it >= start && it <= end).ToArray();
        }

        /// <summary>
        /// Gets a <see cref="ProtocolVersion"/> by it's id.
        /// If it dosn't exsists, null is returned.
        /// </summary>
        /// <param name="id">The id of the ProtocolVersion.</param>
        /// <returns>The ProtocolVersion or null.</returns>
        public static ProtocolVersion Get(int id)
        {
            return _versions.TryGetValue(id, out var version) ? version : null;
        }

        /// <summary>
        /// Gets the neares version to the given version id.
        /// </summary>
        /// <param name="version">The version id to approximate.</param>
        /// <returns>The nearest Version.</returns>
        public static ProtocolVersion ApproximateVersion(int version)
        {
            if (_versions.TryGetValue(version, out var ret)) return ret;
            ProtocolVersion result = null;
            var distance = version;
            foreach (var value in _versions.Values)
            {
                var current = value.Id - version;
                if (current < 0) current = -current;
                if (current >= distance) continue;
                result = value;
                distance = current;
            }
            return result;
        }

        /// <summary>
        /// The Human-readable Name of a Protocol Version.
        /// </summary>
        [JsonProperty("name")]
        public readonly string Name;

        /// <summary>
        /// The numeric Id of a Protocol Version
        /// </summary>
        [JsonProperty("protocol")]
        public readonly int Id;

        /// <summary>
        /// Creates a new Protocol Version.
        /// </summary>
        /// <param name="name">The <see cref="Name"/> ot the Version.</param>
        /// <param name="id">The <see cref="Id"/> of the Version.</param>
        private ProtocolVersion(string name, int id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = id;
            _versions.Add(id, this);
        }

        public bool Equals(ProtocolVersion other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Id == other.Id;
        }

        public int CompareTo(ProtocolVersion other)
        {
            return CompareTo(this, other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProtocolVersion);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Name;
        }

        private static int CompareTo(ProtocolVersion l, ProtocolVersion r)
        {
            if (l == null && r == null) return 0;
            if (l == null) return -1;
            if (r == null) return 1;

            if (l.Id > r.Id) return 1;
            if (l.Id < r.Id) return -1;

            return 0;
        }

        public static bool operator ==(ProtocolVersion l, ProtocolVersion r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(ProtocolVersion l, ProtocolVersion r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }

        public static bool operator ==(ProtocolVersion l, int r)
        {
            return !ReferenceEquals(l, null) && l.Id == r;
        }

        public static bool operator !=(ProtocolVersion l, int r)
        {
            return ReferenceEquals(l, null) || l.Id != r;
        }

        public static bool operator <(ProtocolVersion l, ProtocolVersion r)
        {
            return CompareTo(l, r) < 0;
        }

        public static bool operator >(ProtocolVersion l, ProtocolVersion r)
        {
            return CompareTo(l, r) > 0;
        }

        public static bool operator <(ProtocolVersion l, int r)
        {
            return ReferenceEquals(l, null) || l.Id < r;
        }

        public static bool operator >(ProtocolVersion l, int r)
        {
            return !ReferenceEquals(l, null) && l.Id > r;
        }

        public static bool operator <=(ProtocolVersion l, ProtocolVersion r)
        {
            return CompareTo(l, r) <= 0;
        }

        public static bool operator >=(ProtocolVersion l, ProtocolVersion r)
        {
            return CompareTo(l, r) >= 0;
        }

        public static bool operator <=(ProtocolVersion l, int r)
        {
            return ReferenceEquals(l, null) || l.Id <= r;
        }

        public static bool operator >=(ProtocolVersion l, int r)
        {
            return !ReferenceEquals(l, null) && l.Id >= r;
        }
    }
}