using System;

namespace RedstoneByte.Text
{
    /// <summary>
    /// An event that happens when the player clicks a Text.
    /// </summary>
    public sealed class ClickEvent : IEquatable<ClickEvent>
    {
        /// <summary>
        /// The Action of this Event.
        /// </summary>
        public readonly ClickAction Action;

        /// <summary>
        /// The Value of this Action.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Creates a new <see cref="ClickEvent"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> of this Event.</param>
        /// <param name="value">The <see cref="Value"/> of this Event.</param>
        public ClickEvent(ClickAction action, string value)
        {
            Action = action;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool Equals(ClickEvent other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Action == other.Action && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ClickEvent);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Action * 397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(ClickEvent l, ClickEvent r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(ClickEvent l, ClickEvent r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }

        /// <summary>
        /// The <see cref="ClickAction"/> of a <see cref="ClickEvent"/>.
        /// </summary>
        public enum ClickAction
        {
            /// <summary>
            /// Opens an URL at the Client.
            /// Only HTTP and HTTPS links are allowed.
            /// The <see cref="ClickEvent.Value"/> is the URL to open.
            /// </summary>
            OpenUrl,

            /// <summary>
            /// Runs a Command as a Player.
            /// Can also be used to Write a Text as a Player.
            /// The <see cref="ClickEvent.Value"/> is the Command/Text to run.
            /// </summary>
            RunCommand,

            /// <summary>
            /// Puts a Command into the Players Chat Field.
            /// The <see cref="ClickEvent.Value"/> is the Command/Text to put in.
            /// </summary>
            SuggestCommand,

            /// <summary>
            /// Changes the Page of a Book.
            /// Can only be used in Books.
            /// The <see cref="ClickEvent.Value"/> is the Page to go to.
            /// </summary>
            ChangePage
        }
    }
}