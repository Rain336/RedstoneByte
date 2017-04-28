using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public interface IPacket
    {
        void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version);
        void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version);
    }

    public interface IEntityPacket : IPacket
    {
        int EntityId { get; set; }
    }
}