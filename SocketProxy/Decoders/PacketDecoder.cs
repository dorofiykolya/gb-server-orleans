using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ionic.Zlib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SocketProxy.Decoders
{
    public class PacketDecoder : ChannelHandlerAdapter
    {
        private readonly PacketConverter _packetsConverter;
        private readonly ConsoleServerLogger _logger;
        private bool _readLength = true;
        private int _packetLength;

        public PacketDecoder(PacketConverter packetsConverter, ConsoleServerLogger logger)
        {
            _packetsConverter = packetsConverter;
            _logger = logger;
        }

        public virtual bool AcceptInboundMessage(object msg)
        {
            return msg is IByteBuffer;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (this.AcceptInboundMessage(message))
            {
                this.Decode(context, (IByteBuffer)message);
            }
        }

        protected async void Decode(IChannelHandlerContext context, IByteBuffer message)
        {
            var buffer = message;
            while (buffer != null && buffer.ReadableBytes >= 4)
            {
                if (_readLength)
                {
                    var endianBuffer = buffer.WithOrder(ByteOrder.BigEndian);
                    _packetLength = endianBuffer.ReadInt();
                    _readLength = false;
                }
                if (buffer.ReadableBytes >= _packetLength)
                {
                    var packet = buffer.ReadBytes(_packetLength).ToArray();
                    try
                    {
                        packet = Ionic.Zlib.ZlibStream.UncompressBuffer(packet);
                        var json = Encoding.UTF8.GetString(packet);
                        var data = JsonConvert.DeserializeObject(json);
                        foreach (var node in (JObject)data)
                        {
                            var converted = await _packetsConverter.ConvertAsync(node.Key, node.Value);
                            context.FireChannelRead(new Packet
                            {
                                Bytes = packet,
                                Command = node.Key,
                                Content = converted,
                                Data = data
                            });
                        }
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
