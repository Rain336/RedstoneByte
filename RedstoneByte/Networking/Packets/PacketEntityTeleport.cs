using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityTeleport : EntityPacket
    {
        public override int EntityId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
        public bool OnGround { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            X = buffer.ReadDouble();
            Y = buffer.ReadDouble();
            Z = buffer.ReadDouble();
            Yaw = buffer.ReadByte();
            Pitch = buffer.ReadByte();
            OnGround = buffer.ReadBoolean();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteDouble(X);
            buffer.WriteDouble(Y);
            buffer.WriteDouble(Z);
            buffer.WriteByte(Yaw);
            buffer.WriteByte(Pitch);
            buffer.WriteBoolean(OnGround);
        }
    }
}