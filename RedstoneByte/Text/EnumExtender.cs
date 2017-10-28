using System;

namespace RedstoneByte.Text
{
    internal static class EnumExtender
    {
        internal static string Serialize(this HoverEvent.HoverAction action)
        {
            switch (action)
            {
                case HoverEvent.HoverAction.ShowText:
                    return "show_text";

                case HoverEvent.HoverAction.ShowItem:
                    return "show_item";

                case HoverEvent.HoverAction.ShowEntity:
                    return "show_entity";

                case HoverEvent.HoverAction.ShowAchievement:
                    return "show_achievement";

                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        internal static string Serialize(this ClickEvent.ClickAction action)
        {
            switch (action)
            {
                case ClickEvent.ClickAction.OpenUrl:
                    return "open_url";

                case ClickEvent.ClickAction.RunCommand:
                    return "run_command";

                case ClickEvent.ClickAction.SuggestCommand:
                    return "suggest_command";

                case ClickEvent.ClickAction.ChangePage:
                    return "change_page";

                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        internal static string Serialize(this Selector selector)
        {
            switch (selector)
            {
                case Selector.NearestPlayer:
                    return "@p";

                case Selector.RandomPlayer:
                    return "@r";

                case Selector.AllPlayers:
                    return "@a";

                case Selector.AllEntities:
                    return "@e";

                default:
                    throw new ArgumentOutOfRangeException(nameof(selector), selector, null);
            }
        }

        internal static string Serialize(this TextColor color)
        {
            switch (color)
            {
                case TextColor.Black:
                    return "black";

                case TextColor.DarkBlue:
                    return "dark_blue";

                case TextColor.DarkGreen:
                    return "dark_green";

                case TextColor.DarkAqua:
                    return "dark_aqua";

                case TextColor.DarkRed:
                    return "dark_red";

                case TextColor.DarkPurple:
                    return "dark_purple";

                case TextColor.Gold:
                    return "gold";

                case TextColor.Gray:
                    return "gray";

                case TextColor.DarkGray:
                    return "dark_gray";

                case TextColor.Blue:
                    return "blue";

                case TextColor.Green:
                    return "green";

                case TextColor.Aqua:
                    return "aqua";

                case TextColor.Red:
                    return "red";

                case TextColor.LightPurple:
                    return "light_purple";

                case TextColor.Yellow:
                    return "yellow";

                case TextColor.White:
                    return "white";

                case TextColor.Obfuscated:
                    return "obfuscated";

                case TextColor.Bold:
                    return "bold";

                case TextColor.Strikethrough:
                    return "strikethrough";

                case TextColor.Underline:
                    return "underline";

                case TextColor.Italic:
                    return "italic";

                case TextColor.Reset:
                    return "reset";

                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        internal static HoverEvent.HoverAction ParseHover(string value)
        {
            switch (value)
            {
                case "show_text":
                    return HoverEvent.HoverAction.ShowText;

                case "show_item":
                    return HoverEvent.HoverAction.ShowItem;

                case "show_entity":
                    return HoverEvent.HoverAction.ShowEntity;

                case "show_achievement":
                    return HoverEvent.HoverAction.ShowAchievement;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        internal static ClickEvent.ClickAction ParseClick(string value)
        {
            switch (value)
            {
                case "open_url":
                    return ClickEvent.ClickAction.OpenUrl;

                case "run_command":
                    return ClickEvent.ClickAction.RunCommand;

                case "suggest_command":
                    return ClickEvent.ClickAction.SuggestCommand;

                case "change_page":
                    return ClickEvent.ClickAction.ChangePage;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        internal static Selector ParseSelector(string value)
        {
            switch (value)
            {
                case "@p":
                    return Selector.NearestPlayer;

                case "@r":
                    return Selector.RandomPlayer;

                case "@a":
                    return Selector.AllPlayers;

                case "@e":
                    return Selector.AllEntities;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        internal static TextColor ParseColor(string value)
        {
            switch (value)
            {
                case "black":
                    return TextColor.Black;

                case "dark_blue":
                    return TextColor.DarkBlue;

                case "dark_green":
                    return TextColor.DarkGreen;

                case "dark_aqua":
                    return TextColor.DarkAqua;

                case "dark_red":
                    return TextColor.DarkRed;

                case "dark_purple":
                    return TextColor.DarkPurple;

                case "gold":
                    return TextColor.Gold;

                case "gray":
                    return TextColor.Gray;

                case "dark_gray":
                    return TextColor.DarkGray;

                case "blue":
                    return TextColor.Blue;

                case "green":
                    return TextColor.Green;

                case "aqua":
                    return TextColor.Aqua;

                case "red":
                    return TextColor.Red;

                case "light_purple":
                    return TextColor.LightPurple;

                case "yellow":
                    return TextColor.Yellow;

                case "white":
                    return TextColor.White;

                case "obfuscated":
                    return TextColor.Obfuscated;

                case "bold":
                    return TextColor.Bold;

                case "strikethrough":
                    return TextColor.Strikethrough;

                case "underline":
                    return TextColor.Underline;

                case "italic":
                    return TextColor.Italic;

                case "reset":
                    return TextColor.Reset;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}