using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace RedstoneByte.Networking
{
    public sealed class Varint21FrameDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            input.MarkReaderIndex();
            var array = new byte[3];
            for (var i = 0; i < array.Length; i++)
            {
                if (!input.IsReadable())
                {
                    input.ResetReaderIndex();
                    return;
                }

                array[i] = input.ReadByte();

                if ((array[i] & 0x80) == 128) continue;

                var buffer = Unpooled.WrappedBuffer(array);
                var length = buffer.ReadVarInt();
                buffer.Release();

                if(length == 0)
                    throw new CorruptedFrameException("Empty Packet!");

                if (input.ReadableBytes < length)
                {
                    input.ResetReaderIndex();
                    return;
                }

                output.Add(input.Slice(input.ReaderIndex, length).Retain());
                input.SkipBytes(length);
                return;
            }
            throw new CorruptedFrameException("Invalid packet Frame!");
        }
    }
}