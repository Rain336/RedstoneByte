﻿using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityProperties : EntityPacket
    {
        public override int EntityId { get; set; }
        public byte[] Data { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Data = buffer.ToArray();
            buffer.SkipBytes(buffer.ReadableBytes);
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteBytes(Data);
        }
    }
}