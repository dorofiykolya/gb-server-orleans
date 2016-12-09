using System.Collections;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using SocketProxy.Decoders;

namespace SocketProxy
{
    public class AuthHandler : SimpleChannelInboundHandler<Packet>
    {
        private readonly AuthManager _authManager;
        private readonly IInternalLogger _logger;
        private bool _authorized = true;

        public AuthHandler(AuthManager authManager, IInternalLogger logger)
        {
            _authManager = authManager;
            _logger = logger;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, Packet msg)
        {
            if (_authorized)
            {
                ctx.FireChannelRead(msg);
            }
            else
            {
                var dictionary = msg.Data as IDictionary;
                if (dictionary != null)
                {
                    foreach (var key in dictionary.Keys)
                    {
                        if ((string)key == "auth")
                        {
                            _logger.Warn("AUTH");
                        }
                    }
                }
            }
        }
    }
}
