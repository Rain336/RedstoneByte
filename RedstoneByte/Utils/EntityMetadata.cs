using System;
using System.Collections;
using System.Collections.Generic;
using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.Networking;

namespace RedstoneByte.Utils
{
    public sealed class EntityMetadata
    {
        public readonly Dictionary<byte, Entry> Entries = new Dictionary<byte, Entry>();

        public void ReadFromBuffer(IByteBuffer buffer)
        {
            var index = buffer.ReadByte();
            while (index != 0xFF)
            {
                var type = (EntryType) buffer.ReadByte();
                Entries.Add(index, new Entry(type, type.ReadFromBuffer(buffer)));
                index = buffer.ReadByte();
            }
        }

        public void WriteToBuffer(IByteBuffer buffer)
        {
            for (byte i = 0; i < Entries.Count; i++)
            {
                if (Entries[i] == null) continue;
                buffer.WriteByte(i);
                buffer.WriteByte((byte) Entries[i].Type);
                Entries[i].WriteToBuffer(buffer);
            }
            buffer.WriteByte(0xFF);
        }

        public Entry this[byte key]
        {
            get => Entries[key];
            set => Entries[key] = value;
        }

        public sealed class Entry : IEquatable<Entry>
        {
            public readonly EntryType Type;
            public readonly object Value;

            public Entry(EntryType type, object value)
            {
                Type = type;
                Value = value ?? throw new ArgumentNullException(nameof(value));
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

            public bool Equals(Entry other)
            {
                if (ReferenceEquals(other, null)) return false;
                return Type == other.Type && Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Entry);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((int) Type * 397) ^ Value.GetHashCode();
                }
            }

            public static bool operator ==(Entry l, Entry r)
            {
                return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r);
            }

            public static bool operator !=(Entry l, Entry r)
            {
                return ReferenceEquals(l, null) ? !ReferenceEquals(r, null) : !l.Equals(r);
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
    }
}