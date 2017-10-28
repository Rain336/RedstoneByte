using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using RedstoneByte.Networking;

namespace RedstoneByte
{
    public sealed class ServerInfo : IEquatable<ServerInfo>
    {
        public readonly IPEndPoint EndPoint;
        public readonly string Name;

        public ServerInfo(IPEndPoint endPoint, string name)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Task<IChannel> CreateConnectionAsync(Player player)
        {
            var bootstrap = new Bootstrap()
                .Group(NetworkManager.WorkerGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.ConnectTimeout, TimeSpan.FromMilliseconds(300))
                .Option(ChannelOption.WriteBufferHighWaterMark, 10485760)
                .Option(ChannelOption.WriteBufferLowWaterMark, 1048576)
                .Handler(new PipelineUtils.ServerChannelInitializer(this, player));
            return bootstrap.ConnectAsync(EndPoint);
        }

        public bool Equals(ServerInfo other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ServerInfo);
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(ServerInfo l, ServerInfo r)
        {
            return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
        }

        public static bool operator !=(ServerInfo l, ServerInfo r)
        {
            return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
        }
    }
}