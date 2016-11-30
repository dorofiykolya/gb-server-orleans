using System;

namespace Silo
{
    class Program
    {
        private static SiloServer _siloServer;

        static void Main(string[] args)
        {
            var domain = AppDomain.CreateDomain("Silo Server", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args
            });

            Console.WriteLine("Press 'Enter' key to stop");
            Console.ReadLine();

            domain.DoCallBack(ShutdownSilo);
            Environment.Exit(0);
        }

        private static void ShutdownSilo()
        {
            _siloServer.Stop();
            GC.SuppressFinalize(_siloServer);
        }

        private static void InitSilo(string[] args)
        {
            _siloServer = new SiloServer();
            var status = _siloServer.Run();
            switch (status.status)
            {
                case SiloRunStatus.Ready:
                    Console.WriteLine("Silo server started");

                    break;
                case SiloRunStatus.Error:
                    Console.WriteLine("Silo server start error");
                    break;
            }
        }
    }
}
