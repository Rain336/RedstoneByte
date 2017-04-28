using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketRequest : IPacket
    {
        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
        }
    }
}