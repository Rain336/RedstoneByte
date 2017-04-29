using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public class PacketJoinGame : EntityPacket
    {
        public override int EntityId { get; set; }
        public byte GameMode { get; set; }
        public int Dimension { get; set; }
        public byte Difficulty { get; set; }
        public byte MaxPlayers { get; set; }
        public string LevelType { get; set; }
        public bool DebugInfo { get; set; }

        public override void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            EntityId = buffer.ReadInt();
            GameMode = buffer.ReadByte();
            Dimension = version < ProtocolVersion.V191 ? buffer.ReadByte() : buffer.ReadInt();
            Difficulty = buffer.ReadByte();
            MaxPlayers = buffer.ReadByte();
            LevelType = buffer.ReadString();
            DebugInfo = buffer.ReadBoolean();
        }

        public override void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteInt(EntityId);
            buffer.WriteByte(GameMode);
            if (version < ProtocolVersion.V191) buffer.WriteByte(Dimension);
            else buffer.WriteInt(Dimension);
            buffer.WriteByte(Difficulty);
            buffer.WriteByte(MaxPlayers);
            buffer.WriteString(LevelType);
            buffer.WriteBoolean(DebugInfo);
        }

    }
}