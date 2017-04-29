using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityVelocity : EntityPacket
    {
        public override int EntityId { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            VelocityX = buffer.ReadShort();
            VelocityY = buffer.ReadShort();
            VelocityZ = buffer.ReadShort();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteShort(VelocityX);
            buffer.WriteShort(VelocityY);
            buffer.WriteShort(VelocityZ);
        }
    }
}