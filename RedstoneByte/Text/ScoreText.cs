using System;
using System.Text;

namespace RedstoneByte.Text
{
    /// <summary>
    /// A Text Displaying a Scorebord Value.
    /// </summary>
    public sealed class ScoreText : TextBase, IEquatable<ScoreText>
    {
        public string Name { get; set; }
        public string Objective { get; set; }
        public string Value { get; set; }

        public ScoreText(string name, string objective)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Objective = objective ?? throw new ArgumentNullException(nameof(objective));
        }

        //TODO: Implement Into Scoreboard Api.

        public ScoreText(ScoreText other) : base(other)
        {
            Name = other.Name;
            Objective = other.Objective;
            Value = other.Value;
        }

        public bool Equals(ScoreText other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Name == other.Name && Objective == other.Objective;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ScoreText);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name?.GetHashCode() * 397 ?? 0) ^ (Objective?.GetHashCode() ?? 0);
            }
        }

        protected override void ToPlain(StringBuilder builder)
        {
            builder.Append(Value);
            base.ToPlain(builder);
        }

        public override TextBase Copy()
        {
            return new ScoreText(this);
        }

        public static bool operator ==(ScoreText l, ScoreText r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(ScoreText l, ScoreText r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}