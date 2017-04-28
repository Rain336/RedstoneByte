using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedstoneByte.Text
{
    /// <summary>
    /// The base class of a Texts.
    /// </summary>
    [JsonConverter(typeof(TextBaseConverter))]
    public abstract class TextBase
    {
        /// <summary>
        /// A flags enum representing the style of this Text.
        /// Aka this controlls <see cref="Bold"/>, <see cref="Italic"/>, <see cref="Underlined"/>, <see cref="Strikethrough"/> and <see cref="Obfuscated"/>
        /// </summary>
        public TextStyle Style { get; set; }

        /// <summary>
        /// Is this Text bold?
        /// </summary>
        public bool Bold
        {
            get => Style.HasFlag(TextStyle.Bold);
            set
            {
                if (value) Style |= TextStyle.Bold;
                else Style &= ~TextStyle.Bold;
            }
        }

        /// <summary>
        /// Is this Text italic?
        /// </summary>
        public bool Italic
        {
            get => Style.HasFlag(TextStyle.Italic);
            set
            {
                if (value) Style |= TextStyle.Italic;
                else Style &= ~TextStyle.Italic;
            }
        }

        /// <summary>
        /// Is this Text underlined?
        /// </summary>
        public bool Underlined
        {
            get => Style.HasFlag(TextStyle.Underlind);
            set
            {
                if (value) Style |= TextStyle.Underlind;
                else Style &= ~TextStyle.Underlind;
            }
        }

        /// <summary>
        /// Is this Text strikethrough?
        /// </summary>
        public bool Strikethrough
        {
            get => Style.HasFlag(TextStyle.Strikethrough);
            set
            {
                if (value) Style |= TextStyle.Strikethrough;
                else Style &= ~TextStyle.Strikethrough;
            }
        }

        /// <summary>
        /// Is this Text obfuscated?
        /// </summary>
        public bool Obfuscated
        {
            get => Style.HasFlag(TextStyle.Obfuscated);
            set
            {
                if (value) Style |= TextStyle.Obfuscated;
                else Style &= ~TextStyle.Obfuscated;
            }
        }

        /// <summary>
        /// The <see cref="TextColor"/> of this Text.
        /// </summary>
        public TextColor Color { get; set; }

        /// <summary>
        /// The Text to insert on shif-click.
        /// </summary>
        public string Insertion { get; set; }

        /// <summary>
        /// The <see cref="ClickEvent"/> of this Text.
        /// </summary>
        public ClickEvent ClickEvent { get; set; }

        /// <summary>
        /// The <see cref="HoverEvent"/> of this Text.
        /// </summary>
        public HoverEvent HoverEvent { get; set; }

        /// <summary>
        /// The siblings of this Text.
        /// </summary>
        public readonly List<TextBase> Extra = new List<TextBase>();

        public bool HasFormatting => Style != TextStyle.None ||
                                     Color != TextColor.Reset ||
                                     !string.IsNullOrEmpty(Insertion) ||
                                     HoverEvent != null ||
                                     ClickEvent != null;

        /// <summary>
        /// Makes an excact compare of two Texts.
        /// </summary>
        /// <param name="other">The other Text to compare to.</param>
        /// <returns>true, if the Texts are equal.</returns>
        public virtual bool DeepEquals(TextBase other)
        {
            return Equals(other) && Bold == other.Bold && Italic == other.Italic && Underlined == other.Underlined &&
                   Strikethrough == other.Strikethrough && Obfuscated == other.Obfuscated && Color == other.Color &&
                   Insertion == other.Insertion && ClickEvent == other.ClickEvent && HoverEvent == other.HoverEvent &&
                   Extra.SequenceEqual(other.Extra);
        }

        /// <summary>
        /// Creates a <see cref="TextBase"/>
        /// </summary>
        protected TextBase()
        {
        }

        /// <summary>
        /// Creates a <see cref="TextBase"/> from anoter one.
        /// </summary>
        /// <param name="other">The other TextBase.</param>
        protected TextBase(TextBase other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Style = other.Style;
            Color = other.Color;
            Insertion = other.Insertion;
            ClickEvent = other.ClickEvent;
            HoverEvent = other.HoverEvent;
            if (other.Extra.Count == 0) return;
            foreach (var e in other.Extra)
            {
                Extra.Add(e.Copy());
            }
        }

        /// <summary>
        /// Returns a simple string representation of this Text.
        /// </summary>
        /// <returns>A string representation.</returns>
        public string ToPlain()
        {
            var builder = new StringBuilder();
            ToPlain(builder);
            return builder.ToString();
        }

        protected virtual void ToPlain(StringBuilder builder)
        {
            foreach (var extra in Extra)
            {
                extra.ToPlain(builder);
            }
        }

        /// <summary>
        /// Creates a copy of this <see cref="TextBase"/>.
        /// </summary>
        /// <returns>The copy.</returns>
        public abstract TextBase Copy();

        public override string ToString()
        {
            return ToPlain();
        }

        public sealed class TextBaseConverter : JsonConverter
        {
            public override bool CanConvert(Type type)
            {
                return typeof(TextBase).IsAssignableFrom(type);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                var text = (TextBase) value;
                WriteProperty(writer, "bold", text.Bold);
                WriteProperty(writer, "italic", text.Italic);
                WriteProperty(writer, "underlined", text.Underlined);
                WriteProperty(writer, "strikethrough", text.Strikethrough);
                WriteProperty(writer, "obfuscated", text.Obfuscated);
                WriteProperty(writer, "color", text.Color.Serialize());

                if (!string.IsNullOrEmpty(text.Insertion))
                    WriteProperty(writer, "insertion", text.Insertion);

                if (text.ClickEvent != null)
                {
                    writer.WritePropertyName("clickEvent");
                    writer.WriteStartObject();
                    WriteProperty(writer, "action", text.ClickEvent.Action.Serialize());
                    WriteProperty(writer, "value", text.ClickEvent.Value);
                    writer.WriteEndObject();
                }

                if (text.HoverEvent != null)
                {
                    writer.WritePropertyName("hoverEvent");
                    writer.WriteStartObject();
                    WriteProperty(writer, "action", text.HoverEvent.Action.Serialize());
                    WriteProperty(writer, "value", text.HoverEvent.Value);
                    writer.WriteEndObject();
                }

                if (text.Extra.Count > 0)
                    WriteProperty(writer, "extra", text.Extra, serializer);

                switch (text)
                {
                    case StringText str:
                        WriteProperty(writer, "text", str.Text);
                        break;

                    case TranslationText translation:
                        WriteProperty(writer, "translate", translation.Key);
                        if (translation.With.Count > 0)
                            WriteProperty(writer, "with", translation.With, serializer);
                        break;

                    case ScoreText score:
                        writer.WritePropertyName("score");
                        writer.WriteStartObject();
                        WriteProperty(writer, "name", score.Name);
                        WriteProperty(writer, "objective", score.Objective);
                        WriteProperty(writer, "value", score.Value);
                        writer.WriteEndObject();
                        break;

                    case SelectorText selector:
                        WriteProperty(writer, "selector", selector.Selector.Serialize());
                        break;
                }
                writer.WriteEndObject();
            }

            private static void WriteProperty<T>(JsonWriter writer, string name, T value)
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }

            private static void WriteProperty<T>(JsonWriter writer, string name, T value, JsonSerializer serializer)
            {
                writer.WritePropertyName(name);
                serializer.Serialize(writer, value);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        return new StringText(reader.Value.ToString());

                    case JsonToken.StartArray:
                        TextBase root = null;
                        foreach (var text in serializer.Deserialize<List<TextBase>>(reader))
                        {
                            if (root == null)
                                root = text;
                            else
                                root.Extra.Add(text);
                        }
                        return root;

                    case JsonToken.StartObject:
                        var json = JObject.Load(reader);
                        TextBase result;
                        if (json["text"] != null)
                        {
                            result = new StringText(json["text"].Value<string>());
                        }
                        else if (json["translate"] != null)
                        {
                            result = new TranslationText(json["translate"].Value<string>());
                            if (json["with"] != null)
                                ((TranslationText) result).With.AddRange(json["with"].ToObject<List<TextBase>>());
                        }
                        else if (json["score"] != null)
                        {
                            result = new ScoreText(json["score"]["name"].Value<string>(),
                                json["score"]["objective"].Value<string>());
                            if (json["score"]["value"] != null)
                                ((ScoreText) result).Value = json["score"]["value"].Value<string>();
                        }
                        else if (json["selector"] != null)
                        {
                            result = new SelectorText(EnumExtender.ParseSelector(json["selector"].Value<string>()));
                        }
                        else
                            throw new JsonSerializationException("Text is missing Type!");
                        if (json["bold"] != null)
                            result.Bold = json["bold"].Value<bool>();
                        if (json["italic"] != null)
                            result.Italic = json["italic"].Value<bool>();
                        if (json["underlined"] != null)
                            result.Underlined = json["underlined"].Value<bool>();
                        if (json["strikethrough"] != null)
                            result.Strikethrough = json["strikethrough"].Value<bool>();
                        if (json["obfuscated"] != null)
                            result.Obfuscated = json["obfuscated"].Value<bool>();
                        if (json["color"] != null)
                            result.Color = EnumExtender.ParseColor(json["color"].Value<string>());
                        if (json["insertion"] != null)
                            result.Insertion = json["insertion"].Value<string>();
                        if (json["clickEvent"] != null)
                            result.ClickEvent =
                                new ClickEvent(EnumExtender.ParseClick(json["clickEvent"]["action"].Value<string>()),
                                    json["clickEvent"]["value"].Value<string>());
                        if (json["hoverEvent"] != null)
                            result.HoverEvent =
                                new HoverEvent(EnumExtender.ParseHover(json["hoverEvent"]["action"].Value<string>()),
                                    json["hoverEvent"]["value"].Value<string>());
                        return result;

                    default:
                        throw new JsonSerializationException("Unexpected Token!");
                }
            }
        }
    }

    /// <summary>
    /// An enum representing the style of a Text.
    /// </summary>
    [Flags]
    public enum TextStyle
    {
        None,
        Bold,
        Italic,
        Underlind,
        Strikethrough,
        Obfuscated
    }
}