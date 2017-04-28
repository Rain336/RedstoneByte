using System;
using System.Collections;
using System.Collections.Generic;
using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.Networking;

namespace RedstoneByte.Utils
{
    public sealed class EntityMetadata : IList<EntityMetadata.Entry>
    {
        public readonly List<Entry> Entries = new List<Entry>();

        public void ReadFromBuffer(IByteBuffer buffer)
        {
            var index = buffer.ReadByte();
            while (index != 0xFF)
            {
                var type = (EntryType) buffer.ReadByte();
                Entries.Insert(index, new Entry(type, type.ReadFromBuffer(buffer)));
                index = buffer.ReadByte();
            }
        }

        public void WriteToBuffer(IByteBuffer buffer)
        {
            for (var i = 0; i < Entries.Count; i++)
            {
                if (Entries[i] == null) continue;
                buffer.WriteByte(i);
                buffer.WriteByte((byte) Entries[i].Type);
                Entries[i].WriteToBuffer(buffer);
            }
            buffer.WriteByte(0xFF);
        }

        public sealed class Entry
        {
            public readonly EntryType Type;
            public readonly object Value;

            public Entry(EntryType type, object value)
            {
                Type = type;
                Value = value;
            }

            public T GetValue<T>()
            {
                return (T) Value;
            }

            public void WriteToBuffer(IByteBuffer buffer)
            {
                switch (Type)
                {
                    case EntryType.Byte:
                        buffer.WriteByte((byte) Value);
                        break;

                    case EntryType.VarInt:
                        buffer.WriteVarInt((int) Value);
                        break;

                    case EntryType.Float:
                        buffer.WriteFloat((float) Value);
                        break;

                    case EntryType.String:
                        buffer.WriteString((string) Value);
                        break;

                    case EntryType.Chat:
                        buffer.WriteString(JsonConvert.SerializeObject(Value));
                        break;

                    case EntryType.Slot:
                        buffer.WriteSlot((Slot) Value);
                        break;

                    case EntryType.Boolean:
                        buffer.WriteBoolean((bool) Value);
                        break;

                    case EntryType.Rotation:
                        var rotation = (Rotation) Value;
                        buffer.WriteFloat(rotation.X);
                        buffer.WriteFloat(rotation.Y);
                        buffer.WriteFloat(rotation.Z);
                        break;

                    case EntryType.Position:
                        buffer.WritePosition((Position) Value);
                        break;

                    case EntryType.OptPosition:
                        var position = (Position) Value;
                        if (position == Position.Zero)
                        {
                            buffer.WriteBoolean(false);
                        }
                        else
                        {
                            buffer.WriteBoolean(true);
                            buffer.WriteLong(position.ToLong());
                        }
                        break;

                    case EntryType.Direction:
                        buffer.WriteVarInt((int) Value);
                        break;

                    case EntryType.OptGuid:
                        var guid = (Guid) Value;
                        if (guid == Guid.Empty)
                        {
                            buffer.WriteBoolean(false);
                        }
                        else
                        {
                            buffer.WriteBoolean(true);
                            buffer.WriteGuid(guid);
                        }
                        break;

                    case EntryType.OptBlockId:
                        buffer.WriteVarInt(((BlockId) Value).ToInteger());
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum EntryType
        {
            Byte,
            VarInt,
            Float,
            String,
            Chat,
            Slot,
            Boolean,
            Rotation,
            Position,
            OptPosition,
            Direction,
            OptGuid,
            OptBlockId
        }

        public IEnumerator<Entry> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Entry item)
        {
            Entries.Add(item);
        }

        public void Clear()
        {
            Entries.Clear();
        }

        public bool Contains(Entry item)
        {
            return Entries.Contains(item);
        }

        public void CopyTo(Entry[] array, int arrayIndex)
        {
            Entries.CopyTo(array, arrayIndex);
        }

        public bool Remove(Entry item)
        {
            return Entries.Remove(item);
        }

        public int Count => Entries.Count;
        public bool IsReadOnly => false;

        public int IndexOf(Entry item)
        {
            return Entries.IndexOf(item);
        }

        public void Insert(int index, Entry item)
        {
            Entries.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Entries.RemoveAt(index);
        }

        public Entry this[int index]
        {
            get => Entries[index];
            set => Entries[index] = value;
        }
    }
}