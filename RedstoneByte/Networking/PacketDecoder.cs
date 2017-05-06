using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using RedstoneByte.Networking.Packets;

namespace RedstoneByte.Networking
{
    public sealed class PacketDecoder : ByteToMessageDecoder
    {
        public PacketHandler Handler { get; set; }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            if (!input.IsReadable()) return;
            var id = input.ReadVarInt();
            var packet = PacketRegistry.CreatePacket(Handler.Client, Handler.State, Handler.Version, id)
                         ?? new PacketUnknowen
                         {
                             PacketId = id
                         };
#if DEBUG
            RedstoneByte.Logger.Debug(Handler.Handler.GetType().Name + " <=IN= " +
                                      packet.GetType().Name + '(' + id +')');
            //RedstoneByte.Logger.Debug(
            //    System.BitConverter.ToString(input.Array, input.ReaderIndex, input.ReadableBytes));
#endif
            packet.ReadFromBuffer(input, Handler.Version);
            output.Add(packet);
        }
    }
}