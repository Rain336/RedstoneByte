using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityEquipment : EntityPacket
    {
        public override int EntityId { get; set; }
        public EquipmentSlot Slot { get; set; }
        public byte[] Item { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Slot = (EquipmentSlot)(version >= ProtocolVersion.V19 ? buffer.ReadVarInt() : buffer.ReadShort());
            Item = buffer.ToArray();
            buffer.SkipBytes(buffer.ReadableBytes);
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            if (version >= ProtocolVersion.V19) buffer.WriteVarInt((int)Slot);
            else buffer.WriteShort((short)Slot);
            buffer.WriteBytes(Item);
        }

        public enum EquipmentSlot
        {
            MainHand,
            Offhand,
            Boots,
            Leggings,
            Chestplate,
            Helmet
        }
    }
}