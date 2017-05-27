using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using RedstoneByte.Native;

namespace RedstoneByte.Networking
{
    public sealed class PacketDecryptor : MessageToMessageDecoder<IByteBuffer>
    {
        public readonly NativeCipher Cipher;

        public PacketDecryptor(byte[] key)
        {
            Cipher = new NativeCipher(false);
            Cipher.Init(key, key);
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            var result = context.Allocator.Buffer(message.ReadableBytes);
            Cipher.Process(message.Array, message.ArrayOffset + message.ReaderIndex, message.ReadableBytes,
                result.Array, result.ArrayOffset + result.WriterIndex);
            result.SetWriterIndex(result.WriterIndex + message.ReadableBytes);
            message.SkipBytes(message.ReadableBytes);
            output.Add(result);
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