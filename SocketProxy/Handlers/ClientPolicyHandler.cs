using System;
using System.Linq;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;

namespace SocketProxy
{
    public class ClientPolicyWriter : SimpleChannelInboundHandler<PolicyChecked>
    {
        private readonly IInternalLogger _logger;
        public static readonly string Policy = "<?xml version='1.0'?><!DOCTYPE cross-domain-policy SYSTEM '/xml/dtds/cross-domain-policy.dtd'><cross-domain-policy> <allow-access-from domain='*' to-ports='*' /></cross-domain-policy>";
        public static readonly byte[] PolicyBytes = Encoding.UTF8.GetBytes(Policy).Concat(new byte[] { 0 }).ToArray();
        public static readonly IByteBuffer PolicyBuffer = Unpooled.WrappedBuffer(PolicyBytes);

        public ClientPolicyWriter(IInternalLogger logger)
        {
            _logger = logger;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, PolicyChecked msg)
        {
            if (msg.Success && msg.Type == PolicyType.Flash)
            {
                ctx.WriteAndFlushAsync(PolicyBuffer);
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            Console.WriteLine("{0}", e.StackTrace);
            contex.CloseAsync();
        }

        public override bool IsSharable => true;
    }

    public class ClientPolicyHandler : ChannelHandlerAdapter
    {
        private readonly IInternalLogger _logger;
        public static readonly string Request = "<policy-file-request/>";
        public static readonly byte[] RequestBytes = Encoding.UTF8.GetBytes(Request).Concat(new byte[] { 0 }).ToArray();
        public static readonly IByteBuffer RequestBuffer = Unpooled.WrappedBuffer(RequestBytes);

        private bool _checked;
        private bool _needSkip;

        public ClientPolicyHandler(IInternalLogger logger)
        {
            _logger = logger;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Decode(context, (IByteBuffer)message);
        }

        protected void Decode(IChannelHandlerContext context, IByteBuffer buffer)
        {
            if (_needSkip || _checked)
            {
                context.FireChannelRead(buffer);
                return;
            }

            if (buffer.ReadableBytes != 0)
            {
                for (int i = 0; i < RequestBuffer.Capacity; i++)
                {
                    if (buffer.ReadableBytes > i)
                    {
                        if (buffer.GetByte(i) != RequestBuffer.GetByte(i))
                        {
                            _logger.Info("POLICY SKIP" + context.Channel.RemoteAddress);

                            _needSkip = true;
                            context.FireChannelRead(buffer);
                            return;
                        }
                    }
                    else
                    {
                        Fail(context);
                        return;
                    }
                }
                _checked = true;

                _logger.Info("POLICY CHECKED" + context.Channel.RemoteAddress);

                buffer.SkipBytes(RequestBytes.Length);
                context.FireChannelRead(new PolicyChecked
                {
                    Success = true,
                    Type = PolicyType.Flash
                });
            }
        }

        private void Fail(IChannelHandlerContext context)
        {
            _logger.Info("POLICY NOT FULL" + context.Channel.RemoteAddress);

            //throw new DecoderException("wait full request" + context.Channel.RemoteAddress);
        }
    }
}
