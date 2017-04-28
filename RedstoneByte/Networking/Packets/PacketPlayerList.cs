using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using Newtonsoft.Json;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking.Packets
{
    public sealed class PacketPlayerList : IPacket
    {
        public Action TheAction { get; set; }
        public Item[] Items;

        public void ReadFromBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            TheAction = (Action) buffer.ReadVarInt();
            var count = buffer.ReadVarInt();
            var list = new List<Item>();
            for (var i = 0; i < count; i++)
            {
                list.Add(Item.FromBuffer(buffer, TheAction));
            }
        }

        public void WriteToBuffer(IByteBuffer buffer, ProtocolVersion version)
        {
            buffer.WriteVarInt((int) TheAction);
            buffer.WriteVarInt(Items.Length);
            foreach (var item in Items)
            {
                item.ToBuffer(buffer, TheAction);
            }
        }

        public static PacketPlayerList RemovePlayer(params Guid[] guids)
        {
            if(guids.Length == 0)
                throw new ArgumentException("cannot be null!", nameof(guids));

            var items = new Item[guids.Length];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = Item.RemovePlayer(guids[i]);
            }
            return new PacketPlayerList
            {
                TheAction = Action.RemovePlayer,
                Items = items
            };
        }

        public static PacketPlayerList UpdateDisplayName(params (Guid, TextBase)[] args)
        {
            if(args.Length == 0)
                throw new ArgumentException("cannot be null!", nameof(args));

            var items = new Item[args.Length];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = Item.UpdateDisplayName(args[i].Item1, args[i].Item2);
            }
            return new PacketPlayerList
            {
                TheAction = Action.UpdateDisplayName,
                Items = items
            };
        }

        public static PacketPlayerList UpdateLatency(params (Guid, int)[] args)
        {
            if(args.Length == 0)
                throw new ArgumentException("cannot be null!", nameof(args));

            var items = new Item[args.Length];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = Item.UpdateLatency(args[i].Item1, args[i].Item2);
            }
            return new PacketPlayerList
            {
                TheAction = Action.UpdateLatency,
                Items = items
            };
        }

        public static PacketPlayerList UpdateGamemode(params (Guid, Gamemode)[] args)
        {
            if(args.Length == 0)
                throw new ArgumentException("cannot be null!", nameof(args));

            var items = new Item[args.Length];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = Item.UpdateGamemode(args[i].Item1, args[i].Item2);
            }
            return new PacketPlayerList
            {
                TheAction = Action.UpdateGamemode,
                Items = items
            };
        }

        public static PacketPlayerList AddPlayer(params (GameProfile, Gamemode, int, TextBase)[] args)
        {
            if(args.Length == 0)
                throw new ArgumentException("cannot be null!", nameof(args));

            var items = new Item[args.Length];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = Item.AddPlayer(args[i].Item1, args[i].Item2, args[i].Item3, args[i].Item4);
            }
            return new PacketPlayerList
            {
                TheAction = Action.AddPlayer,
                Items = items
            };
        }

        public sealed class Item
        {
            public Guid Guid { get; set; }
            public TextBase DisplayName { get; set; }
            public int Ping { get; set; }
            public Gamemode GameMode { get; set; }
            public GameProfile Profile { get; set; }

            public static Item RemovePlayer(Guid guid)
            {
                return new Item
                {
                    Guid = guid
                };
            }

            public static Item UpdateDisplayName(Guid guid, TextBase name)
            {
                return new Item
                {
                    Guid = guid,
                    DisplayName = name
                };
            }

            public static Item UpdateLatency(Guid guid, int ping)
            {
                return new Item
                {
                    Guid = guid,
                    Ping = ping
                };
            }

            public static Item UpdateGamemode(Guid guid, Gamemode mode)
            {
                return new Item
                {
                    Guid = guid,
                    GameMode = mode
                };
            }

            public static Item AddPlayer(GameProfile profile, Gamemode mode, int ping, TextBase name)
            {
                return new Item
                {
                    Guid = profile.Guid,
                    Profile = profile,
                    GameMode = mode,
                    Ping = ping,
                    DisplayName = name
                };
            }

            public static Item FromBuffer(IByteBuffer buffer, Action action)
            {
                var result = new Item
                {
                    Guid = buffer.ReadGuid()
                };
                switch (action)
                {
                    case Action.AddPlayer:
                        result.Profile = new GameProfile(buffer.ReadString(), result.Guid);
                        var count = buffer.ReadVarInt();
                        for (var i = 0; i < count; i++)
                        {
                            result.Profile.Properties.Add(new GameProfile.Property(buffer.ReadString(),
                                buffer.ReadString(), buffer.ReadBoolean() ? buffer.ReadString() : null));
                        }
                        result.GameMode = (Gamemode) buffer.ReadVarInt();
                        result.Ping = buffer.ReadVarInt();
                        if (buffer.ReadBoolean())
                            result.DisplayName = JsonConvert.DeserializeObject<TextBase>(buffer.ReadString());
                        break;

                    case Action.UpdateGamemode:
                        result.GameMode = (Gamemode) buffer.ReadVarInt();
                        break;

                    case Action.UpdateLatency:
                        result.Ping = buffer.ReadVarInt();
                        break;

                    case Action.UpdateDisplayName:
                        if (buffer.ReadBoolean())
                            result.DisplayName = JsonConvert.DeserializeObject<TextBase>(buffer.ReadString());
                        break;

                    case Action.RemovePlayer:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action, null);
                }
                return result;
            }

            public void ToBuffer(IByteBuffer buffer, Action action)
            {
                buffer.WriteGuid(Guid);
                switch (action)
                {
                    case Action.AddPlayer:
                        buffer.WriteString(Profile.Name);
                        buffer.WriteVarInt(Profile.Properties.Count);
                        foreach (var property in Profile.Properties)
                        {
                            buffer.WriteString(property.Name);
                            buffer.WriteString(property.Value);
                            buffer.WriteBoolean(property.HasSignature);
                            if(property.HasSignature)
                                buffer.WriteString(property.Signature);
                        }
                        buffer.WriteVarInt((int) GameMode);
                        buffer.WriteVarInt(Ping);
                        buffer.WriteBoolean(DisplayName != null);
                        if (DisplayName != null)
                            buffer.WriteString(JsonConvert.SerializeObject(DisplayName));
                        break;

                    case Action.UpdateGamemode:
                        buffer.WriteVarInt((int) GameMode);
                        break;

                    case Action.UpdateLatency:
                        buffer.WriteVarInt(Ping);
                        break;

                    case Action.UpdateDisplayName:
                        buffer.WriteBoolean(DisplayName != null);
                        if (DisplayName != null)
                            buffer.WriteString(JsonConvert.SerializeObject(DisplayName));
                        break;

                    case Action.RemovePlayer:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action, null);
                }
            }
        }

        public enum Action
        {
            AddPlayer,
            UpdateGamemode,
            UpdateLatency,
            UpdateDisplayName,
            RemovePlayer
        }
    }
}