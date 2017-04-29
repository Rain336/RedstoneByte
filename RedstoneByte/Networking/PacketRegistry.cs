using System;
using System.Collections.Generic;
using RedstoneByte.Networking.Packets;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public sealed class PacketRegistry
    {
        public static readonly PacketRegistry Client = new PacketRegistry(it =>
        {
            it.Handshake = null;

            it.Status = new SimpleEntry(status =>
            {
                status.Add(0x00, typeof(PacketResponse));
                status.Add(0x01, typeof(PacketPing));
            });

            it.Login = new SimpleEntry(login =>
            {
                login.Add(0x00, typeof(PacketDisconnect));
                login.Add(0x01, typeof(PacketEncryptionRequest));
                login.Add(0x02, typeof(PacketLoginSuccess));
                login.Add(0x03, typeof(PacketSetCompression));
            });

            it.Play = new VersionedEntry(play =>
            {
                var plus19 = ProtocolVersion.Range(ProtocolVersion.V19);
                play.RegisterPacket(0x00, typeof(PacketKeepAlive), ProtocolVersion.V189);
                play.RegisterPacket(0x01, typeof(PacketJoinGame), ProtocolVersion.V189);
                play.RegisterPacket(0x02, typeof(PacketChatMessageClient), ProtocolVersion.V189);
                play.RegisterPacket(0x04, typeof(PacketEntityEquipment), ProtocolVersion.V189);
                play.RegisterPacket(0x05, typeof(PacketSpawnPlayer), plus19);
                play.RegisterPacket(0x06, typeof(PacketAnimationClient), plus19);
                play.RegisterPacket(0x08, typeof(PacketBlockBreakAnimation), plus19);
                play.RegisterPacket(0x0A, typeof(PacketUseBed), ProtocolVersion.V189);
                play.RegisterPacket(0x0B, typeof(PacketAnimationClient), ProtocolVersion.V189);
                play.RegisterPacket(0x0C, typeof(PacketSpawnPlayer), ProtocolVersion.V189);
                play.RegisterPacket(0x0D, typeof(PacketCollectItem), ProtocolVersion.V189);
                play.RegisterPacket(0x0F, typeof(PacketChatMessageClient), plus19);
                play.RegisterPacket(0x12, typeof(PacketEntityVelocity), ProtocolVersion.V189);
                play.RegisterPacket(0x13, typeof(PacketDestroyEntities), ProtocolVersion.V189);
                play.RegisterPacket(0x15, typeof(PacketEntityRelativeMove), ProtocolVersion.V189);
                play.RegisterPacket(0x16, typeof(PacketEntityLook), ProtocolVersion.V189);
                play.RegisterPacket(0x17, typeof(PacketEntityLookAndRelativeMove), ProtocolVersion.V189);
                play.RegisterPacket(0x18, typeof(PacketPluginMessage), plus19);
                play.RegisterPacket(0x18, typeof(PacketEntityTeleport), ProtocolVersion.V189);
                play.RegisterPacket(0x19, typeof(PacketEntityHeadLook), ProtocolVersion.V189);
                play.RegisterPacket(0x1A, typeof(PacketDisconnect), plus19);
                play.RegisterPacket(0x1A, typeof(PacketEntityStatus), ProtocolVersion.V189);
                play.RegisterPacket(0x1B, typeof(PacketEntityStatus), plus19);
                play.RegisterPacket(0x1B, typeof(PacketAttachEntity), ProtocolVersion.V189);
                play.RegisterPacket(0x1C, typeof(PacketEntityMetadata), ProtocolVersion.V189);
                play.RegisterPacket(0x1D, typeof(PacketEntityEffect), ProtocolVersion.V189);
                play.RegisterPacket(0x1E, typeof(PacketRemoveEntityEffect), ProtocolVersion.V189);
                play.RegisterPacket(0x1F, typeof(PacketKeepAlive), plus19);
                play.RegisterPacket(0x20, typeof(PacketEntityProperties), ProtocolVersion.V189);
                play.RegisterPacket(0x23, typeof(PacketJoinGame), plus19);
                play.RegisterPacket(0x25, typeof(PacketBlockBreakAnimation), ProtocolVersion.V189);
                play.RegisterPacket(0x25, typeof(PacketEntityRelativeMove), plus19);
                play.RegisterPacket(0x26, typeof(PacketEntityLookAndRelativeMove), plus19);
                play.RegisterPacket(0x27, typeof(PacketEntityLook), plus19);
                play.RegisterPacket(0x28, typeof(PacketEntity), ProtocolVersion.Range());
                play.RegisterPacket(0x2D, typeof(PacketPlayerList), plus19);
                play.RegisterPacket(0x2F, typeof(PacketUseBed), plus19);
                play.RegisterPacket(0x30, typeof(PacketDestroyEntities), plus19);
                play.RegisterPacket(0x31, typeof(PacketRemoveEntityEffect), plus19);
                play.RegisterPacket(0x34, typeof(PacketEntityHeadLook), plus19);
                play.RegisterPacket(0x36, typeof(PacketCamera), plus19);
                play.RegisterPacket(0x38, typeof(PacketPlayerList), ProtocolVersion.V189);
                play.RegisterPacket(0x39, typeof(PacketEntityMetadata), plus19);
                play.RegisterPacket(0x3A, typeof(PacketAttachEntity), plus19);
                play.RegisterPacket(0x3B, typeof(PacketEntityVelocity), plus19);
                play.RegisterPacket(0x3C, typeof(PacketEntityEquipment), plus19);
                play.RegisterPacket(0x3F, typeof(PacketPluginMessage), ProtocolVersion.V189);
                play.RegisterPacket(0x40, typeof(PacketDisconnect), ProtocolVersion.V189);
                play.RegisterPacket(0x40, typeof(PacketSetPassengers), plus19);
                play.RegisterPacket(0x42, typeof(PacketCombatEvent), ProtocolVersion.V189);
                play.RegisterPacket(0x43, typeof(PacketCamera), ProtocolVersion.V189);
                play.RegisterPacket(0x48, typeof(PacketCollectItem), ProtocolVersion.Range(ProtocolVersion.V194));
                play.RegisterPacket(0x49, typeof(PacketEntityTeleport), ProtocolVersion.Range(ProtocolVersion.V194));
                play.RegisterPacket(0x49, typeof(PacketCollectItem),
                    ProtocolVersion.V19, ProtocolVersion.V191, ProtocolVersion.V192);
                play.RegisterPacket(0x4A, typeof(PacketEntityProperties), ProtocolVersion.Range(ProtocolVersion.V194));
                play.RegisterPacket(0x4A, typeof(PacketEntityTeleport),
                    ProtocolVersion.V19, ProtocolVersion.V191, ProtocolVersion.V192);
                play.RegisterPacket(0x4A, typeof(PacketEntityProperties),
                    ProtocolVersion.V19, ProtocolVersion.V191, ProtocolVersion.V192);
                play.RegisterPacket(0x4B, typeof(PacketEntityEffect), ProtocolVersion.Range(ProtocolVersion.V194));
                play.RegisterPacket(0x4C, typeof(PacketEntityEffect),
                    ProtocolVersion.V19, ProtocolVersion.V191, ProtocolVersion.V192);
            });
        });

        public static readonly PacketRegistry Server = new PacketRegistry(it =>
        {
            it.Handshake = new SingleEntry(0x00, typeof(PacketHandshake));

            it.Status = new SimpleEntry(status =>
            {
                status.Add(0x00, typeof(PacketRequest));
                status.Add(0x01, typeof(PacketPing));
            });

            it.Login = new SimpleEntry(login =>
            {
                login.Add(0x00, typeof(PacketLoginStart));
                login.Add(0x01, typeof(PacketEncryptionResponse));
            });

            it.Play = new VersionedEntry(play =>
            {
                var plus19 = ProtocolVersion.Range(ProtocolVersion.V19);
                play.RegisterPacket(0x00, typeof(PacketKeepAlive), ProtocolVersion.V189);
                play.RegisterPacket(0x04, typeof(PacketClientSettings), plus19);
                play.RegisterPacket(0x09, typeof(PacketPluginMessage), plus19);
                play.RegisterPacket(0x0B, typeof(PacketKeepAlive), plus19);
                play.RegisterPacket(0x0B, typeof(PacketEntityAction), ProtocolVersion.V189);
                play.RegisterPacket(0x14, typeof(PacketEntityAction), plus19);
                play.RegisterPacket(0x15, typeof(PacketClientSettings), ProtocolVersion.V189);
                play.RegisterPacket(0x17, typeof(PacketPluginMessage), ProtocolVersion.V189);
            });
        });

        public IEntry Handshake { get; private set; }

        public IEntry Status { get; private set; }

        public IEntry Login { get; private set; }

        public IEntry Play { get; private set; }

        private PacketRegistry(Action<PacketRegistry> init)
        {
            init(this);
        }

        public int GetId(ConnectionState state, ProtocolVersion version, Type packet)
        {
            switch (state)
            {
                case ConnectionState.Handshaking:
                    return Handshake?.GetId(version, packet) ?? 0x00;

                case ConnectionState.Play:
                    return Play.GetId(version, packet);

                case ConnectionState.Status:
                    return Status.GetId(version, packet);

                case ConnectionState.Login:
                    return Login.GetId(version, packet);

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public static int GetId(bool client, ConnectionState state, ProtocolVersion version, Type packet)
        {
            return client ? Client.GetId(state, version, packet) : Server.GetId(state, version, packet);
        }

        public IPacket CreatePacket(ConnectionState state, ProtocolVersion version, int id)
        {
            switch (state)
            {
                case ConnectionState.Handshaking:
                    return Handshake?.CreatePacket(version, id);

                case ConnectionState.Play:
                    return Play.CreatePacket(version, id);

                case ConnectionState.Status:
                    return Status.CreatePacket(version, id);

                case ConnectionState.Login:
                    return Login.CreatePacket(version, id);

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public static IPacket CreatePacket(bool client, ConnectionState state, ProtocolVersion version, int id)
        {
            return client ? Client.CreatePacket(state, version, id) : Server.CreatePacket(state, version, id);
        }

        public interface IEntry
        {
            int GetId(ProtocolVersion version, Type packet);
            IPacket CreatePacket(ProtocolVersion version, int id);
        }

        public sealed class SingleEntry : IEntry
        {
            public readonly int Id;
            public readonly Type Type;

            internal SingleEntry(int id, Type type)
            {
                Id = id;
                Type = type;
            }

            public int GetId(ProtocolVersion version, Type packet)
            {
                return Id;
            }

            public IPacket CreatePacket(ProtocolVersion version, int id)
            {
                return (IPacket) Activator.CreateInstance(Type);
            }
        }

        public sealed class SimpleEntry : IEntry
        {
            private readonly BiDictionary<int, Type> _packets = new BiDictionary<int, Type>();

            internal SimpleEntry(Action<BiDictionary<int, Type>> init)
            {
                init(_packets);
            }

            public int GetId(ProtocolVersion version, Type packet)
            {
                if (_packets.TryGetValue(packet, out var id))
                {
                    return id;
                }
                return -1;
            }

            public IPacket CreatePacket(ProtocolVersion version, int id)
            {
                if (_packets.TryGetValue(id, out var type))
                {
                    return (IPacket) Activator.CreateInstance(type);
                }
                return null;
            }
        }

        public sealed class VersionedEntry : IEntry
        {
            private readonly Dictionary<ProtocolVersion, BiDictionary<int, Type>> _packets =
                new Dictionary<ProtocolVersion, BiDictionary<int, Type>>();

            internal VersionedEntry(Action<VersionedEntry> init)
            {
                init(this);
            }

            internal void RegisterPacket(int id, Type type, params ProtocolVersion[] versions)
            {
                if (versions.Length == 0)
                    throw new ArgumentException("Atleast one version must be supplied.", nameof(versions));
                foreach (var version in versions)
                {
                    if (_packets.TryGetValue(version, out var dict))
                    {
                        dict.Add(id, type);
                    }
                    else
                    {
                        _packets.Add(version, new BiDictionary<int, Type>
                        {
                            {id, type}
                        });
                    }
                }
            }

            public int GetId(ProtocolVersion version, Type packet)
            {
                if (!_packets.TryGetValue(version, out var dict)) return -1;
                if (dict.TryGetValue(packet, out var id))
                {
                    return id;
                }
                return -1;
            }

            public IPacket CreatePacket(ProtocolVersion version, int id)
            {
                if (!_packets.TryGetValue(version, out var dict)) return null;
                if (dict.TryGetValue(id, out var type))
                {
                    return (IPacket) Activator.CreateInstance(type);
                }
                return null;
            }
        }
    }
}