using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedstoneByte.Networking;
using RedstoneByte.Networking.Packets;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte
{
    public sealed class Player
    {
        private readonly object _lock = new object();
        private readonly HashSet<ServerInfo> _pending = new HashSet<ServerInfo>(); //NOTE: USE LOCK!!!
        public readonly PacketHandler Handler;
        public readonly GameProfile Profile;
        public readonly bool Forge;

        public Guid Guid => Profile.Guid;
        public string Name => Profile.Name;

        public Server Server { get; private set; }
        public int ServerEntityId { get; set; }
        public int ClientEntityId { get; set; }

        public Player(PacketHandler handler, GameProfile profile, bool forge)
        {
            Handler = handler;
            Profile = profile;
            Forge = forge;
        }

        public Task SendPacketAsync(IPacket packet)
            => Handler.SendPacketAsync(packet);

        public Task DisconnectAsync(TextBase reason)
        {
            return Task.Delay(250)
                .ContinueWith(t => SendPacketAsync(new PacketDisconnect
                {
                    Reason = reason
                }))
                .ContinueWith(t => Handler.Channel.CloseAsync());
        }

        public async Task ConnectAsync(ServerInfo info)
        {
            if (Server != null && Server.Info == info)
                throw new InvalidOperationException("The Player is already connected to this Server.");

            lock (_lock)
            {
                if (_pending.Contains(info))
                    throw new InvalidOperationException("The Player is already connecting to this Server.");

                _pending.Add(info);
            }

            try
            {
                var channel = await info.CreateConnectionAsync(this);
            }
            catch (AggregateException)
            {
                ClosePendingConnection(info);
                throw;
            }
        }

        public void ClosePendingConnection(ServerInfo info)
        {
            lock (_lock)
                _pending.Remove(info);
        }

        public void SwitchServer(Server server)
        {
            Server?.DisconnectAsync();
            Server = server;
        }
    }
}