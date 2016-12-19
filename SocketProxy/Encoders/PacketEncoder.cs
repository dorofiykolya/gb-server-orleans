using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Newtonsoft.Json;

namespace SocketProxy.Decoders
{
    public class PacketEncoder : MessageToMessageEncoder<object>
    {
        private readonly ConsoleServerLogger _logger;

        public PacketEncoder(ConsoleServerLogger logger)
        {
            _logger = logger;
        }

        protected override void Encode(IChannelHandlerContext context, object message, List<object> output)
        {
            if (!(message is string))
            {
                message = JsonConvert.SerializeObject(message);
            }
            if (message != null)
            {
                var bytes = Encoding.UTF8.GetBytes((string)message);
                var compressed = Ionic.Zlib.ZlibStream.CompressBuffer(bytes);
                var len = compressed.Length;
                var buffer = context.Allocator.Buffer(len + 4);
                if (buffer.Order != ByteOrder.BigEndian)
                {
                    buffer = buffer.WithOrder(ByteOrder.BigEndian);
                }
                buffer.WriteInt(len);
                buffer.WriteBytes(compressed);
                context.WriteAndFlushAsync(buffer);
            }
        }
    }
}
