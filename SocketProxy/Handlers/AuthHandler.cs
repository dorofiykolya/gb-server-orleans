using System.Collections.Concurrent;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using Newtonsoft.Json.Linq;
using SocketProxy.Decoders;

namespace SocketProxy
{
    public class AuthHandler : SimpleChannelInboundHandler<Packet>
    {
        private readonly AuthManager _authManager;
        private readonly IInternalLogger _logger;
        private bool _authorized = false;
        private bool _checkAuth = false;
        private readonly ConcurrentQueue<Packet> _queue;

        public AuthHandler(AuthManager authManager, IInternalLogger logger)
        {
            _queue = new ConcurrentQueue<Packet>();
            _authManager = authManager;
            _logger = logger;
        }

        protected override async void ChannelRead0(IChannelHandlerContext ctx, Packet msg)
        {
            if (_authorized)
            {
                ctx.FireChannelRead(msg);
            }
            else if (!_checkAuth)
            {
                var jObject = msg.GetData<JObject>();
                if (jObject != null)
                {
                    var isValid = false;
                    foreach (var key in jObject)
                    {
                        switch (key.Key)
                        {
                            case "authByDevelopers":
                                _checkAuth = true;
                                isValid = await _authManager.CheckAuthByDevelopers(key.Value["userKey"].Value<string>());
                                break;
                            case "authByAndroid":
                                _checkAuth = true;
                                break;
                            case "authByBrowser":
                                _checkAuth = true;
                                isValid = await _authManager.CheckAuthByBrowser(key.Value);
                                break;
                            case "authByIOS":
                                _checkAuth = true;
                                break;
                            default:
                                _logger.Error("InvalidAuth");
                                await ctx.CloseAsync();
                                break;
                        }
                    }
                    if (isValid)
                    {

                    }
                }
            }
            else
            {
                _queue.Enqueue(msg);
            }
        }
    }
}
