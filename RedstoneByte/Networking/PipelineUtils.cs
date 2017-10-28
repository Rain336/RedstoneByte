using System;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace RedstoneByte.Networking
{
    public static class PipelineUtils
    {
        public const string LagacyId = "lagacy";
        public static readonly LagacyPingHandler Lagacy = new LagacyPingHandler();

        public const string PrependerId = "prepender";
        public static readonly Varint21FrameEncoder Prepender = new Varint21FrameEncoder();

        public const string DecompressorId = "decompressor";
        public static readonly PacketDecompressor Decompressor = new PacketDecompressor();

        public const string TimeoutId = "timeout";
        public const string SplitterId = "splitter";
        public const string DecoderId = "decoder";
        public const string EncoderId = "encoder";
        public const string HandlerId = "handler";
        public const string EncryptorId = "encrypter";
        public const string DecryptorId = "decrypter";
        public const string CompressorId = "compressor";

        public static readonly ActionChannelInitializer<ISocketChannel> ClientInitializer =
            new ActionChannelInitializer<ISocketChannel>(channel =>
            {
                try
                {
                    channel.Configuration.SetOption(ChannelOption.IpTos, 0x18);
                }
                catch (ChannelException)
                {
                }
                channel.Configuration.SetOption(ChannelOption.TcpNodelay, true);
                channel.Configuration.Allocator = PooledByteBufferAllocator.Default;

                var handler = new PacketHandler(false);
                handler.Handler = new ClientStartupHandler(handler);

                channel.Pipeline
                    .AddLast(TimeoutId, new ReadTimeoutHandler(RedstoneByte.Config.Networking.ReadTimeout))
                    .AddLast(LagacyId, Lagacy)
                    .AddLast(SplitterId, new Varint21FrameDecoder())
                    .AddLast(DecoderId, new PacketDecoder())
                    .AddLast(PrependerId, Prepender)
                    .AddLast(EncoderId, new PacketEncoder())
                    .AddLast(HandlerId, handler);
            });

        public sealed class ServerChannelInitializer : ChannelInitializer<ISocketChannel>
        {
            public readonly ServerInfo Info;
            public readonly Player Player;

            public ServerChannelInitializer(ServerInfo info, Player player)
            {
                Info = info ?? throw new ArgumentNullException(nameof(info));
                Player = player ?? throw new ArgumentNullException(nameof(player));
            }

            protected override void InitChannel(ISocketChannel channel)
            {
                try
                {
                    channel.Configuration.SetOption(ChannelOption.IpTos, 0x18);
                }
                catch (ChannelException)
                {
                }
                channel.Configuration.SetOption(ChannelOption.TcpNodelay, true);
                channel.Configuration.Allocator = PooledByteBufferAllocator.Default;

                var handler = new PacketHandler(true);
                handler.Handler = new ServerStartupHandler(handler, Info, Player);

                channel.Pipeline
                    .AddLast(TimeoutId, new ReadTimeoutHandler(RedstoneByte.Config.Networking.ReadTimeout))
                    .AddLast(SplitterId, new Varint21FrameDecoder())
                    .AddLast(DecoderId, new PacketDecoder())
                    .AddLast(PrependerId, Prepender)
                    .AddLast(EncoderId, new PacketEncoder())
                    .AddLast(HandlerId, handler);
            }
        }
    }
}