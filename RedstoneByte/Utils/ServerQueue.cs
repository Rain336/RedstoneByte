using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace RedstoneByte.Utils
{
    public static class ServerQueue
    {
        public const string Filename = "ServerQueue.json";
        private static readonly List<ServerInfo> _queue = new List<ServerInfo>();
        private static readonly Dictionary<string, ServerInfo> _servers = new Dictionary<string, ServerInfo>();
        private static readonly object _lock = new object();

        public static ServerInfo First
        {
            get
            {
                lock (_lock)
                {
                    if (_queue.Count == 0) return null;
                    return _queue[0];
                }
            }
        }

        internal static void LoadFromFile()
        {
            if (!File.Exists(Filename)) return;
            var json = JObject.Parse(File.ReadAllText(Filename));
            lock (_lock)
            {
                foreach (var entry in json)
                {
                    var info = new ServerInfo(
                        new IPEndPoint(IPAddress.Parse(entry.Value["Address"].Value<string>()),
                            entry.Value["Port"].Value<int>()), entry.Key);
                    _queue.Add(info);
                    _servers.Add(info.Name, info);
                }
            }
        }

        internal static void WriteToFile()
        {
            var json = new JObject();
            lock (_lock)
            {
                foreach (var info in _queue)
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
            lock (_lock)
            {
                _queue.Add(info);
                _servers.Add(info.Name, info);
            }
        }

        public static void AddFirst(ServerInfo info)
        {
            lock (_lock)
            {
                _queue.Insert(0, info);
                _servers.Add(info.Name, info);
            }
        }

        public static void AddBefore(ServerInfo before, ServerInfo info)
        {
            lock (_lock)
            {
                _queue.Insert(_queue.IndexOf(before), info);
                _servers.Add(info.Name, info);
            }
        }

        public static void AddAfter(ServerInfo after, ServerInfo info)
        {
            lock (_lock)
            {
                _queue.Insert(_queue.IndexOf(after) + 1, info);
                _servers.Add(info.Name, info);
            }
        }

        public static void Remove(ServerInfo info)
        {
            lock (_lock)
            {
                _servers.Remove(info.Name);
                _queue.Remove(info);
            }
        }

        public static void Remove(string name)
        {
            lock (_lock)
            {
                _queue.Remove(_servers[name]);
                _servers.Remove(name);
            }
        }

        public static ServerInfo Get(string name)
        {
            lock (_lock)
            {
                return _servers.TryGetValue(name, out var result) ? result : null;
            }
        }

        public static ServerInfo GetBefore(ServerInfo info)
        {
            lock (_lock)
            {
                return _queue[_queue.IndexOf(info) - 1];
            }
        }

        public static ServerInfo GetAfter(ServerInfo info)
        {
            lock (_lock)
            {
                return _queue[_queue.IndexOf(info) + 1];
            }
        }

        public static ServerInfo GetBefore(string name)
        {
            lock (_lock)
            {
                return _queue[_queue.IndexOf(_servers[name]) - 1];
            }
        }

        public static ServerInfo GetAfter(string name)
        {
            lock (_lock)
            {
                return _queue[_queue.IndexOf(_servers[name]) + 1];
            }
        }
    }
}