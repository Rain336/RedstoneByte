using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public interface IPacket
    {
        void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version);
        void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version);
    }

    public abstract class EntityPacket : IPacket
    {
        public abstract int EntityId { get; set; }
        public abstract void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version);
        public abstract void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version);

        public virtual void CompareSet(int test, int value)
        {
            if (EntityId == test)
                EntityId = value;
        }
    }
}