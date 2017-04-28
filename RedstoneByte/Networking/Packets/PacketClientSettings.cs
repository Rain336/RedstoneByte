using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketClientSettings : IPacket
    {
        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public int ChatMode { get; set; }
        public bool ChatColors { get; set; }
        public byte SkinParts { get; set; }
        public int MainHand { get; set; } = 1;

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Locale = buffer.ReadString();
            ViewDistance = buffer.ReadByte();
            ChatMode = buffer.ReadVarInt();
            ChatColors = buffer.ReadBoolean();
            SkinParts = buffer.ReadByte();
            if (version > ProtocolVersion.V189) MainHand = buffer.ReadVarInt();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteString(Locale);
            buffer.WriteByte(ViewDistance);
            buffer.WriteVarInt(ChatMode);
            buffer.WriteBoolean(ChatColors);
            buffer.WriteByte(SkinParts);
            if (version > ProtocolVersion.V189) buffer.WriteVarInt(MainHand);
        }
    }
}