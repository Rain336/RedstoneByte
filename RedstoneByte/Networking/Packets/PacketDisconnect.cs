using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketDisconnect : IPacket
    {
        public TextBase Reason { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Reason = JsonConvert.DeserializeObject<TextBase>(buffer.ReadString());
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteString(JsonConvert.SerializeObject(Reason));
        }
    }
}