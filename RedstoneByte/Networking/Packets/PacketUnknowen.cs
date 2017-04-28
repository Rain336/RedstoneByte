using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public class PacketUnknowen : IPacket
    {
        public int PacketId { get; set; }
        public IByteBuffer Buffer { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Buffer = buffer.Slice();
            Buffer.Retain();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteBytes(Buffer);
        }
    }
}