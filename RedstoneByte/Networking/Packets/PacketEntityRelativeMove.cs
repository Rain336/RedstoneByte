using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityRelativeMove : EntityPacket
    {
        public override int EntityId { get; set; }
        public short DeltaX { get; set; }
        public short DeltaY { get; set; }
        public short DeltaZ { get; set; }
        public bool OnGround { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            DeltaX = version >= ProtocolVersion.V19 ? buffer.ReadShort() : buffer.ReadByte();
            DeltaY = version >= ProtocolVersion.V19 ? buffer.ReadShort() : buffer.ReadByte();
            DeltaZ = version >= ProtocolVersion.V19 ? buffer.ReadShort() : buffer.ReadByte();
            OnGround = buffer.ReadBoolean();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            if (version >= ProtocolVersion.V19) buffer.WriteShort(DeltaX);
            else buffer.WriteByte((byte) DeltaX);
            if (version >= ProtocolVersion.V19) buffer.WriteShort(DeltaY);
            else buffer.WriteByte((byte) DeltaY);
            if (version >= ProtocolVersion.V19) buffer.WriteShort(DeltaZ);
            else buffer.WriteByte((byte) DeltaZ);
            buffer.WriteBoolean(OnGround);
        }
    }
}