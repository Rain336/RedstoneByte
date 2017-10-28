using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityStatus : EntityPacket
    {
        public override int EntityId { get; set; }
        public byte Status { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadInt();
            Status = buffer.ReadByte();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteInt(EntityId);
            buffer.WriteByte(Status);
        }
    }
}