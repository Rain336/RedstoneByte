using System;
using System.Text;
using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.NBT;
using RedstoneByte.Networking;
using RedstoneByte.Text;

namespace RedstoneByte.Utils
{
    public static class EnumExtender
    {
        public static object ReadFromBuffer(this EntityMetadata.EntryType type, IByteBuffer buffer)
        {
            switch (type)
            {
                case EntityMetadata.EntryType.Byte:
                    return buffer.ReadByte();

                case EntityMetadata.EntryType.VarInt:
                    return buffer.ReadVarInt();

                case EntityMetadata.EntryType.Float:
                    return buffer.ReadFloat();

                case EntityMetadata.EntryType.String:
                    return buffer.ReadString();

                case EntityMetadata.EntryType.Chat:
                    return JsonConvert.DeserializeObject<TextBase>(buffer.ReadString());

                case EntityMetadata.EntryType.Slot:
                    return buffer.ReadSlot();

                case EntityMetadata.EntryType.Boolean:
                    return buffer.ReadBoolean();

                case EntityMetadata.EntryType.Rotation:
                    return new Rotation(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());

                case EntityMetadata.EntryType.Position:
                    return new Position(buffer.ReadLong());

                case EntityMetadata.EntryType.OptPosition:
                    return buffer.ReadBoolean() ? new Position(buffer.ReadLong()) : Position.Zero;

                case EntityMetadata.EntryType.Direction:
                    return (Direction) buffer.ReadVarInt();

                case EntityMetadata.EntryType.OptGuid:
                    return buffer.ReadBoolean() ? buffer.ReadGuid() : Guid.Empty;

                case EntityMetadata.EntryType.OptBlockId:
                    var value = buffer.ReadVarInt();
                    return value == 0 ? BlockId.Zero : new BlockId(value);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static NbtTag ReadFromBuffer(this NbtType type, IByteBuffer buffer)
        {
            switch (type)
            {
                case NbtType.End:
                    throw new InvalidOperationException("NbtEnd Cannot be read!");

                case NbtType.Byte:
                    return new NbtByte(buffer.ReadByte());

                case NbtType.Short:
                    return new NbtShort(buffer.ReadShort());

                case NbtType.Int:
                    return new NbtInt(buffer.ReadInt());

                case NbtType.Long:
                    return new NbtLong(buffer.ReadLong());

                case NbtType.Float:
                    return new NbtFloat(buffer.ReadFloat());

                case NbtType.Double:
                    return new NbtDouble(buffer.ReadDouble());

                case NbtType.ByteArray:
                    return new NbtByteArray(buffer.ReadBytes(buffer.ReadInt()).ToArray());

                case NbtType.String:
                    return new NbtString(
                        Encoding.UTF8.GetString(buffer.ReadBytes(buffer.ReadUnsignedShort()).ToArray()));

                case NbtType.List:
                    var list = new NbtList();
                    list.ReadFromBuffer(buffer);
                    return list;

                case NbtType.Compound:
                    var nbt = new NbtCompound();
                    nbt.ReadFromBuffer(buffer);
                    return nbt;

                case NbtType.IntArray:
                    var array = new int[buffer.ReadInt()];
                    for (var i = 0; i < array.Length; i++)
                    {
                        array[i] = buffer.ReadInt();
                    }
                    return new NbtIntArray(array);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}