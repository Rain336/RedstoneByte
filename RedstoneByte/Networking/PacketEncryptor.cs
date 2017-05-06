using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace RedstoneByte.Networking
{
    public sealed class PacketEncryptor : MessageToByteEncoder<IByteBuffer>
    {
        //public readonly ICryptoTransform Encryptor;
        public readonly BufferedBlockCipher Cipher;

        //public PacketEncryptor(ICryptoTransform encryptor)
        //{
        //    Encryptor = encryptor;
        //}

        public PacketEncryptor(byte[] secret)
        {
            Cipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            Cipher.Init(true, new ParametersWithIV(new KeyParameter(secret), secret, 0, 16));
        }

        protected override void Encode(IChannelHandlerContext context, IByteBuffer message, IByteBuffer output)
        {
            //Encryptor.TransformBlock(message.Array, 0, message.ReadableBytes, result.Array, 0);
            Cipher.ProcessBytes(message.Array, message.ArrayOffset + message.ReaderIndex, message.ReadableBytes,
                output.Array, output.ArrayOffset + output.WriterIndex);
            output.SetWriterIndex(output.WriterIndex + message.ReadableBytes);
            message.SkipBytes(message.ReadableBytes);
        }

        //public override Task CloseAsync(IChannelHandlerContext context)
        //{
        //    Encryptor.Dispose();
        //    return base.CloseAsync(context);
        //}

        //public override void HandlerRemoved(IChannelHandlerContext context)
        //{
        //    Encryptor.Dispose();
        //    base.HandlerRemoved(context);
        //}
    }
}