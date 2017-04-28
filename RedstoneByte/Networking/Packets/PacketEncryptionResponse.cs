using DotNetty.Buffers;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketEncryptionResponse : IPacket
    {
        public byte[] SharedSecret { get; set; }
        public byte[] VerifyToken { get; set; }

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            SharedSecret = buffer.ReadBytes(buffer.ReadVarInt()).ToArray();
            VerifyToken = buffer.ReadBytes(buffer.ReadVarInt()).ToArray();
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt(SharedSecret.Length);
            buffer.WriteBytes(SharedSecret);
            buffer.WriteVarInt(VerifyToken.Length);
            buffer.WriteBytes(VerifyToken);
        }
    }
}