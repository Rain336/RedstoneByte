using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityLook : EntityPacket
    {
        public override int EntityId { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
        public bool OnGround { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Yaw = buffer.ReadByte();
            Pitch = buffer.ReadByte();
            OnGround = buffer.ReadBoolean();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteByte(Yaw);
            buffer.WriteByte(Pitch);
            buffer.WriteBoolean(OnGround);
        }
    }
}