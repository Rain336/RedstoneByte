using System;
using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEntityEffect : EntityPacket
    {
        public override int EntityId { get; set; }
        public Effect Effect { get; set; }
        public byte Amplifier { get; set; }
        public int Duration { get; set; }
        public EffectFlags Flags { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadVarInt();
            Effect = (Effect) buffer.ReadByte();
            Amplifier = buffer.ReadByte();
            Duration = buffer.ReadVarInt();
            Flags = (EffectFlags) buffer.ReadByte();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(EntityId);
            buffer.WriteByte((byte) Effect);
            buffer.WriteByte(Amplifier);
            buffer.WriteVarInt(Duration);
            buffer.WriteByte((byte) Flags);
        }

        [Flags]
        public enum EffectFlags
        {
            None,
            IsAmbient,
            ShowParticles
        }
    }
}