using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using RedstoneByte.Native;

namespace RedstoneByte.Networking
{
    public sealed class PacketEncryptor : MessageToByteEncoder<IByteBuffer>
    {
        public readonly NativeCipher Cipher;

        public PacketEncryptor(byte[] key)
        {
            Cipher = new NativeCipher(true);
            Cipher.Init(key, key);
        }

        protected override void Encode(IChannelHandlerContext context, IByteBuffer message, IByteBuffer output)
        {
            Cipher.Process(message.Array, message.ArrayOffset + message.ReaderIndex, message.ReadableBytes,
                output.Array, output.ArrayOffset + output.WriterIndex);
            output.SetWriterIndex(output.WriterIndex + message.ReadableBytes);
            message.SkipBytes(message.ReadableBytes);
        }

        public override Task CloseAsync(IChannelHandlerContext context)
        {
            Cipher.Dispose();
            return base.CloseAsync(context);
        }

        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            Cipher.Dispose();
            base.HandlerRemoved(context);
        }
    }
}