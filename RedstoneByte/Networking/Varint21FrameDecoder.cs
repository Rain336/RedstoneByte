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
            var array = new byte[3];
            for (var i = 0; i < array.Length; i++)
            {
                if (!input.IsReadable())
                {
                    input.ResetReaderIndex();
                    return;
                }

                array[i] = input.ReadByte();

                if ((array[i] & 0x80) != 0) continue;

                var buffer = Unpooled.WrappedBuffer(array);
                var length = buffer.ReadVarInt();
                buffer.Release();

                if(length == 0)
                    throw new CorruptedFrameException("Empty Packet!");

                if (input.ReadableBytes >= length)
                    output.Add(input.ReadBytes(length));
                return;
            }
            throw new CorruptedFrameException("Invalid packet Frame!");
        }
    }
}