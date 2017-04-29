using System;
using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketSpawnPlayer : EntityPacket
    {
        public override int EntityId { get; set; }
        public Guid PlayerGuid { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
        public short CurrentItem { get; set; }
        public readonly EntityMetadata Metadata = new EntityMetadata();

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            PlayerGuid = buffer.ReadGuid();
            X = version >= ProtocolVersion.V19 ? buffer.ReadDouble() : buffer.ReadInt();
            Y = version >= ProtocolVersion.V19 ? buffer.ReadDouble() : buffer.ReadInt();
            Z = version >= ProtocolVersion.V19 ? buffer.ReadDouble() : buffer.ReadInt();
            Yaw = buffer.ReadByte();
            Pitch = buffer.ReadByte();
            if (version == ProtocolVersion.V189)
                CurrentItem = buffer.ReadShort();
            Metadata.ReadFromBuffer(buffer);
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteGuid(PlayerGuid);
            if (version >= ProtocolVersion.V19) buffer.WriteDouble(X);
            else buffer.WriteInt((int) Math.Floor(X));
            if (version >= ProtocolVersion.V19) buffer.WriteDouble(Y);
            else buffer.WriteInt((int) Math.Floor(Y));
            if (version >= ProtocolVersion.V19) buffer.WriteDouble(Z);
            else buffer.WriteInt((int) Math.Floor(Z));
            buffer.WriteByte(Yaw);
            buffer.WriteByte(Pitch);
            if (version == ProtocolVersion.V189)
                buffer.WriteShort(CurrentItem);
            Metadata.WriteToBuffer(buffer);
        }
    }
}