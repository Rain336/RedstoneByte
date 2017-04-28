using System;

namespace RedstoneByte.Text
{
    /// <summary>
    /// The <see cref="TextBase"/> Factory class.
    /// </summary>
    public static class Texts
    {
        /// <summary>
        /// Creates an empty Text.
        /// </summary>
        /// <returns>An empty Text.</returns>
        public static TextBase Of()
        {
            return new StringText();
        }

        /// <summary>
        /// Creates a Text from a string.
        /// </summary>
        /// <param name="value">The string of this Text.</param>
        /// <returns>The string as a Text.</returns>
        public static TextBase Of(string value)
        {
            return new StringText(value);
        }

        /// <summary>
        /// Creates a Text from a char.
        /// </summary>
        /// <param name="value">The char of this Text.</param>
        /// <returns>The char as a Text.</returns>
        public static TextBase Of(char value)
        {
            return new StringText(value.ToString());
        }

        /// <summary>
        /// Creates a Text from a <see cref="Selector"/>.
        /// </summary>
        /// <param name="selector">The Selector of this Text.</param>
        /// <returns>The Selector as a Text.</returns>
        public static TextBase Of(Selector selector)
        {
            return new SelectorText(selector);
        }

        /// <summary>
        /// Creates a Text from a sequence of objects.
        /// </summary>
        /// <param name="args">The objects of this Text.</param>
        /// <returns>The Objects as a Text.</returns>
        public static TextBase Of(params object[] args)
        {
            if (args.Length == 0)
            {
                var arg = args[0];
                if (arg is TextBase)
                {
                    return (TextBase) args[0];
                }
                if (arg is Selector)
                {
                    return new SelectorText((Selector) arg);
                }
                if (arg is string)
                {
                    return Of((string) arg);
                }
                return null;
            }

            TextBase result = null;
            var style = TextStyle.None;
            var color = TextColor.Reset;
            ClickEvent click = null;
            HoverEvent hover = null;
            var formated = false;

            foreach (var obj in args)
            {
                if (obj is TextStyle)
                {
                    formated = true;
                    style = (TextStyle) obj;
                }
                else if (obj is TextColor)
                {
                    formated = true;
                    color = (TextColor) obj;
                }
                else if (obj is ClickEvent)
                {
                    formated = true;
                    click = (ClickEvent) obj;
                }
                else if (obj is HoverEvent)
                {
                    formated = true;
                    hover = (HoverEvent) obj;
                }
                else if (obj is TextBase)
                {
                    formated = false;
                    var text = (TextBase) obj;

                    text.Style |= style;
                    if (color != TextColor.Reset)
                        text.Color = color;
                    if (text.ClickEvent == null)
                        text.ClickEvent = click;
                    if (text.HoverEvent == null)
                        text.HoverEvent = hover;

                    if (result == null) result = text;
                    else result.Extra.Add(text);
                }
                else
                {
                    formated = false;
                    TextBase text;

                    if (obj is string)
                        text = new StringText((string) obj);
                    else if (obj is Selector)
                        text = new SelectorText((Selector) obj);
                    else
                        text = new StringText(obj.ToString());

                    text.Style = style;
                    text.Color = color;
                    text.ClickEvent = click;
                    text.HoverEvent = hover;

                    if (result == null) result = text;
                    else result.Extra.Add(text);
                }
            }

            if (formated)
            {
                TextBase text = new StringText();

                text.Style = style;
                text.Color = color;
                text.ClickEvent = click;
                text.HoverEvent = hover;

                if (result == null) result = text;
                else result.Extra.Add(text);
            }

            return result;
        }

        /// <summary>
        /// Creates a Text from a Translation Key and an optional <see cref="TextBase"/> array.
        /// </summary>
        /// <param name="key">The Translation Key of this Text.</param>
        /// <param name="args">An optional Text array of this Text.</param>
        /// <returns>The Key and Texts as Text.</returns>
        public static TranslationText Translate(string key, params TextBase[] args)
        {
            var result = new TranslationText(key);
            result.With.AddRange(args);
            return result;
        }
    }
}