﻿using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProxyPackets;

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
                            Type contentType;
                            var converted = await _packetsConverter.ConvertAsync(node.Key, node.Value, out contentType);
                            if (contentType != null)
                            {
                                context.FireChannelRead(new Packet
                                {
                                    Bytes = packet,
                                    CommandKey = node.Key,
                                    Content = converted,
                                    Data = data,
                                    ContentType = contentType
                                });
                            }
                            else
                            {
#pragma warning disable 4014
                                context.WriteAndFlushAsync(new
                                {
                                    error = new
                                    {
                                        message = $"command not found: \"{node.Key}\""
                                    }
                                });
                            }

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
#pragma warning restore 4014
            }
        }
    }
}
