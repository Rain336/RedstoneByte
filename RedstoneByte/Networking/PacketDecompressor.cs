using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace RedstoneByte.Networking
{
    public sealed class PacketDecompressor : MessageToMessageDecoder<IByteBuffer>
    {
        public override bool IsSharable => true;

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            var size = message.ReadVarInt();
            if (size == 0)
            {
                output.Add(message.Slice().Retain());
                message.SkipBytes(message.ReadableBytes);
            }
            else
            {
                var result = context.Allocator.Buffer(size);
                using (var stream = new DeflateStream(
                    new MemoryStream(message.Array, message.ArrayOffset + message.ReaderIndex, message.ReadableBytes),
                    CompressionMode.Decompress))
                {
                    stream.Read(result.Array, result.ArrayOffset + result.WriterIndex, message.ReadableBytes);
                }
                result.SetWriterIndex(result.WriterIndex + message.ReadableBytes);
                message.SkipBytes(message.ReadableBytes);
            }
        }
    }
}