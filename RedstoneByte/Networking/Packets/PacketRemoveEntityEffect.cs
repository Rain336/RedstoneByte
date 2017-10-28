using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketRemoveEntityEffect : EntityPacket
    {
        public override int EntityId { get; set; }
        public byte EffectId { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            EffectId = buffer.ReadByte();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteByte(EffectId);
        }
    }
}