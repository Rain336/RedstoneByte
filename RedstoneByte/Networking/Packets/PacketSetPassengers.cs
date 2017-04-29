using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketSetPassengers : EntityPacket
    {
        public override int EntityId { get; set; }
        public int[] Passengers { get; set; }

        public override void CompareExchange(int test, int value)
        {
            base.CompareExchange(test, value);
            for (var i = 0; i < Passengers.Length; i++)
            {
                if (Passengers[i] == test)
                    Passengers[i] = value;
            }
        }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Passengers = new int[buffer.ReadVarInt()];
            for (var i = 0; i < Passengers.Length; i++)
            {
                Passengers[i] = buffer.ReadVarInt();
            }
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteVarInt(Passengers.Length);
            foreach (var passenger in Passengers)
            {
                buffer.WriteVarInt(passenger);
            }
        }
    }
}