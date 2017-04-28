using System;
using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketLoginSuccess : IPacket
    {
        public Guid Guid { get; set; }
        public string Username { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Guid = Guid.Parse(buffer.ReadString());
            Username = buffer.ReadString();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteString(Guid.ToString("D"));
            buffer.WriteString(Username);
        }
    }
}