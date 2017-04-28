using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketChatMessageClient : IPacket
    {
        public TextBase Message { get; set; }
        public byte Position { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Message = JsonConvert.DeserializeObject<TextBase>(buffer.ReadString());
            Position = buffer.ReadByte();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteString(JsonConvert.SerializeObject(Message));
            buffer.WriteByte(Position);
        }
    }
}