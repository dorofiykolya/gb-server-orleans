using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;

namespace SocketProxy
{
    public class LoggerHandler : ChannelHandlerAdapter
    {
        private readonly IInternalLogger Logger;
        private readonly InternalLogLevel InternalLevel;

        public LoggerHandler(IInternalLogger logger, InternalLogLevel level)
        {
            Logger = logger;
            InternalLevel = level;
        }

        public override void ChannelRegistered(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "REGISTERED"));
            ctx.FireChannelRegistered();
        }

        public override void ChannelUnregistered(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "UNREGISTERED"));
            ctx.FireChannelUnregistered();
        }

        public override void ChannelActive(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "ACTIVE"));
            ctx.FireChannelActive();
        }

        public override void ChannelInactive(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "INACTIVE"));
            ctx.FireChannelInactive();
        }

        public override void ExceptionCaught(IChannelHandlerContext ctx, Exception cause)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "EXCEPTION", (object)cause), cause);
            ctx.FireExceptionCaught(cause);
        }

        public override void UserEventTriggered(IChannelHandlerContext ctx, object evt)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "USER_EVENT", evt));
            ctx.FireUserEventTriggered(evt);
        }

        public override Task BindAsync(IChannelHandlerContext ctx, EndPoint localAddress)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "BIND", (object)localAddress));
            return ctx.BindAsync(localAddress);
        }

        public override Task ConnectAsync(IChannelHandlerContext ctx, EndPoint remoteAddress, EndPoint localAddress)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "CONNECT", (object)remoteAddress, (object)localAddress));
            return ctx.ConnectAsync(remoteAddress, localAddress);
        }

        public override Task DisconnectAsync(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "DISCONNECT"));
            return ctx.DisconnectAsync();
        }

        public override Task CloseAsync(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "CLOSE"));
            return ctx.CloseAsync();
        }

        public override Task DeregisterAsync(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "DEREGISTER"));
            return ctx.DeregisterAsync();
        }

        public override void ChannelRead(IChannelHandlerContext ctx, object message)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "RECEIVED", message));
            ctx.FireChannelRead(message);
        }

        public override Task WriteAsync(IChannelHandlerContext ctx, object msg)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "WRITE", msg));
            return ctx.WriteAsync(msg);
        }

        public override void Flush(IChannelHandlerContext ctx)
        {
            if (this.Logger.IsEnabled(this.InternalLevel))
                this.Logger.Log(this.InternalLevel, this.Format(ctx, "FLUSH"));
            ctx.Flush();
        }

        protected string Format(IChannelHandlerContext ctx, string eventName)
        {
            string str = ctx.Channel.ToString();
            return new StringBuilder(str.Length + 1 + eventName.Length).Append(str).Append(' ').Append(eventName).ToString();
        }

        protected string Format(IChannelHandlerContext ctx, string eventName, object arg)
        {
            if (arg is IByteBuffer)
                return this.FormatByteBuffer(ctx, eventName, (IByteBuffer)arg);
            if (arg is IByteBufferHolder)
                return this.FormatByteBufferHolder(ctx, eventName, (IByteBufferHolder)arg);
            return this.FormatSimple(ctx, eventName, arg);
        }

        protected string Format(IChannelHandlerContext ctx, string eventName, object firstArg, object secondArg)
        {
            if (secondArg == null)
                return this.FormatSimple(ctx, eventName, firstArg);
            string str1 = ctx.Channel.ToString();
            string str2 = firstArg.ToString();
            string str3 = secondArg.ToString();
            StringBuilder stringBuilder = new StringBuilder(str1.Length + 1 + eventName.Length + 2 + str2.Length + 2 + str3.Length);
            string str4 = str1;
            stringBuilder.Append(str4).Append(' ').Append(eventName).Append(": ").Append(str2).Append(", ").Append(str3);
            return stringBuilder.ToString();
        }

        private string FormatByteBuffer(IChannelHandlerContext ctx, string eventName, IByteBuffer msg)
        {
            string str1 = ctx.Channel.ToString();
            int readableBytes = msg.ReadableBytes;
            if (readableBytes == 0)
            {
                StringBuilder stringBuilder = new StringBuilder(str1.Length + 1 + eventName.Length + 4);
                string str2 = str1;
                stringBuilder.Append(str2).Append(' ').Append(eventName).Append(": 0B");
                return stringBuilder.ToString();
            }
            int num = readableBytes / 16 + (readableBytes % 15 == 0 ? 0 : 1) + 4;
            StringBuilder dump = new StringBuilder(str1.Length + 1 + eventName.Length + 2 + 10 + 1 + 2 + num * 80);
            string str3 = str1;
            dump.Append(str3).Append(' ').Append(eventName).Append(": ").Append(readableBytes).Append('B').Append('\n');
            IByteBuffer buf = msg;
            ByteBufferUtil.AppendPrettyHexDump(dump, buf);
            return dump.ToString();
        }

        private string FormatByteBufferHolder(IChannelHandlerContext ctx, string eventName, IByteBufferHolder msg)
        {
            string str1 = ctx.Channel.ToString();
            string str2 = msg.ToString();
            IByteBuffer content = msg.Content;
            int readableBytes = content.ReadableBytes;
            if (readableBytes == 0)
            {
                StringBuilder stringBuilder = new StringBuilder(str1.Length + 1 + eventName.Length + 2 + str2.Length + 4);
                string str3 = str1;
                stringBuilder.Append(str3).Append(' ').Append(eventName).Append(", ").Append(str2).Append(", 0B");
                return stringBuilder.ToString();
            }
            int num = readableBytes / 16 + (readableBytes % 15 == 0 ? 0 : 1) + 4;
            StringBuilder dump = new StringBuilder(str1.Length + 1 + eventName.Length + 2 + str2.Length + 2 + 10 + 1 + 2 + num * 80);
            string str4 = str1;
            dump.Append(str4).Append(' ').Append(eventName).Append(": ").Append(str2).Append(", ").Append(readableBytes).Append('B').Append('\n');
            IByteBuffer buf = content;
            ByteBufferUtil.AppendPrettyHexDump(dump, buf);
            return dump.ToString();
        }

        private string FormatSimple(IChannelHandlerContext ctx, string eventName, object msg)
        {
            string str1 = ctx.Channel.ToString();
            string str2 = msg.ToString();
            return new StringBuilder(str1.Length + 1 + eventName.Length + 2 + str2.Length).Append(str1).Append(' ').Append(eventName).Append(": ").Append(str2).ToString();
        }
    }
}
