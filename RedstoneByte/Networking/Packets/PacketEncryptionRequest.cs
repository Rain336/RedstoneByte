using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEncryptionRequest : IPacket
    {
        public byte[] PublicKey { get; set; }
        public byte[] VerifyToken { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.ReadString();
            PublicKey = buffer.ReadBytes(buffer.ReadVarInt()).ToArray();
            VerifyToken = buffer.ReadBytes(buffer.ReadVarInt()).ToArray();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteString("");
            buffer.WriteVarInt(PublicKey.Length);
            buffer.WriteBytes(PublicKey);
            buffer.WriteVarInt(VerifyToken.Length);
            buffer.WriteBytes(VerifyToken);
        }
    }
}