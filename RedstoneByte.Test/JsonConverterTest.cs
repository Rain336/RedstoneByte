using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte.Test
{
    [TestClass]
    public static class JsonConverterTest
    {
        public static readonly StatusResponse ResponseDummy = new StatusResponse
        {
            Version = ProtocolVersion.V189,
            MaxPlayers = 500,
            OnlinePlayers = 560,
            Sample =
            {
                new GameProfile("OfflineUser", Guid.Empty)
            },
            Motd = Texts.Of("Unittest!"),
            Favicon = null
        };

        public const string TextJsonDummy =
                "{\"translate\":\"chat.type.text\",\"with\":[{\"text\":\"Herobrine\",\"clickEvent\":{\"action\":\"suggest_command\",\"value\":\"/msg Herobrine \"},\"hoverEvent\":{\"action\":\"show_entity\",\"value\":\"{id:f84c6a79-0a4e-45e0-879b-cd49ebd4c4e2,name:Herobrine}\"},\"insertion\":\"Herobrine\"},{\"text\":\"I don\'t exist\"}]}";

        public static readonly TextBase TextDummy = new TranslationText("chat.type.text")
        {
            With =
            {
                new StringText("Herobrine")
                {
                    ClickEvent = new ClickEvent(ClickEvent.ClickAction.SuggestCommand, "/msg Herobrine "),
                    HoverEvent = new HoverEvent(HoverEvent.HoverAction.ShowEntity,
                        "{id:f84c6a79-0a4e-45e0-879b-cd49ebd4c4e2,name:Herobrine}"),
                    Insertion = "Herobrine"
                },
                new StringText("I don't exist")
            }
        };

        [TestMethod]
        public static void StatusResponse_JsonWrite()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(ResponseDummy));

            Assert.AreEqual(json["version"]["protocol"].Value<int>(), 47);
            Assert.AreEqual(json["version"]["name"].Value<string>(), "1.8.9");

            Assert.AreEqual(json["players"]["max"].Value<int>(), 500);
            Assert.AreEqual(json["players"]["online"].Value<int>(), 560);

            var profiles = json["players"]["sample"].ToObject<List<GameProfile>>();
            Assert.AreEqual(profiles.Count, 1);
            Assert.AreEqual(profiles[0].Name, "OfflineUser");
            Assert.AreEqual(profiles[0].Guid, Guid.Empty);

            var text = json["description"].ToObject<TextBase>();
            Assert.AreEqual(text.ToPlain(), "Unittest!");
            Assert.IsNull(json["favicon"]);
        }

        [TestMethod]
        public static void TextBase_JsonRead()
        {
            var text = JsonConvert.DeserializeObject<TextBase>(TextJsonDummy);
            var translation = text as TranslationText;

            Assert.IsNotNull(translation);
            Assert.AreEqual(translation.With.Count, 2);

            var name = translation.With[0] as StringText;

            Assert.IsNotNull(name);
            Assert.AreEqual(name.Text, "Herobrine");
            Assert.AreEqual(name.Insertion, "Herobrine");

            Assert.IsNotNull(name.ClickEvent);
            Assert.AreEqual(name.ClickEvent.Action, ClickEvent.ClickAction.SuggestCommand);
            Assert.AreEqual(name.ClickEvent.Value, "/msg Herobrine ");

            Assert.IsNotNull(name.HoverEvent);
            Assert.AreEqual(name.HoverEvent.Action, HoverEvent.HoverAction.ShowEntity);
            Assert.AreEqual(name.HoverEvent.Value, "{id:f84c6a79-0a4e-45e0-879b-cd49ebd4c4e2,name:Herobrine}");

            var message = translation.With[1];

            Assert.IsInstanceOfType(message, typeof(StringText));
            Assert.AreEqual(message.ToPlain(), "I don't exist");
        }

        [TestMethod]
        public static void TextBase_JsonWrite()
        {
            var json = JsonConvert.SerializeObject(TextDummy);
        }
    }
}