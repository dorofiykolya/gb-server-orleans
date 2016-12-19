using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using SocketProxy.Decoders;
using SocketProxy.Handlers;
using SocketProxy.Packets;
using SocketProxy.Requests;

namespace SocketProxy
{
    public class AuthHandler : SimpleChannelInboundHandler<Packet>
    {
        private readonly AuthManager _authManager;
        private readonly IInternalLogger _logger;
        private bool _authorized = false;
        private bool _checkAuth = false;
        private Auth _auth;

        public AuthHandler(AuthManager authManager, IInternalLogger logger)
        {
            _authManager = authManager;
            _logger = logger;
        }

        protected override async void ChannelRead0(IChannelHandlerContext ctx, Packet msg)
        {
            if (_authorized)
            {
                ctx.FireChannelRead(new UserPacket(msg, _auth));
            }
            else if (!_checkAuth)
            {
                switch ((string)msg.Command)
                {
                    case "authByDeveloper":
                        var auth = await _authManager.GetAuthByDevelopers(msg.ContentAs<AuthByDeveloperPacket>().DeveloperId);
                        _auth = auth;
                        var info = new UserAuthInfoRequest { userAuthInfo = auth };
                        ctx.WriteAndFlushAsync(info);
                        break;
                    case "authByAndroid":
                        _checkAuth = true;
                        break;
                    case "authByBrowser":
                        _checkAuth = true;
                        break;
                    case "authByIOS":
                        _checkAuth = true;
                        break;
                    case "userAuth":
                        var userAuth = msg.ContentAs<UserAuthPacket>();
                        var authState = await _authManager.CheckAuth(userAuth.UserKey, userAuth.AuthKey, userAuth.AuthTs, userAuth.IsBrowser);
                        var data = new UserAuthStateRequest
                        {
                            UserAuthState = new UserAuthStateData
                            {
                                State = (int)authState,
                                MinVersion = "0.0.0"
                            }
                        };
                        ctx.WriteAndFlushAsync(data);

                        if (authState == AuthState.Success)
                        {
                            _authorized = true;
                        }
                        break;
                    default:
                        _logger.Error("InvalidAuth");
                        ctx.CloseAsync();
                        break;
                }
            }
        }
    }
}
