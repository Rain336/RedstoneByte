using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketBlockBreakAnimation : IEntityPacket
    {
        public int EntityId { get; set; }
        public Position Position { get; set; }
        public byte State { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Position = buffer.ReadPosition();
            State = buffer.ReadByte();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WritePosition(Position);
            buffer.WriteByte(State);
        }
    }
}