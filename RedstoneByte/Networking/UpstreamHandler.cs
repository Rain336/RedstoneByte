using System;
using DotNetty.Handlers.Timeout;
using NLog;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public sealed class UpstreamHandler : IHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public readonly Player Player;
#if DEBUG
        private int _lost;
#endif

        public UpstreamHandler(Player player)
        {
            Player = player;
        }

        public void OnConnect()
        {
        }

        public void OnPacket(IPacket packet)
        {
#if DEBUG
            System.Threading.Interlocked.Increment(ref _lost);
#endif
            if (Player.Server == null) return;
            PatchEntityId(packet as EntityPacket);
            Player.Server.SendPacketAsync(packet)
#if DEBUG
                .ContinueWith(t => System.Threading.Interlocked.Decrement(ref _lost))
#endif
                ;
        }

        public void OnDisconnect()
        {
#if DEBUG
            Logger.Debug("Lost Packets: " + System.Threading.Interlocked.CompareExchange(ref _lost, 0, 0));
#endif
            Player.Server?.DisconnectAsync();
            PlayerList.RemovePlayer(Player);
        }

        public void OnException(Exception exception)
        {
            if (exception is ReadTimeoutException)
            {
                Logger.Info(exception, "'{0}' timed out!", Player.Name);
            }
            else
            {
                Logger.Warn(exception, "'{0}' errored while Connecting!", Player.Name);
            }
        }

        private void PatchEntityId(EntityPacket packet)
        {
            packet?.CompareSet(Player.ClientEntityId, Player.ServerEntityId);
        }
    }
}