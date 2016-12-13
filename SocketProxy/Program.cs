using System;
using System.Threading;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using DotNetty.Common.Internal.Logging;
using Orleans;
using SocketProxy.Decoders;

namespace SocketProxy
{
    class Program
    {
        static async Task Run()
        {
            if (false)
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
            Thread.Sleep(1000);
            var result = new OrleansClientResult();
            var count = 3;
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
            var clientManager = new ClientManager();
            var authManager = new AuthManager();
            var logger = new ConsoleServerLogger();

            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
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
                        pipeline.AddLast(new PacketDecoder(logger), new PacketEncoder(logger));
                        //auth
                        pipeline.AddLast(new AuthHandler(authManager, logger));
                        //handler
                        pipeline.AddLast(new ClientChannelHandler(clientManager, authManager, logger));
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

        static void Main() => Run().Wait();

        class OrleansClientResult
        {
            public bool Initialized { get; set; }
            public Exception Exception { get; set; }
        }
    }

}
