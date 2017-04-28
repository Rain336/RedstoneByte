using System;
using System.Collections.Generic;
using System.Linq;

namespace RedstoneByte.Utils
{
    public static class PlayerList
    {
        private static readonly Dictionary<Guid, Player> Players = new Dictionary<Guid, Player>();
        private static readonly object Lock = new object();

        public static int Count
        {
            get
            {
                lock (Lock)
                {
                    return Players.Count;
                }
            }
        }

        public static void AddPlayer(Player player)
        {
            lock (Lock)
            {
                Players.Add(player.Guid, player);
            }
        }

        public static Player GetPlayer(Guid guid)
        {
            lock (Lock)
            {
                return Players.TryGetValue(guid, out var result) ? result : null;
            }
        }

        public static Player GetPlayer(string name)
        {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            lock (Lock)
            {
                return Players.Values.FirstOrDefault(player => player.Name == name);
            }
        }

        public static void RemovePlayer(Player player)
            => RemovePlayer(player.Guid);

        public static void RemovePlayer(Guid guid)
        {
            lock (Lock)
            {
                Players.Remove(guid);
            }
        }
    }
}