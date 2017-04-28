using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketResponse : IPacket
    {
        public StatusResponse Response { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            Response = JsonConvert.DeserializeObject<StatusResponse>(buffer.ReadString());
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteString(JsonConvert.SerializeObject(Response));
        }
    }
}