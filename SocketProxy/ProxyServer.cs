﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Common.Internal.Logging;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Orleans;
using SocketProxy.Decoders;
using SocketProxy.Users;

namespace SocketProxy
{
    public class ProxyServer
    {
        public async Task RunAsync()
        {
            if (true)
            {
                var status = RunGrainClient();
                if (status.Exception != null)
                {
                    Console.WriteLine("GrainClient not started, exception:" + status.Exception);
                    Console.ReadKey();
                    return;
                }
            }
            await RunServerAsync();
        }

        static OrleansClientResult RunGrainClient()
        {
            Console.WriteLine("Wait To Start Silo");
            Thread.Sleep(1000);
            var result = new OrleansClientResult();
            var count = 5;
            while (count-- > 0)
            {
                try
                {
                    GrainClient.Initialize("ProxyConfig.xml");
                    result.Initialized = GrainClient.IsInitialized;
                }
                catch (Exception exception)
                {
                    if (count <= 0)
                    {
                        result.Exception = exception;
                        break;
                    }
                    Thread.Sleep(1500);
                    Console.WriteLine("Try To Reconnect GrainClient");
                }
            }

            Console.WriteLine("GrainClient-IsInitialized:" + GrainClient.IsInitialized);
            return result;
        }

        static async Task RunServerAsync()
        {
            var authManager = new AuthManager();
            var logger = new ConsoleServerLogger();
            var userCommandProvider = new UserCommandFactoryProvider();

            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
                var packetsConverter = new UserPacketConverter();
                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler(LogLevel.TRACE))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new LoggerHandler(logger, InternalLogLevel.TRACE));
                        //policy
                        pipeline.AddLast(new ClientPolicyHandler(logger), new ClientPolicyWriter(logger));
                        //test decode / encode
                        //pipeline.AddLast(new StringEncoder(), new StringDecoder());
                        //packet encoder
                        pipeline.AddLast(new PacketEncoder(logger));
                        //packet decoder
                        pipeline.AddLast(new PacketDecoder(packetsConverter, logger));
                        //auth
                        pipeline.AddLast(new AuthHandler(authManager, logger, userCommandProvider));
                    }));

                IChannel bootstrapChannel = await bootstrap.BindAsync(8899);

                Console.WriteLine("Press Enter To Exit");
                Console.ReadLine();

                await bootstrapChannel.CloseAsync();
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }

        class OrleansClientResult
        {
            public bool Initialized { get; set; }
            public Exception Exception { get; set; }
        }
    }
}
