using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedstoneByte.Text
{
    /// <summary>
    /// A Minecraft Translation as a Text.
    /// </summary>
    public sealed class TranslationText : TextBase, IEquatable<TranslationText>
    {
        /// <summary>
        /// The Key of this Translation.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// An optional array/list of Texts to replace the placeholders in the Translation.
        /// </summary>
        public readonly List<TextBase> With = new List<TextBase>();

        /// <summary>
        /// Creates a Minecraft Text Translation with the given Key.
        /// </summary>
        /// <param name="key">The Key of this Translation.</param>
        public TranslationText(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        /// <summary>
        /// Creats a <see cref="TranslationText"/> from another one.
        /// </summary>
        /// <param name="other">The other TranslationText.</param>
        public TranslationText(TranslationText other) : base(other)
        {
            Key = other.Key;
            if(other.With.Count == 0) return;
            foreach (var with in other.With)
            {
                With.Add(with.Copy());
            }
        }

        public bool Equals(TranslationText other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TranslationText);
        }

        public override int GetHashCode()
        {
            return Key?.GetHashCode() ?? 0;
        }

        public override bool DeepEquals(TextBase other)
        {
            return base.DeepEquals(other) && With.SequenceEqual(((TranslationText) other).With);
        }

        protected override void ToPlain(StringBuilder builder)
        {
            builder.Append(Key);
            base.ToPlain(builder);
        }

        public override TextBase Copy()
        {
            return new TranslationText(this);
        }

        public static bool operator ==(TranslationText l, TranslationText r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(TranslationText l, TranslationText r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}