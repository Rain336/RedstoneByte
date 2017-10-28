using System;
using System.Text;
using DotNetty.Buffers;
using RedstoneByte.NBT;

namespace RedstoneByte.Utils
{
    public static class EnumExtender
    {
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