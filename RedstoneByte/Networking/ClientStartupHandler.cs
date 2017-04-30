using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using NLog;
using RedstoneByte.Networking.Packets;
using RedstoneByte.Text;
using RedstoneByte.Utils;

namespace RedstoneByte.Networking
{
    public sealed class ClientStartupHandler : IHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public readonly PacketHandler Handler;
        private readonly byte[] _token = new byte[4];
        private StartupState _state = StartupState.Handshake;
        private bool _forge;
        private string _name;
        private bool _disconnected;

        public ClientStartupHandler(PacketHandler handler)
        {
            Handler = handler;
            EncryptionUtils.Random.NextBytes(_token);
        }

        public void OnConnect()
        {
        }

        public void OnPacket(IPacket packet)
        {
            switch (packet)
            {
                case PacketHandshake handshake:
                    OnHandshake(handshake);
                    return;

                case PacketRequest request:
                    OnRequest(request);
                    return;

                case PacketPing ping:
                    OnPing(ping);
                    return;

                case PacketLoginStart start:
                    OnLoginStart(start);
                    return;

                case PacketEncryptionResponse response:
                    OnEncryptionResponse(response);
                    return;
            }
        }

        private void OnEncryptionResponse(PacketEncryptionResponse response)
        {
            CheckState(StartupState.Encryption);

            var token = EncryptionUtils.CryptoServiceProvider.Decrypt(response.VerifyToken, false);
            if (!_token.SequenceEqual(token))
                throw new InvalidOperationException("The VerifyTokens didn't match!");

            var secret = EncryptionUtils.CryptoServiceProvider.Decrypt(response.SharedSecret, false);
            Handler.SetupEncryption(secret);

            MojangApi.HasJoined(_name,
                    EncryptionUtils.JavaHexDigest(secret.Concat(EncryptionUtils.GetPublicKey()).ToArray()))
                .ContinueWith(t => Next(t.Result));
        }

        private void OnLoginStart(PacketLoginStart start)
        {
            CheckState(StartupState.Login);
            if (start.Name.Contains("."))
            {
                DisconnectAsync(Texts.Of("Invalid Username!")); //TODO: Translation
                return;
            }
            if (start.Name.Length > 16)
            {
                DisconnectAsync(Texts.Of("Invalid Username!")); //TODO: Translation
                return;
            }

            _name = start.Name;

            if (ProxyConfig.Instance.PlayerLimit > 0 && (ProxyConfig.Instance.PlayerLimit - PlayerList.Count) <= 0)
            {
                DisconnectAsync(Texts.Of("The Proxy is full!")); //TODO: Translation
                return;
            }

            if (ProxyConfig.Instance.OnlineMode)
            {
                _state = StartupState.Encryption;
                Handler.SendPacketAsync(new PacketEncryptionRequest
                {
                    PublicKey = EncryptionUtils.GetPublicKey(),
                    VerifyToken = _token
                });
            }
            else
            {
                Next(new GameProfile(start.Name, Guid.Empty));
            }
        }


        private void OnPing(PacketPing ping)
        {
            CheckState(StartupState.Ping);
            Handler.SendPacketAsync(new PacketPing
                {
                    Payload = ping.Payload
                })
                .ContinueWith(t => Handler.CloseConnectionAsync());
        }

        private void OnRequest(PacketRequest request)
        {
            switch (_state)
            {
                case StartupState.Ping:
                    Handler.CloseConnectionAsync();
                    break;

                case StartupState.Status:
                    //TODO: Post Event!
                    Handler.SendPacketAsync(new PacketResponse
                    {
                        Response = RedstoneByte.CopyStatusResponse()
                    });
                    _state = StartupState.Ping;
                    break;

                default:
                    throw new InvalidOperationException("Invalid Packet Sequence!");
            }
        }

        private void OnHandshake(PacketHandshake handshake)
        {
            switch (handshake.Next)
            {
                case 1:
                    _state = StartupState.Status;
                    Handler.State = ConnectionState.Status;
                    break;

                case 2:
                    if (RedstoneByte.MinVersion > handshake.Version)
                    {
                        DisconnectAsync(Texts.Of("Outdated Client! Minimum is ", RedstoneByte.MinVersion.Name));
                        //TODO: Translation
                    }
                    else if (RedstoneByte.MaxVersion < handshake.Version)
                    {
                        DisconnectAsync(Texts.Of("Outdated Proxy! Maximum is ", RedstoneByte.MaxVersion.Name));
                        //TODO: Translation
                    }
                    else
                    {
                        Handler.Version = ProtocolVersion.ApproximateVersion(handshake.Version);
                        Handler.State = ConnectionState.Login;
                        _state = StartupState.Login;
                        _forge = handshake.Address.Contains("\0FML\0");
                    }
                    break;

                default:
                    throw new InvalidOperationException($"Invalid Next-State in Handshake '{handshake.Next}'");
            }
        }

        private void Next(GameProfile profile)
        {
            if (ProxyConfig.Instance.OnlineMode)
            {
                var target = PlayerList.GetPlayer(profile.Guid);
                target?.DisconnectAsync(Texts.Of("You are alredy connecting."));
                //TODO: Translation

                target = PlayerList.GetPlayer(profile.Name);
                target?.DisconnectAsync(Texts.Of("You are alredy connecting."));
                //TODO: Translation
            }
            else
            {
                var target = PlayerList.GetPlayer(profile.Name);
                if (target != null)
                {
                    DisconnectAsync(Texts.Of("You are already connected."));
                    //TODO: Translation
                    return;
                }
            }

            Handler.SendPacketAsync(new PacketLoginSuccess
                {
                    Guid = profile.Guid,
                    Username = profile.Name
                })
                .ContinueWith(t =>
                {
                    var player = new Player(Handler, profile, _forge);

                    Handler.State = ConnectionState.Play;
                    Handler.Handler = new UpstreamHandler(player);
                    PlayerList.AddPlayer(player);
                    var info = ServerQueue.First;
                    if (info == null)
                        DisconnectAsync(Texts.Of("No Backend Server Found!")); //TODO: Translation
                    else
                        player.ConnectAsync(info)
                            .ContinueWith(e =>
                                {
                                    player.DisconnectAsync(Texts.Of("Error connecting to Server"));
                                    //TODO: Translation
                                    Logger.Warn(e.Exception.InnerException.InnerException,
                                        "'{0}' couldn't connect to Server '{1}'",
                                        _name, info.Name);
                                },
                                TaskContinuationOptions.OnlyOnFaulted);
                });
        }

        private void CheckState(StartupState state)
        {
            if (_state != state)
                throw new InvalidOperationException("Invalid Packet Sequence!");
        }

        private Task DisconnectAsync(TextBase reason)
        {
            _disconnected = true;
            return Handler.SendPacketAsync(new PacketDisconnect
                {
                    Reason = reason
                })
                .ContinueWith(t => Handler.CloseConnectionAsync());
        }

        public void OnDisconnect()
        {
            if (!_disconnected && _state != StartupState.Ping)
                Logger.Debug("'{0}' diconnected without a trace. :(", _name);
        }

        public void OnException(Exception exception)
        {
            _disconnected = true;
            switch (exception)
            {
                case ReadTimeoutException timeout:
                    Logger.Info(exception, "'{0}' timed out!", _name);
                    break;

                default:
                    Logger.Warn(exception, "'{0}' errored while Connecting!", _name);
                    break;
            }
        }

        private enum StartupState
        {
            Handshake,

            Login,
            Encryption,

            Status,
            Ping
        }
    }
}