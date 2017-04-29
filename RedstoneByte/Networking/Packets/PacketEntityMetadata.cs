using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityMetadata : EntityPacket
    {
        public override int EntityId { get; set; }
        public readonly EntityMetadata Metadata = new EntityMetadata();

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Metadata.ReadFromBuffer(buffer);
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            Metadata.WriteToBuffer(buffer);
        }
    }
}