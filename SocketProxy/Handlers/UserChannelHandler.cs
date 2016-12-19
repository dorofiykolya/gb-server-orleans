using System;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using SocketProxy.Handlers;

namespace SocketProxy
{
    public class UserChannelHandler : SimpleChannelInboundHandler<UserPacket>
    {
        private readonly ClientManager _clientManager;
        private readonly AuthManager _authManager;
        private readonly IInternalLogger _logger;
        private readonly UserCommandProcessor _userCommandProcessor;

        public UserChannelHandler(ClientManager clientManager, AuthManager authManager, IInternalLogger logger)
        {
            _clientManager = clientManager;
            _authManager = authManager;
            _logger = logger;
            _userCommandProcessor = new UserCommandProcessor(this);
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            _logger.Error(e);
            contex.CloseAsync();
        }

        public override bool IsSharable => true;

        protected override void ChannelRead0(IChannelHandlerContext ctx, UserPacket msg)
        {
            _userCommandProcessor.Enqueue(msg);
        }
    }
}
