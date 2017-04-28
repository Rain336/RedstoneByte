using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RedstoneByte.Text;

namespace RedstoneByte.Utils
{
    [JsonConverter(typeof(StatusResponseConverter))]
    public sealed class StatusResponse
    {
        public ProtocolVersion Version { get; set; }

        public int MaxPlayers { get; set; }

        public int OnlinePlayers { get; set; }

        public readonly List<GameProfile> Sample = new List<GameProfile>();

        public TextBase Motd { get; set; }

        public string Favicon { get; set; }

        public StatusResponse Copy()
        {
            var result = new StatusResponse
            {
                Version = Version,
                Motd = Motd.Copy(),
                Favicon = Favicon,
                MaxPlayers = MaxPlayers,
                OnlinePlayers = OnlinePlayers
            };
            result.Sample.AddRange(Sample);
            return result;
        }

        public sealed class StatusResponseConverter : JsonConverter
        {
            public override bool CanConvert(Type type)
            {
                return typeof(StatusResponse) == type;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                var response = (StatusResponse) value;
                writer.WritePropertyName("version");
                serializer.Serialize(writer, response.Version);
                writer.WritePropertyName("players");
                writer.WriteStartObject();
                writer.WritePropertyName("max");
                writer.WriteValue(response.MaxPlayers);
                writer.WritePropertyName("online");
                writer.WriteValue(response.OnlinePlayers);
                if (response.Sample != null && response.Sample.Count > 0)
                {
                    writer.WritePropertyName("sample");
                    serializer.Serialize(writer, response.Sample);
                }
                writer.WriteEndObject();
                writer.WritePropertyName("description");
                serializer.Serialize(writer, response.Motd);
                if (string.IsNullOrEmpty(response.Favicon)) return;
                writer.WritePropertyName("favicon");
                writer.WriteValue(response.Favicon);
                writer.WriteEndObject();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                throw new NotImplementedException("Reading a Status Response is not implemented.");
            }

            public override bool CanRead => false;
        }
    }
}