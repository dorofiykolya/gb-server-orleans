using System.Threading.Tasks;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using ProxyPackets;
using SocketProxy.Decoders;
using SocketProxy.Handlers;
using SocketProxy.Requests;
using SocketProxy.Users;

namespace SocketProxy
{
    public class AuthHandler : SimpleChannelInboundHandler<Packet>
    {
        private readonly AuthManager _authManager;
        private readonly IInternalLogger _logger;
        private readonly CommandFactoryProvider _provider;
        private bool _authorized = false;
        private bool _checkAuth = false;
        private Auth _auth;
        private UserChannelHandler _userHandler;
        private Task _authTask;

        public AuthHandler(AuthManager authManager, IInternalLogger logger, CommandFactoryProvider provider)
        {
            _authManager = authManager;
            _logger = logger;
            _provider = provider;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, Packet msg)
        {
            if (_authorized)
            {
                ctx.FireChannelRead(new UserPacket(msg, _auth));
            }
            else if (!_checkAuth)
            {
                switch ((string)msg.CommandKey)
                {
                    case "authByDeveloper":
                        _authTask?.Dispose();
                        _authTask = Task.Factory.StartNew(() =>
                       {
                           var authTask = _authManager.GetAuthByDevelopers(msg.ContentAs<AuthByDeveloperPacket>().DeveloperId);
                           authTask.Wait();
                           _auth = authTask.Result;
                           var info = new UserAuthInfoRequest { userAuthInfo = _auth };
                           InitializeUserHandler(ctx, info.userAuthInfo.UserId);
                           ctx.WriteAndFlushAsync(info);
                       });
                        _authorized = true;
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
                        _authTask?.Dispose();
                        _authTask = Task.Factory.StartNew(() =>
                       {
                           var authState = _authManager.CheckAuth(userAuth.UserId, userAuth.AuthKey, userAuth.AuthTs, userAuth.IsBrowser);
                           Task.WaitAll(authState);
                           var data = new UserAuthStateRequest
                           {
                               UserAuthState = new UserAuthStateData
                               {
                                   State = (int)authState.Result,
                                   MinVersion = "0.0.0"
                               }
                           };
                           InitializeUserHandler(ctx, userAuth.UserId);
                           ctx.WriteAndFlushAsync(data);

                           if (authState.Result == AuthState.Success)
                           {
                               _authorized = true;
                           }
                       });

                        break;
                    default:
                        _logger.Error("InvalidAuth");
                        ctx.CloseAsync();
                        break;
                }
            }
        }

        protected void InitializeUserHandler(IChannelHandlerContext ctx, int userId)
        {
            if (_userHandler != null)
            {
                ctx.Channel.Pipeline.Remove(_userHandler);
            }
            _userHandler = new UserChannelHandler(_logger, userId, _provider);
            ctx.Channel.Pipeline.AddLast(_userHandler);
        }
    }
}
