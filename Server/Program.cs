using Orleans.Runtime.Host;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var silo = new SiloHost("SILO");
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            //GrainClient.GrainFactory.GetGrain<IGrainWithStringKey>("");
            
            System.Console.ReadKey();
        }
    }
}
