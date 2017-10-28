using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public sealed class PacketHandler : SimpleChannelInboundHandler<IPacket>
    {
        public ConnectionState State = ConnectionState.Handshaking;
        public ProtocolVersion Version = RedstoneByte.ProtocolVersion;
        public IPEndPoint EndPoint => (IPEndPoint)Channel.RemoteAddress;
        public IHandler Handler { get; set; }
        public readonly bool Client;
        public IChannel Channel { get; private set; }

        public PacketHandler(bool client)
        {
            Client = client;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            context.Channel.Pipeline.Get<PacketDecoder>().Handler = this;
            context.Channel.Pipeline.Get<PacketEncoder>().Handler = this;
            Channel = context.Channel;
            Handler.OnConnect();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IPacket msg)
        {
            Handler.OnPacket(msg);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Handler.OnDisconnect();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Handler.OnException(exception);
        }

        public Task SendPacketAsync(IPacket packet)
        {
#if DEBUG
            var watch = System.Diagnostics.Stopwatch.StartNew();
#endif
            return Channel.WriteAndFlushAsync(packet)
#if DEBUG
                .ContinueWith(t =>
                {
                    watch.Stop();
                    RedstoneByte.Logger.Debug(Handler.GetType().Name + " =OUT=> " +
                                              packet.GetType().Name + '(' + watch.ElapsedMilliseconds + "ms)");
                })
#endif
                ;
        }

        public void SetupEncryption(byte[] secret)
        {
            Channel.Pipeline.AddBefore(PipelineUtils.SplitterId, PipelineUtils.DecryptorId, new PacketDecryptor(secret));
            Channel.Pipeline.AddBefore(PipelineUtils.PrependerId, PipelineUtils.EncryptorId, new PacketEncryptor(secret));
        }

        public void UpdateCompressionThreshold(int threshold)
        {
            if (Channel.Pipeline.Get<PacketCompressor>() == null && threshold >= 0)
            {
                Channel.Pipeline.AddBefore(PipelineUtils.EncoderId, PipelineUtils.CompressorId, new PacketCompressor(threshold));
            }
            else if (threshold < 0)
            {
                Channel.Pipeline.Remove<PacketCompressor>();
            }
            else
            {
                Channel.Pipeline.Get<PacketCompressor>().Threshold = threshold;
            }

            if (Channel.Pipeline.Get<PacketDecompressor>() == null && threshold >= 0)
            {
                Channel.Pipeline.AddBefore(PipelineUtils.DecoderId, PipelineUtils.DecompressorId,
                    PipelineUtils.Decompressor);
            }
            else
            {
                Channel.Pipeline.Remove<PacketDecompressor>();
            }
        }
    }

    public interface IHandler
    {
        void OnConnect();
        void OnPacket(IPacket packet);
        void OnDisconnect();
        void OnException(Exception exception);
    }
}