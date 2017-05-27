using System;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotNetty.Common.Concurrency;
using DotNetty.Transport.Channels;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public sealed class PacketHandler : SimpleChannelInboundHandler<IPacket>
    {
        private static readonly FieldInfo ModeValue = typeof(SymmetricAlgorithm).GetField("ModeValue");
        public ConnectionState State = ConnectionState.Handshaking;
        public ProtocolVersion Version = RedstoneByte.ProtocolVersion;
        public IPEndPoint Address => (IPEndPoint) _channel.RemoteAddress;
        public IHandler Handler { get; set; }
        public readonly bool Client;
        private readonly ConcurrentQueue<BufferedPacket> _buffer = new ConcurrentQueue<BufferedPacket>();
        private IChannel _channel;

        public PacketHandler(bool client)
        {
            Client = client;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            context.Channel.Pipeline.Get<PacketDecoder>().Handler = this;
            context.Channel.Pipeline.Get<PacketEncoder>().Handler = this;
            _channel = context.Channel;
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

        public override void ChannelWritabilityChanged(IChannelHandlerContext context)
        {
            while (_channel.IsWritable)
            {
                BufferedPacket packet;
                if (!_buffer.TryDequeue(out packet)) return;
                context.WriteAsync(packet.Packet)
                    .ContinueWith(t => packet.Promise.Complete());
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Handler.OnException(exception);
        }

        public void Flush()
            => _channel.Flush();

        public Task SendPacketAsync(IPacket packet)
        {
            if (_channel.IsWritable)
            {
#if DEBUG
                var watch = System.Diagnostics.Stopwatch.StartNew();
#endif
                return _channel.WriteAsync(packet)
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
            var buffer = new BufferedPacket(packet);
            _buffer.Enqueue(buffer);
            return buffer.Promise.Task;
        }

        public Task CloseConnectionAsync()
        {
            return _channel.CloseAsync();
        }

        public void SetupEncryption(byte[] secret)
        {
            _channel.Pipeline.AddBefore(PipelineUtils.SplitterId, PipelineUtils.DecryptorId,
                new PacketDecryptor(secret));
            _channel.Pipeline.AddBefore(PipelineUtils.PrependerId, PipelineUtils.EncryptorId,
                new PacketEncryptor(secret));
        }

        public void UpdateCompressionThreshold(int threshold)
        {
            if (_channel.Pipeline.Get<PacketCompressor>() == null && threshold >= 0)
            {
                _channel.Pipeline.AddBefore(PipelineUtils.EncoderId, PipelineUtils.CompressorId,
                    new PacketCompressor(threshold));
            }
            else if (threshold < 0)
            {
                _channel.Pipeline.Remove<PacketCompressor>();
            }
            else
            {
                _channel.Pipeline.Get<PacketCompressor>().Threshold = threshold;
            }

            if (_channel.Pipeline.Get<PacketDecompressor>() == null && threshold >= 0)
            {
                _channel.Pipeline.AddBefore(PipelineUtils.DecoderId, PipelineUtils.DecompressorId,
                    PipelineUtils.Decompressor);
            }
            else
            {
                _channel.Pipeline.Remove<PacketDecompressor>();
            }
        }

        private sealed class BufferedPacket
        {
            public readonly TaskCompletionSource Promise = new TaskCompletionSource();
            public readonly IPacket Packet;

            public BufferedPacket(IPacket packet)
            {
                Packet = packet;
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