using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using RedstoneByte.Networking.Packets;

namespace RedstoneByte.Networking
{
    public sealed class PacketEncoder : MessageToByteEncoder<IPacket>
    {
        public PacketHandler Handler { get; set; }

        protected override void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
        {
            var unknowen = message as PacketUnknowen;
            output.WriteVarInt(unknowen?.PacketId ??
                               PacketRegistry.GetId(!Handler.Client, Handler.State, Handler.Version,
                                   message.GetType()));
            message.WriteToBuffer(output, Handler.Version);
#if DEBUG
            RedstoneByte.Logger.Debug(Handler.Handler.GetType().Name + " =OUT=> " + message.GetType().Name);
            RedstoneByte.Logger.Debug(
                System.BitConverter.ToString(output.Array, output.ReaderIndex, output.ReadableBytes));
#endif
        }
    }
}