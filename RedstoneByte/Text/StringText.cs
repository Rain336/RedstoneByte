using System;
using System.Text;

namespace RedstoneByte.Text
{
    /// <summary>
    /// A Text Displaying a string.
    /// </summary>
    public sealed class StringText : TextBase, IEquatable<StringText>
    {
        /// <summary>
        /// The string of this Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Creates a new <see cref="StringText"/>.
        /// </summary>
        /// <param name="text">The <see cref="Text"/> of this Text.</param>
        public StringText(string text = "")
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        /// <summary>
        /// Creates a <see cref="StringText"/> from another one.
        /// </summary>
        /// <param name="other">The other StringText.</param>
        public StringText(StringText other) : base(other)
        {
            Text = other.Text;
        }

        /// <summary>
        /// Creates an empty <see cref="StringText"/> with the given extra text.
        /// </summary>
        /// <param name="extras">The extras of this text.</param>
        public StringText(params TextBase[] extras) : this()
        {
            Extra.AddRange(extras);
        }

        public bool Equals(StringText other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Text == other.Text;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StringText);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        protected override void ToPlain(StringBuilder builder)
        {
            builder.Append(Text);
            base.ToPlain(builder);
        }

        public override TextBase Copy()
        {
            return new StringText(this);
        }

        public static bool operator ==(StringText l, StringText r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(StringText l, StringText r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}