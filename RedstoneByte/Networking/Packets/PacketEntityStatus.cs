﻿using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityStatus : EntityPacket
    {
        public override int EntityId { get; set; }
        public EntityStatus Status { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Status = (EntityStatus) buffer.ReadByte();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteByte((byte) Status);
        }
    }
}