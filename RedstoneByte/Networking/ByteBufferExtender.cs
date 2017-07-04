using System;
using System.Text;
using DotNetty.Buffers;
using RedstoneByte.Networking.Packets;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public static class ByteBufferExtender
    {
//        public static int ReadVarInt(this IByteBuffer buffer)
//        {
//            var value = 0;
//            var size = 0;
//            while (true)
//            {
//                var b = buffer.ReadByte();
//                value |= (b & 0x7F) << size++ * 7;
//                if (size > 5)
//                    throw new FormatException("VarInt may not be longer than 5 bytes.");
//                if ((b & 0x80) != 128)
//                    break;
//            }
//            return value;
//        }

        public static int ReadVarInt(this IByteBuffer buffer)
        {
            int value;
            do
            {
                var b = buffer.ReadByte(); value  = (b & 0x7F)      ; if ((b & 0x80) == 0) break;
                    b = buffer.ReadByte(); value |= (b & 0x7F) <<  7; if ((b & 0x80) == 0) break;
                    b = buffer.ReadByte(); value |= (b & 0x7F) << 14; if ((b & 0x80) == 0) break;
                    b = buffer.ReadByte(); value |= (b & 0x7F) << 21; if ((b & 0x80) == 0) break;
                    b = buffer.ReadByte(); value |= (b & 0x7F) << 28; if ((b & 0x80) == 0) break;
                throw new FormatException("VarInt may not be longer than 5 bytes.");
            } while (false);
            return value;
        }

        public static float ReadFloat(this IByteBuffer buffer)
        {
            return BitConverter.ToSingle(buffer.ReadBytes(4).ToArray(), 0);
        }

        public static Position ReadPosition(this IByteBuffer buffer)
        {
            return new Position(buffer.ReadLong());
        }

        public static Slot ReadSlot(this IByteBuffer buffer)
        {
            var result = new Slot(buffer.ReadShort());
            if (result.Id == -1) return result;
            result.Count = buffer.ReadByte();
            result.Metadata = buffer.ReadShort();
            result.NbtCompound.ReadFromBuffer(buffer);
            return result;
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

        public static void WriteFloat(this IByteBuffer buffer, float value)
        {
            buffer.WriteBytes(BitConverter.GetBytes(value));
        }

        public static void WritePosition(this IByteBuffer buffer, Position value)
        {
            buffer.WriteLong(value.ToLong());
        }

        public static void WriteSlot(this IByteBuffer buffer, Slot value)
        {
            buffer.WriteShort(value.Id);
            if(value.Id == -1) return;
            buffer.WriteByte(value.Count);
            buffer.WriteShort(value.Metadata);
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