using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace RedstoneByte.Utils
{
    public static class ServerQueue
    {
        public const string Filename = "ServerQueue.json";
        private static readonly List<ServerInfo> Queue = new List<ServerInfo>();
        private static readonly Dictionary<string, ServerInfo> Servers = new Dictionary<string, ServerInfo>();
        private static readonly object Lock = new object();

        public static ServerInfo First
        {
            get
            {
                lock (Lock)
                {
                    return Queue[0];
                }
            }
        }

        public static void LoadFromFile()
        {
            if (!File.Exists(Filename)) return;
            var json = JObject.Parse(File.ReadAllText(Filename));
            lock (Lock)
            {
                foreach (var entry in json)
                {
                    var info = new ServerInfo(
                        new IPEndPoint(IPAddress.Parse(entry.Value["Address"].Value<string>()),
                            entry.Value["Port"].Value<int>()), entry.Key);
                    Queue.Add(info);
                    Servers.Add(info.Name, info);
                }
            }
        }

        public static void WriteToFile()
        {
            var json = new JObject();
            lock (Lock)
            {
                foreach (var info in Queue)
                {
                    json.Add(info.Name, JObject.FromObject(new
                    {
                        Address = info.EndPoint.Address.ToString(),
                        Port = info.EndPoint.Port
                    }));
                }
            }
            File.WriteAllText(Filename, json.ToString());
        }

        public static void AddLast(ServerInfo info)
        {
            lock (Lock)
            {
                Queue.Add(info);
                Servers.Add(info.Name, info);
            }
        }

        public static void AddFirst(ServerInfo info)
        {
            lock (Lock)
            {
                Queue.Insert(0, info);
                Servers.Add(info.Name, info);
            }
        }

        public static void AddBefore(ServerInfo before, ServerInfo info)
        {
            lock (Lock)
            {
                Queue.Insert(Queue.IndexOf(before), info);
                Servers.Add(info.Name, info);
            }
        }

        public static void AddAfter(ServerInfo after, ServerInfo info)
        {
            lock (Lock)
            {
                Queue.Insert(Queue.IndexOf(after) + 1, info);
                Servers.Add(info.Name, info);
            }
        }

        public static void Remove(ServerInfo info)
        {
            lock (Lock)
            {
                Servers.Remove(info.Name);
                Queue.Remove(info);
            }
        }

        public static void Remove(string name)
        {
            lock (Lock)
            {
                Queue.Remove(Servers[name]);
                Servers.Remove(name);
            }
        }

        public static ServerInfo Get(string name)
        {
            lock (Lock)
            {
                return Servers.TryGetValue(name, out var result) ? result : null;
            }
        }

        public static ServerInfo GetBefore(ServerInfo info)
        {
            lock (Lock)
            {
                return Queue[Queue.IndexOf(info) - 1];
            }
        }

        public static ServerInfo GetAfter(ServerInfo info)
        {
            lock (Lock)
            {
                return Queue[Queue.IndexOf(info) + 1];
            }
        }

        public static ServerInfo GetBefore(string name)
        {
            lock (Lock)
            {
                return Queue[Queue.IndexOf(Servers[name]) - 1];
            }
        }

        public static ServerInfo GetAfter(string name)
        {
            lock (Lock)
            {
                return Queue[Queue.IndexOf(Servers[name]) + 1];
            }
        }
    }
}