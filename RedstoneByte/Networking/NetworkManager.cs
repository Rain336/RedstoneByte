using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace RedstoneByte.Networking
{
    public static class NetworkManager
    {
        public static readonly MultithreadEventLoopGroup BossGroup = new MultithreadEventLoopGroup();
        public static readonly MultithreadEventLoopGroup WorkerGroup = new MultithreadEventLoopGroup();

        public static IChannel Channel { get; private set; }

        public static async Task Run(EndPoint addr)
        {
            var bootstrap = new ServerBootstrap()
                .Group(BossGroup, WorkerGroup)
                .Option(ChannelOption.SoReuseaddr, true)
                .ChildOption(ChannelOption.WriteBufferHighWaterMark, 10485760)
                .ChildOption(ChannelOption.WriteBufferLowWaterMark, 1048576)
                .Channel<TcpServerSocketChannel>()
                .ChildHandler(PipelineUtils.ClientInitializer);

            Channel = await bootstrap.BindAsync(addr).ConfigureAwait(false);
        }

        public static async Task Stop()
        {
            await Channel.CloseAsync().ConfigureAwait(false);
            await Task.WhenAll(BossGroup.ShutdownGracefullyAsync(), WorkerGroup.ShutdownGracefullyAsync())
                .ConfigureAwait(false);
        }
    }
}