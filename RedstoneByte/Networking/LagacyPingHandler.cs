using DotNetty.Buffers;
using DotNetty.Transport.Channels;

namespace RedstoneByte.Networking
{
    public sealed class LagacyPingHandler : SimpleChannelInboundHandler<IByteBuffer>
    {
        public override bool IsSharable => true;

        protected override void ChannelRead0(IChannelHandlerContext ctx, IByteBuffer msg)
        {
            if (!msg.IsReadable()) return;

            msg.MarkReaderIndex();
            if (msg.ReadByte() == 0xFE)
            {
                var version = msg.ReadLagacyPing();
                msg.Release();
                var buffer = ctx.Allocator.Buffer();
                buffer.WriteLagacyPing(version);
                ctx.WriteAndFlushAsync(buffer).ContinueWith(t => ctx.CloseAsync());
                return;
            }
            msg.ResetReaderIndex();
            ctx.Channel.Pipeline.Remove(this);
            ctx.FireChannelRead(msg);
        }
    }
}