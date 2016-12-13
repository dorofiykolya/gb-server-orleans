using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Newtonsoft.Json;

namespace SocketProxy.Decoders
{
    public class PacketDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        private readonly ConsoleServerLogger _logger;
        private bool _readLength = true;
        private int _packetLength;

        public PacketDecoder(ConsoleServerLogger logger)
        {
            _logger = logger;
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            var buffer = message;
            while (buffer != null && buffer.ReadableBytes >= 4)
            {
                if (_readLength)
                {
                    var endianBuffer = buffer.WithOrder(ByteOrder.LittleEndian);
                    _packetLength = endianBuffer.ReadInt();
                    _readLength = false;
                }
                if (buffer.ReadableBytes >= _packetLength)
                {
                    var packet = buffer.ReadBytes(_packetLength);
                    try
                    {
                        var data = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(packet.ToArray()));
                        output.Add(new Packet { Data = data });
                        _readLength = true;
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(exception);
                        context.CloseAsync();
                    }
                }
                break;
            }
        }
    }
}
