using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketAttachEntity : EntityPacket
    {
        public override int EntityId { get; set; }
        public int HoldingEntityId { get; set; }
        public bool Leash { get; set; }

        public override void CompareSet(int test, int value)
        {
            base.CompareSet(test, value);
            if (HoldingEntityId == test)
                HoldingEntityId = value;
        }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadInt();
            HoldingEntityId = buffer.ReadInt();
            if (version == ProtocolVersion.V189)
                Leash = buffer.ReadBoolean();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteVarInt(HoldingEntityId);
            if (version == ProtocolVersion.V189)
                buffer.WriteBoolean(Leash);
        }
    }
}