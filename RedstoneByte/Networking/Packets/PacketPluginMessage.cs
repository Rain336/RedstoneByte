using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketPluginMessage : IPacket
    {
        public string Tag { get; set; }
        public IByteBuffer Buffer { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Tag = buffer.ReadString();
            Buffer = buffer.Slice();
            Buffer.Retain();
            buffer.SkipBytes(buffer.ReadableBytes);
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteString(Tag);
            buffer.WriteBytes(Buffer);
        }
    }
}