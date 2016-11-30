using System;
using System.Net;
using Orleans.Runtime.Host;
using StorageProvider;

namespace Silo
{
    public class SiloServer : IDisposable
    {
        public const string SiloServerConfig = "SiloServerConfig.xml";

        private readonly SiloHost _silo;

        public SiloServer()
        {
            _silo = new SiloHost(Dns.GetHostName())
            {
                ConfigFileName = SiloServerConfig
            };
        }

        public SiloRunResult Run()
        {
            bool started = false;

            try
            {
                _silo.LoadOrleansConfig();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new SiloRunResult { status = SiloRunStatus.Error };
            }

            try
            {
                _silo.InitializeOrleansSilo();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new SiloRunResult { status = SiloRunStatus.Error };
            }

            try
            {
                started = _silo.StartOrleansSilo();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new SiloRunResult { status = SiloRunStatus.Error };
            }

            if (!started)
            {
                return new SiloRunResult { status = SiloRunStatus.Error };
            }

            return new SiloRunResult { status = SiloRunStatus.Ready };
        }

        public bool Stop()
        {
            try
            {
                _silo.ShutdownOrleansSilo();
                _silo.WaitForOrleansSiloShutdown();
            }
            catch (Exception exception)
            {
                _silo.ReportStartupError(exception);
                Console.WriteLine(exception);

                return false;
            }
            return true;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
