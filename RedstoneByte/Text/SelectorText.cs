using System;
using System.Text;
using Newtonsoft.Json;

namespace RedstoneByte.Text
{
    /// <summary>
    /// A Text Displaying a Selector.
    /// </summary>
    public sealed class SelectorText : TextBase, IEquatable<SelectorText>
    {
        /// <summary>
        /// The <see cref="Selector"/> of this <see cref="SelectorText"/>.
        /// </summary>
        public Selector Selector { get; set; }

        /// <summary>
        /// Creates a <see cref="SelectorText"/>.
        /// </summary>
        /// <param name="selector">The <see cref="Selector"/> of this Text.</param>
        public SelectorText(Selector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Creates a <see cref="SelectorText"/> from another one.
        /// </summary>
        /// <param name="other">The other SelectorText.</param>
        public SelectorText(SelectorText other) : base(other)
        {
            Selector = other.Selector;
        }

        public bool Equals(SelectorText other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Selector == other.Selector;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SelectorText);
        }

        public override int GetHashCode()
        {
            return Selector.GetHashCode();
        }

        protected override void ToPlain(StringBuilder builder)
        {
            builder.Append(Selector);
            base.ToPlain(builder);
        }

        public override TextBase Copy()
        {
            return new SelectorText(this);
        }

        public static bool operator ==(SelectorText l, SelectorText r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(SelectorText l, SelectorText r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }

    /// <summary>
    /// An enum representing the Minecraft Selectors.
    /// </summary>
    public enum Selector
    {
        NearestPlayer,
        RandomPlayer,
        AllPlayers,
        AllEntities
    }
}