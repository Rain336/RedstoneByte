using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace RedstoneByte.Networking
{
    public sealed class Varint21FrameEncoder : MessageToByteEncoder<IByteBuffer>
    {
        public override bool IsSharable => true;

        protected override void Encode(IChannelHandlerContext context, IByteBuffer message, IByteBuffer output)
        {
            var varint = VarIntSize(message.ReadableBytes);
            output.EnsureWritable(message.ReadableBytes + varint);
            output.WriteVarInt(message.ReadableBytes);
            output.WriteBytes(message);
        }

        private static int VarIntSize(int paramInt)
        {
            if ((paramInt & 0xFFFFFF80) == 0) return 1;
            if ((paramInt & 0xFFFFC000) == 0) return 2;
            if ((paramInt & 0xFFE00000) == 0) return 3;
            if ((paramInt & 0xF0000000) == 0) return 4;
            return 5;
        }
    }
}