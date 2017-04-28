using System;
using System.Collections.Generic;

namespace RedstoneByte.Utils
{
    /// <summary>
    /// A GameProflie is a registered Minecraft Account with UUID/GUID.
    /// </summary>
    public sealed class GameProfile : IEquatable<GameProfile>
    {
        /// <summary>
        /// The Username of this Account.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The GUID/UUID of this Account.
        /// </summary>
        public readonly Guid Guid;

        /// <summary>
        /// The Properties aka Cape and Skin of this Account.
        /// </summary>
        public readonly List<Property> Properties = new List<Property>();

        /// <summary>
        /// Creats a new <see cref="GameProfile"/>.
        /// </summary>
        /// <param name="name">The <see cref="Name"/> of this GameProfile.</param>
        /// <param name="guid">The <see cref="Guid"/> of this GameProfile.</param>
        public GameProfile(string name, Guid guid)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Guid = guid;
        }

        public bool Equals(GameProfile other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Name == other.Name && Guid == other.Guid;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameProfile);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Guid.GetHashCode();
            }
        }

        public static bool operator ==(GameProfile l, GameProfile r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(GameProfile l, GameProfile r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }

        /// <summary>
        /// A <see cref="Property"/> can be a link to a cape or a skin of a player.
        /// </summary>
        public sealed class Property : IEquatable<Property>
        {
            /// <summary>
            /// The Name or Type of this Property.
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// The Value, a base64 encoded json file.
            /// </summary>
            public readonly string Value;

            /// <summary>
            /// A base64 string, signed by the all mighty Yggdrasil.
            /// Can be null.
            /// </summary>
            public readonly string Signature;

            public bool HasSignature => Signature != null;

            /// <summary>
            /// Creates a Property from a name, a value and an optional signature
            /// </summary>
            /// <param name="name">The name ot this <see cref="Property"/>.</param>
            /// <param name="value">The value ot this <see cref="Property"/>.</param>
            /// <param name="signature">The signature ot this <see cref="Property"/>.</param>
            public Property(string name, string value, string signature = null)
            {
                Name = name ?? throw new ArgumentNullException(nameof(name));
                Value = value ?? throw new ArgumentNullException(nameof(value));
                Signature = signature;
            }

            public bool Equals(Property other)
            {
                if (ReferenceEquals(other, null)) return false;
                return Name == other.Name && Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Property);
            }

            public override string ToString()
            {
                return Value;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Name.GetHashCode() * 397) ^ Value.GetHashCode();
                }
            }

            public static bool operator ==(Property l, Property r)
            {
                return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
            }

            public static bool operator !=(Property l, Property r)
            {
                return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
            }
        }
    }
}