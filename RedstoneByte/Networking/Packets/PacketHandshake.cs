using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketHandshake : IPacket
    {
        public int Version { get; set; }
        public string Address { get; set; }
        public ushort Port { get; set; }
        public int Next { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Version = buffer.ReadVarInt();
            Address = buffer.ReadString();
            Port = buffer.ReadUnsignedShort();
            Next = buffer.ReadVarInt();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(Version);
            buffer.WriteString(Address);
            buffer.WriteUnsignedShort(Port);
            buffer.WriteVarInt(Next);
        }
    }
}