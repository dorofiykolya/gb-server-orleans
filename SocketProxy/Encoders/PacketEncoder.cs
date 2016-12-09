using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace SocketProxy.Decoders
{
    public class PacketEncoder : MessageToMessageDecoder<string>
    {
        private readonly ConsoleServerLogger _logger;

        public PacketEncoder(ConsoleServerLogger logger)
        {
            _logger = logger;
        }

        protected override void Decode(IChannelHandlerContext context, string message, List<object> output)
        {
            _logger.Info("DECODE");
        }
    }
}
