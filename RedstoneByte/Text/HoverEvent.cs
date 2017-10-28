using System;

namespace RedstoneByte.Text
{
    /// <summary>
    /// An Event that hapens when a Player hovers over the Text.
    /// </summary>
    public sealed class HoverEvent : IEquatable<HoverEvent>
    {
        /// <summary>
        /// The <see cref="HoverAction"/> of this <see cref="HoverEvent"/>.
        /// </summary>
        public readonly HoverAction Action;
        /// <summary>
        /// The Value of this <see cref="Action"/>
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Creates a new <see cref="HoverEvent"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> of this event.</param>
        /// <param name="value">The <see cref="Value"/> of this event.</param>
        public HoverEvent(HoverAction action, string value)
        {
            Action = action;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool Equals(HoverEvent other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Action == other.Action && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HoverEvent);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Action * 397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(HoverEvent l, HoverEvent r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(HoverEvent l, HoverEvent r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }

        /// <summary>
        /// The <see cref="HoverAction"/> of a <see cref="HoverEvent"/>.
        /// </summary>
        public enum HoverAction
        {
            /// <summary>
            /// Displays a Text.
            /// The <see cref="HoverEvent.Value"/> is the text to display.
            /// </summary>
            ShowText,

            /// <summary>
            /// Displays an Item.
            /// The <see cref="HoverEvent.Value"/> is the Item to display.
            /// </summary>
            ShowItem,

            /// <summary>
            /// Displays an Entity.
            /// The <see cref="HoverEvent.Value"/> is the Entity to display.
            /// </summary>
            ShowEntity,

            /// <summary>
            /// Displays an Achievement.
            /// The <see cref="HoverEvent.Value"/> is the id of the Achievement to display.
            /// </summary>
            ShowAchievement
        }
    }
}