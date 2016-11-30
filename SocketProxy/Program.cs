using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace SocketProxy
{
    class Program
    {
        private static readonly string policy = "<?xml version='1.0'?><!DOCTYPE cross-domain-policy SYSTEM '/xml/dtds/cross-domain-policy.dtd'><cross-domain-policy> <allow-access-from domain='*' to-ports='*' /></cross-domain-policy>" + '\0';

        static void Main(string[] args) => RunServerAsync().Wait();

        static async Task RunServerAsync()
        {
            var mainGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();
            var server = new ServerBootstrap();
            server
                .Group(mainGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .Handler(new LoggingHandler("SERVER_LOGS"))
                .ChildHandler(new Handler(false));

            //.ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
            //{
            //    channel.Pipeline.AddLast(new Handler());
            //}));
            var boundChannel = await server.BindAsync(8899);

            Console.WriteLine("ENTER TO TERMINATE");
            Console.ReadKey();
            await boundChannel.CloseAsync();
            await
                Task.WhenAll(
                    mainGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(1))
                    );
        }

        class Handler : ChannelHandlerAdapter
        {
            private readonly bool _isDecoder;

            public Handler(bool isDecoder)
            {
                _isDecoder = isDecoder;
            }

            public override void ChannelRegistered(IChannelHandlerContext context)
            {
                context.Channel.Pipeline.AddLast(new Handler(true));
            }

            public override Task WriteAsync(IChannelHandlerContext context, object message)
            {
                if (!_isDecoder)
                {
                    IByteBuffer buffer = message as IByteBuffer;

                    if (buffer != null)
                    {
                        var str = buffer.ToString(Encoding.UTF8);
                        Console.WriteLine(str);
                    }
                }
                return base.WriteAsync(context, message);
            }

            public override void ChannelActive(IChannelHandlerContext context)
            {
                Console.WriteLine("NEW CLIENT");
                base.ChannelActive(context);
            }

            public override void ChannelRead(IChannelHandlerContext context, object message)
            {
                Console.WriteLine(message);
                IByteBuffer buffer = message as IByteBuffer;

                if (buffer != null)
                {
                    var str = buffer.ToString(Encoding.UTF8);
                    Console.WriteLine(str);
                    if (str.StartsWith("<policy-file-request/>"))
                    {
                        var bytes = Encoding.UTF8.GetBytes(policy);
                        context.WriteAsync(bytes);
                    }
                }
            }

            public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

            public override Task DisconnectAsync(IChannelHandlerContext context)
            {
                Console.WriteLine("DisconnectAsync");
                return base.DisconnectAsync(context);
            }

            public override Task CloseAsync(IChannelHandlerContext context)
            {
                Console.WriteLine("CloseAsync");
                return base.CloseAsync(context);
            }
        }
    }

}
