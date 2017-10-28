using System;
using System.Collections.Generic;
using System.Linq;

namespace RedstoneByte.Utils
{
    public static class PlayerList
    {
        private static readonly Dictionary<Guid, Player> _players = new Dictionary<Guid, Player>();
        private static readonly object _lock = new object();

        public static int Count
        {
            get
            {
                lock (_lock)
                {
                    return _players.Count;
                }
            }
        }

        public static void AddPlayer(Player player)
        {
            lock (_lock)
            {
                _players.Add(player.Guid, player);
            }
        }

        public static Player GetPlayer(Guid guid)
        {
            lock (_lock)
            {
                return _players.TryGetValue(guid, out var result) ? result : null;
            }
        }

        public static Player GetPlayer(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            lock (_lock)
            {
                return _players.Values.FirstOrDefault(player => player.Name == name);
            }
        }

        public static void RemovePlayer(Player player)
            => RemovePlayer(player.Guid);

        public static void RemovePlayer(Guid guid)
        {
            lock (_lock)
            {
                _players.Remove(guid);
            }
        }
    }
}