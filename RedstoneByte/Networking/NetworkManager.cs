using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace RedstoneByte.Networking
{
    public static class NetworkManager
    {
        private static int _workerId = 0;

        public static readonly MultithreadEventLoopGroup BossGroup =
            new MultithreadEventLoopGroup(group => new SingleThreadEventLoop(group, "DotNetty Acceptor"), 1);

        public static readonly MultithreadEventLoopGroup WorkerGroup = new MultithreadEventLoopGroup(
            group => new SingleThreadEventLoop(group, "DotNetty Worker #" + Interlocked.Increment(ref _workerId)));

        public static IChannel Channel { get; private set; }

        public static async Task Run(EndPoint addr)
        {
            var bootstrap = new ServerBootstrap()
                .Group(BossGroup, WorkerGroup)
                .Option(ChannelOption.SoReuseaddr, true)
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