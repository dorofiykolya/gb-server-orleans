using System.Collections.Generic;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace SocketProxy.Decoders
{
    public class PacketEncoder : MessageToMessageDecoder<string>
    {
        public PacketEncoder(ConsoleServerLogger logger)
        {
            
        }

        protected override void Decode(IChannelHandlerContext context, string message, List<object> output)
        {

        }
    }
}
