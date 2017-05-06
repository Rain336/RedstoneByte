using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketCollectItem : EntityPacket
    {
        public override int EntityId { get; set; }
        public int CollectorEntityId { get; set; }
        public int Count { get; set; }

        public override void CompareSet(int test, int value)
        {
            base.CompareSet(test, value);
            if (CollectorEntityId == test)
                CollectorEntityId = value;
        }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            CollectorEntityId = buffer.ReadVarInt();
            if(version >= ProtocolVersion.V111) Count = buffer.ReadVarInt();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteVarInt(CollectorEntityId);
            if(version >= ProtocolVersion.V111) buffer.WriteVarInt(Count);
        }
    }
}