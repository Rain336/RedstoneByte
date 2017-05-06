using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace RedstoneByte.Networking
{
    public sealed class PacketDecryptor : MessageToMessageDecoder<IByteBuffer>
    {
        //public readonly ICryptoTransform Decryptor;
        public readonly BufferedBlockCipher Cipher;

        //public PacketDecryptor(ICryptoTransform decryptor)
        //{
        //    Decryptor = decryptor;
        //}

        public PacketDecryptor(byte[] secret)
        {
            Cipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            Cipher.Init(false, new ParametersWithIV(new KeyParameter(secret), secret, 0, 16));
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            var result = context.Allocator.Buffer(message.ReadableBytes);
            //Decryptor.TransformBlock(message.Array, 0, message.ReadableBytes, result.Array, 0);
            Cipher.ProcessBytes(message.Array, message.ArrayOffset + message.ReaderIndex, message.ReadableBytes,
                result.Array, result.ArrayOffset + result.WriterIndex);
            result.SetWriterIndex(result.WriterIndex + message.ReadableBytes);
            message.SkipBytes(message.ReadableBytes);
            output.Add(result);
        }

        //public override Task CloseAsync(IChannelHandlerContext context)
        //{
        //    Decryptor.Dispose();
        //    return base.CloseAsync(context);
        //}

        //public override void HandlerRemoved(IChannelHandlerContext context)
        //{
        //    Decryptor.Dispose();
        //    base.HandlerRemoved(context);
        //}
    }
}