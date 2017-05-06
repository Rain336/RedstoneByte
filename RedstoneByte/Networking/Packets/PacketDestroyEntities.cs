using System;
using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketDestroyEntities : EntityPacket
    {
        public override int EntityId
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public int[] EntityIds { get; set; }

        public override void CompareSet(int test, int value)
        {
            for (var i = 0; i < EntityIds.Length; i++)
            {
                if (EntityIds[i] == test)
                    EntityIds[i] = value;
            }
        }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityIds = new int[buffer.ReadVarInt()];
            for (var i = 0; i < EntityIds.Length; i++)
            {
                EntityIds[i] = buffer.ReadVarInt();
            }
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityIds.Length);
            foreach (var id in EntityIds)
            {
                buffer.WriteVarInt(id);
            }
        }
    }
}