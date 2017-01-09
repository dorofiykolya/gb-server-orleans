using System.Threading.Tasks;
using Orleans;
using ProxyPackets;
using SocketProxy.Users;

namespace ProxyCommands
{
    [PacketCommand(typeof(InitUserPacket))]
    public class InitUserCommand : Command<InitUserPacket>
    {
        protected override Task Execute()
        {
            return TaskDone.Done;
        }
    }
}
