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
        public IPEndPoint Address => (IPEndPoint) _channel.RemoteAddress;
        public IHandler Handler { get; set; }
        public readonly bool Client;
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

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Handler.OnDisconnect();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Handler.OnException(exception);
        }

        public Task SendPacketAsync(IPacket packet)
            => _channel.WriteAndFlushAsync(packet);

        public Task CloseConnectionAsync()
        {
            return _channel.CloseAsync();
        }

        public void SetupEncryption(byte[] secret)
        {
            //TODO: When .NET Core 2.0 comes out
            //TODO: replace BouncyCastle with System.Security.Cryptography
            //var aes = Aes.Create();
            //aes.Mode = CipherMode.CFB;
            //aes.Padding = PaddingMode.None;
            //aes.KeySize = 128;
            //aes.Key = secret;
            //aes.IV = secret;

            //_channel.Pipeline.AddBefore(PipelineUtils.SplitterId, PipelineUtils.DecryptorId,
            //    new PacketDecryptor(aes.CreateDecryptor()));
            //_channel.Pipeline.AddBefore(PipelineUtils.PrependerId, PipelineUtils.EncryptorId,
            //    new PacketEncryptor(aes.CreateEncryptor()));

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
    }

    public interface IHandler
    {
        void OnConnect();
        void OnPacket(IPacket packet);
        void OnDisconnect();
        void OnException(Exception exception);
    }
}