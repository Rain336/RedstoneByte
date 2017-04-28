using System;
using System.Collections.Generic;
using DotNetty.Handlers.Timeout;
using NLog;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public sealed class UpstreamHandler : IHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public readonly Player Player;
        private readonly List<IPacket> _buffer = new List<IPacket>();

        public UpstreamHandler(Player player)
        {
            Player = player;
        }

        public void ApplyBuffer()
        {
            if (Player.Server == null) return;
            foreach (var packet in _buffer)
            {
                OnPacket(packet);
            }
        }

        public void OnConnect()
        {
        }

        public void OnPacket(IPacket packet)
        {
            if (Player.Server == null)
            {
                _buffer.Add(packet);
                return;
            }
            PatchEntityId(packet as IEntityPacket);
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

        private void PatchEntityId(IEntityPacket packet)
        {
            if (packet != null && packet.EntityId == Player.ClientEntityId)
                packet.EntityId = Player.ServerEntityId;
        }
    }
}