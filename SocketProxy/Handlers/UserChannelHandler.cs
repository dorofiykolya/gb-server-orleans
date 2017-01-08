using System;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using SocketProxy.Handlers;
using SocketProxy.Users;

namespace SocketProxy
{
    public class UserChannelHandler : SimpleChannelInboundHandler<UserPacket>
    {
        private readonly IInternalLogger _logger;
        private readonly int _userId;
        private readonly CommandFactoryProvider _provider;
        private IChannelHandlerContext _context;
        private UserContext _userContext;

        public UserChannelHandler(IInternalLogger logger, int userId, CommandFactoryProvider provider)
        {
            _logger = logger;
            _userId = userId;
            _provider = provider;
        }

        public override void HandlerAdded(IChannelHandlerContext context)
        {
            _context = context;
            _userContext = new UserContext(_context, _userId, _provider);
            base.HandlerAdded(context);
        }

        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            if (_userContext != null)
            {
                _userContext.Dispose();
            }
            _context = null;
            base.HandlerRemoved(context);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            if (_userContext != null)
            {
                _userContext.Dispose();
            }
            _context = null;
            base.ChannelUnregistered(context);
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            _userContext.Dispose();
            _logger.Error(e);
            contex.CloseAsync();
        }

        public override bool IsSharable => true;

        protected override void ChannelRead0(IChannelHandlerContext ctx, UserPacket msg)
        {
            _userContext.EnqueuePacket(msg);
        }
    }
}
