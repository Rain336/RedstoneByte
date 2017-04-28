using System;
using System.Text;
using DotNetty.Handlers.Timeout;
using NLog;
using RedstoneByte.Networking.Packets;
using RedstoneByte.Text;

namespace RedstoneByte.Networking
{
    public sealed class ServerStartupHandler : IHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public readonly PacketHandler Handler;
        public readonly ServerInfo Info;
        public readonly Player Player;
        private bool _disconnected;

        public ServerStartupHandler(PacketHandler handler, ServerInfo info, Player player)
        {
            Handler = handler;
            Info = info;
            Player = player;
        }

        public void OnConnect()
        {
            var builder = new StringBuilder(Info.EndPoint.Address.ToString());
            if (ProxyConfig.Instance.IpForward)
            {
                builder.Append('\0')
                    .Append(Player.Handler.Address.Address)
                    .Append('\0')
                    .Append(Player.Guid.ToString("N"));
            }
            if (Player.Forge)
                builder.Append("\0FML\0");
            Handler.SendPacketAsync(new PacketHandshake
                {
                    Version = RedstoneByte.ProtocolVersion.Id,
                    Address = builder.ToString(),
                    Port = (ushort) Info.EndPoint.Port,
                    Next = 2
                })
                .ContinueWith(t => Handler.State = ConnectionState.Login)
                .ContinueWith(t => Handler.SendPacketAsync(new PacketLoginStart
                {
                    Name = Player.Name
                }));
        }

        public void OnPacket(IPacket packet)
        {
            switch (packet)
            {
                case PacketEncryptionRequest request:
                    OnEncryptionRequest(request);
                    return;

                case PacketSetCompression compression:
                    OnSetCompression(compression);
                    return;

                case PacketLoginSuccess success:
                    OnLoginSuccess(success);
                    return;

                case PacketDisconnect disconnect:
                    OnDisconnect(disconnect);
                    return;
            }
        }

        private void OnDisconnect(PacketDisconnect disconnect)
        {
            _disconnected = true;
            Logger.Info("Server '{0}' disconnected us. How rude! {1}", Info.Name, disconnect.Reason);
            Player.DisconnectAsync(disconnect.Reason);
        }

        private void OnLoginSuccess(PacketLoginSuccess success)
        {
            Handler.State = ConnectionState.Play;
            var server = new Server(Info, Player, Handler);
            Handler.Handler = new DownstreamHandler(server, Player);
            Player.SwitchServer(server);
        }

        private void OnSetCompression(PacketSetCompression compression)
        {
            Handler.UpdateCompressionThreshold(compression.Threshold);
        }

        private static void OnEncryptionRequest(PacketEncryptionRequest request)
        {
            throw new InvalidOperationException("Server is in Online Mode!");
        }

        public void OnDisconnect()
        {
            if (_disconnected) return;
            Logger.Debug("Server '{0}' diconnected without a trace. :(", Info.Name);
            Player.DisconnectAsync(Texts.Of("System.IDontKnowException: The Server doesn't like you!"));
            //TODO: Translation :)
        }

        public void OnException(Exception exception)
        {
            _disconnected = true;
            if (exception is ReadTimeoutException)
            {
                Logger.Info(exception, "Server '{0}' timed out!", Info.Name);
            }
            else
            {
                Logger.Warn(exception, "Server '{0}' errored while Connecting!", Info.Name);
            }
            Player.DisconnectAsync(Texts.Of(exception.Message));
        }
    }
}