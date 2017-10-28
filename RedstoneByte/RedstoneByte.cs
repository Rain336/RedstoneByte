using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using RedstoneByte.Networking;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte
{
    public sealed class RedstoneByte
    {
        public static readonly CancellationTokenSource IsRunning = new CancellationTokenSource();
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static readonly ProtocolVersion MinVersion = ProtocolVersion.V189;
        public static readonly ProtocolVersion MaxVersion = ProtocolVersion.V1112;
        public static readonly ProtocolVersion ProtocolVersion;
        public static readonly Thread StatusUpdater;
        public static readonly ProxyConfig Config;
        private static readonly object _lock = new object();
        private static readonly StatusResponse Response;

        static RedstoneByte()
        {
            Thread.CurrentThread.Name = "RedstoneByte Thread";
            Config = ProxyConfig.Load();
            Logger.Info("Proxy confing Loaded!");

            ProtocolVersion = ProtocolVersion.Get(Config.Networking.ProtocolVersion);
            if (ProtocolVersion == null)
                throw new InvalidOperationException("Invalid Protocol Version in Config.");
            Logger.Info("Target Protocol Version is '{0}'", ProtocolVersion.Name);

            Response = new StatusResponse
            {
                Version = ProtocolVersion,
                MaxPlayers = Config.MaxPlayers,
                Motd = Texts.Of(Config.Motd),
                Favicon = LoadFavicon()
            };
            StatusUpdater = new Thread(StatusUpdate)
            {
                Name = "Status Response Updater"
            };
            StatusUpdater.Start();

            ServerQueue.LoadFromFile();
            Logger.Info("ServerQueue loaded.");
        }

        public static StatusResponse CopyStatusResponse()
        {
            lock (_lock)
            {
                return Response.Copy();
            }
        }

        public static Task Start()
        {
            if (!IPAddress.TryParse(Config.IpAddress, out var address))
            {
                address = IPAddress.Any;
            }

            var ep = new IPEndPoint(address, Config.Port);
            Logger.Info("Running on '{0}'", ep);

            return NetworkManager.Run(ep);
        }

        public static Task Stop()
        {
            Logger.Info("Stopping Proxy...");
            IsRunning.Cancel();
            ServerQueue.WriteToFile();
            Logger.Info("ServerQueue saved.");
            return NetworkManager.Stop();
        }

        private static string LoadFavicon()
        {
            if (!File.Exists("server-icon.png")) return null;
            var bytes = File.ReadAllBytes("server-icon.png");

            var magic = BitConverter.ToUInt16(bytes, 0);
            if (magic != 0xFFD8)
            {
                Logger.Warn("Invalid ServerIcon.");
                return null;
            }

            var width = BitConverter.ToUInt16(bytes, 2);
            if (width != 16)
            {
                Logger.Warn("ServerIcon has an invalid width.");
                return null;
            }

            var height = BitConverter.ToUInt16(bytes, 4);
            if (height != 16)
            {
                Logger.Warn("ServerIcon has an invalid height.");
                return null;
            }

            var builder = new StringBuilder("data:image/png;base64,");
            builder.Append(Convert.ToBase64String(bytes));
            return builder.ToString();
        }

        private static void StatusUpdate()
        {
            var token = IsRunning.Token;
            while (!token.IsCancellationRequested)
            {
                lock (_lock)
                {
                    Response.OnlinePlayers = PlayerList.Count;
                }
                Thread.Sleep(500);
            }
        }
    }
}