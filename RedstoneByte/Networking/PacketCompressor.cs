using System.IO;
using System.IO.Compression;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace RedstoneByte.Networking
{
    public sealed class PacketCompressor : MessageToByteEncoder<IByteBuffer>
    {
        public int Threshold;

        public PacketCompressor(int threshold)
        {
            Threshold = threshold;
        }

        protected override void Encode(IChannelHandlerContext context, IByteBuffer message, IByteBuffer output)
        {
            if (message.ReadableBytes < Threshold)
            {
                output.WriteVarInt(0);
                output.WriteBytes(message);
            }
            else
            {
                output.WriteVarInt(message.ReadableBytes);
                using (var stream = new DeflateStream(
                    new MemoryStream(output.Array, output.ReaderIndex, output.ReadableBytes, true),
                    CompressionMode.Compress))
                {
                    stream.Write(message.Array, message.ReaderIndex, message.ReadableBytes);
                }
                message.SetReaderIndex(message.ReadableBytes);
            }
        }
    }
}