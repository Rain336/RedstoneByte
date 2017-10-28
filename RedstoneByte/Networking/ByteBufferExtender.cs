using System;
using System.Text;
using DotNetty.Buffers;
using RedstoneByte.Networking.Packets;

namespace RedstoneByte.Networking
{
    public static class ByteBufferExtender
    {
        public static int ReadVarInt(this IByteBuffer buffer)
        {
            var value = 0;
            var size = 0;
            while (true)
            {
                var b = buffer.ReadByte();
                value |= (b & 0x7F) << size++ * 7;
                if (size > 5)
                    throw new FormatException("VarInt may not be longer than 5 bytes.");
                if ((b & 0x80) != 128)
                    break;
            }
            return value;
        }

        public static Guid ReadGuid(this IByteBuffer buffer)
        {
            return new Guid(buffer.ReadBytes(16).ToArray());
        }

        public static string ReadString(this IByteBuffer buffer)
        {
            var length = buffer.ReadVarInt();
            return Encoding.UTF8.GetString(buffer.ReadBytes(length).ToArray());
        }

        public static LagacyPingVersion ReadLagacyPing(this IByteBuffer buffer)
        {
            if (!buffer.IsReadable())
            {
                return LagacyPingVersion.V13;
            }
            buffer.ReadByte();
            return buffer.IsReadable() ? LagacyPingVersion.V16 : LagacyPingVersion.V15;
        }

        public static void WriteVarInt(this IByteBuffer buffer, int value)
        {
            while ((value & 0xFFFFFF80) != 0)
            {
                buffer.WriteByte((value & 0x7F) | 0x80);
                value >>= 7;
            }
            buffer.WriteByte(value);
        }

        public static void WriteString(this IByteBuffer buffer, string value)
        {
            buffer.WriteVarInt(value.Length);
            buffer.WriteBytes(Encoding.UTF8.GetBytes(value));
        }

        public static void WriteGuid(this IByteBuffer buffer, Guid guid)
        {
            buffer.WriteBytes(guid.ToByteArray());
        }

        public static void WriteLagacyPing(this IByteBuffer buffer, LagacyPingVersion version)
        {
            buffer.WriteByte(0xFF);
            var ping = RedstoneByte.CopyStatusResponse(); //TODO: Post Event.
            string data;
            switch (version)
            {
                case LagacyPingVersion.V13:
                    data = string.Format("{0}§{1}§{2}", ping.Motd.ToPlain(), ping.OnlinePlayers,
                        ping.MaxPlayers);
                    break;

                case LagacyPingVersion.V15:
                case LagacyPingVersion.V16:
                    data =
                        $"§1\0127\0{ping.Version.Name}\0{ping.Motd.ToPlain()}\0{ping.OnlinePlayers}\0{ping.MaxPlayers}";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(version), version, null);
            }
            buffer.GetShort(data.Length);
            buffer.WriteBytes(Encoding.BigEndianUnicode.GetBytes(data));
        }
    }
}