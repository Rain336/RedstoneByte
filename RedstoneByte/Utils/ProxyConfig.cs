using System.IO;
using Nett;

namespace RedstoneByte
{
    public sealed class ProxyConfig
    {
        public static ProxyConfig Instance { get; private set; }
        public const string Filename = "RedstoneByte.toml";
        [TomlComment(" The Ip Address RedstoneByte listens on.")]
        [TomlComment(" Empty means the Default IpAddress.")]
        public string IpAddress { get; set; }
        [TomlComment(" The Port RedstoneByte listens on.")]
        public ushort Port { get; set; }
        [TomlComment(" The Maximum amount of Players to show in the Server List.")]
        public int MaxPlayers { get; set; }
        [TomlComment(" The Maximum amount of Players this Proxy should have.")]
        [TomlComment(" Anything less than zero means unlimited.")]
        public int PlayerLimit { get; set; }
        [TomlComment(" The Message of the Day of this Proxy.")]
        public string Motd { get; set; }
        [TomlComment(" If the Proxy should autenticate it's players by Mojang.")]
        [TomlComment(" FYI: Disabling this also disables The Protocol Encryption.")]
        public bool OnlineMode { get; set; }
        [TomlComment(" If the Player's Ip and UUID/GUID should be passed to the connecting Server.")]
        public bool IpForward { get; set; }
        [TomlComment(" This Category contins Sttings Specific to Networking.")]
        public NetworkingCategory Networking { get; set; }

        public static void Load()
        {
            if (Instance != null) return;
            if (File.Exists(Filename))
            {
                Instance = Toml.ReadFile<ProxyConfig>(Filename);
            }
            else
            {
                Instance = CreateDefaultConfig();
                Toml.WriteFile(Instance, Filename);
            }
        }

        public static ProxyConfig CreateDefaultConfig() => new ProxyConfig
        {
            IpAddress = "",
            Port = 25565,
            MaxPlayers = 20,
            PlayerLimit = -1,
            Motd = "A RedstoneByte Proxy",
            OnlineMode = true,
            IpForward = true,
            Networking = NetworkingCategory.CreateDefaultConfig()
        };

        public sealed class NetworkingCategory
        {
            [TomlComment(" After how many milisconds without a response, a client shoud be kicked.")]
            public int ReadTimeout { get; set; }
            [TomlComment(" The Protocol of the Clients get Patched to this Version.")]
            public int ProtocolVersion { get; set; }
            [TomlComment(" The minimum size of a Packet to be compressed.")]
            [TomlComment("   - anything less then zero means disabled.")]
            [TomlComment("   - 0 means 'Compress Anything' or any packet.")]
            [TomlComment(" Note: Packets less than 64 bytes wiil be extended to 64 bytes.")]
            [TomlComment(" Note: And The Maximum Transmition Unit is 1500 bytes.")]
            public int CompressionThreshold { get; set; }

            public static NetworkingCategory CreateDefaultConfig() => new NetworkingCategory
            {
                ReadTimeout = 30,
                ProtocolVersion = 316,
                CompressionThreshold = 256
            };
        }
    }
}