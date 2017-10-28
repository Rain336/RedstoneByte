using System.Threading.Tasks;
using RedstoneByte.Networking;

namespace RedstoneByte
{
    public sealed class Server
    {
        public readonly ServerInfo Info;
        public readonly Player Player;
        public readonly PacketHandler Handler;

        public Server(ServerInfo info, Player player, PacketHandler handler)
        {
            Info = info;
            Player = player;
            Handler = handler;
        }

        public Task SendPacketAsync(IPacket packet)
            => Handler.SendPacketAsync(packet);

        public Task DisconnectAsync()
        {
            return Task.Delay(250).ContinueWith(t => Handler.Channel.CloseAsync());
        }
    }
}