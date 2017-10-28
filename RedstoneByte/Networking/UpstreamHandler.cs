using System;
using DotNetty.Handlers.Timeout;
using NLog;
using RedstoneByte.Utils;
using RedstoneByte.Networking.Packets;

namespace RedstoneByte.Networking
{
    public sealed class UpstreamHandler : IHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public readonly Player Player;

        public UpstreamHandler(Player player)
        {
            Player = player;
        }

        public void OnConnect()
        {
        }

        public void OnPacket(IPacket packet)
        {
            if (Player.Server == null) return;
            switch (packet)
            {
                case PacketKeepAlive alive:
                    return;
            }
            PatchEntityId(packet as EntityPacket);
            Player.Server.SendPacketAsync(packet);
        }

        public void OnDisconnect()
        {
            Player.Server?.DisconnectAsync();
            PlayerList.RemovePlayer(Player);
        }

        public void OnException(Exception exception)
        {
            if (exception is ReadTimeoutException)
            {
                Logger.Info(exception, "'{0}' timed out!", Player.Name);
            }
            else
            {
                Logger.Warn(exception, "'{0}' errored while Connecting!", Player.Name);
            }
        }

        private void PatchEntityId(EntityPacket packet)
        {
            packet?.CompareSet(Player.ClientEntityId, Player.ServerEntityId);
        }
    }
}