namespace SocketProxy
{
    class Program
    {
        static void Main() => new ProxyServer().RunAsync().Wait();
    }

}
