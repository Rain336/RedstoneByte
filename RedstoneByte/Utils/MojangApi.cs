using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedstoneByte.Utils
{
    public static class MojangApi
    {
        private const string HasJoinedUrl =
            "https://sessionserver.mojang.com/session/minecraft/hasJoined?username={0}&serverId={1}";

        public static async Task<GameProfile> HasJoined(string name, string hash)
        {
            var resquest = WebRequest.CreateHttp(string.Format(HasJoinedUrl, name, hash));
            var response = await resquest.GetResponseAsync();

            JObject json;
            using (var stream = response.GetResponseStream())
            {
                json = await JObject.LoadAsync(new JsonTextReader(new StreamReader(stream)));
            }

            var result = new GameProfile(json["name"].Value<string>(), Guid.Parse(json["id"].Value<string>()));
            foreach (var property in json["properties"])
            {
                result.Properties.Add(new GameProfile.Property(property["name"].Value<string>(),
                    property["value"].Value<string>(), property["signature"]?.Value<string>()));
            }
            return result;
        }
    }
}