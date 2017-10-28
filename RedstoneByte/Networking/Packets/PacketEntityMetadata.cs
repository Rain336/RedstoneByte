using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityMetadata : EntityPacket
    {
        public override int EntityId { get; set; }
        public byte[] Metadata { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Metadata = buffer.ToArray();
            buffer.SkipBytes(buffer.ReadableBytes);
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteBytes(Metadata);
        }
    }
}