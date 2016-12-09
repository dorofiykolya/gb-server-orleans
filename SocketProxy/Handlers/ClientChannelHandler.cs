using System;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;

namespace SocketProxy
{
    public class ClientChannelHandler : SimpleChannelInboundHandler<string>
    {
        private readonly ClientManager _clientManager;
        private readonly AuthManager _authManager;
        private readonly IInternalLogger _logger;

        public ClientChannelHandler(ClientManager clientManager, AuthManager authManager, IInternalLogger logger)
        {
            _clientManager = clientManager;
            _authManager = authManager;
            _logger = logger;
        }

        protected override void ChannelRead0(IChannelHandlerContext contex, string msg)
        {
            contex.WriteAndFlushAsync(msg);
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            _logger.Error(e);
            contex.CloseAsync();
        }

        public override bool IsSharable => true;
    }
}
