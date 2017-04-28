using System;
using DotNetty.Handlers.Timeout;
using NLog;
using RedstoneByte.Text;

namespace RedstoneByte.Networking
{
    public sealed class DownstreamHandler : IHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public readonly Server Server;
        public readonly Player Player;
        private bool _disconnected;

        public DownstreamHandler(Server server, Player player)
        {
            Server = server;
            Player = player;
        }

        public void OnConnect()
        {
        }

        public void OnPacket(IPacket packet)
        {
            PatchEntityId(packet as IEntityPacket);
            Player.SendPacketAsync(packet);
        }

        public void OnDisconnect()
        {
            if (_disconnected) return;
            Logger.Debug("Server '{0}' diconnected without a trace. :(", Server.Info.Name);
            Player.DisconnectAsync(Texts.Of("System.IDontKnowException: The Server doesn't like you!"));
            //TODO: Translation :)
        }

        public void OnException(Exception exception)
        {
            _disconnected = true;
            if (exception is ReadTimeoutException)
            {
                Logger.Info(exception, "Server '{0}' timed out!", Server.Info.Name);
            }
            else
            {
                Logger.Warn(exception, "Server '{0}' errored while Connecting!", Server.Info.Name);
            }
            Player.DisconnectAsync(Texts.Of(exception.Message));
        }

        private void PatchEntityId(IEntityPacket packet)
        {
            if (packet != null && packet.EntityId == Player.ServerEntityId)
                packet.EntityId = Player.ClientEntityId;
        }
    }
}