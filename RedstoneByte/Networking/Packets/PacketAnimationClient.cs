using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketAnimationClient : IEntityPacket
    {
        public int EntityId { get; set; }
        public Animation Animation { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Animation = (Animation) buffer.ReadByte();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteByte((byte) Animation);
        }
    }
}