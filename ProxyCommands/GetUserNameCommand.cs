using System.Threading.Tasks;
using Grains;
using Orleans;
using ProxyPackets;
using SocketProxy.Users;

namespace ProxyCommands
{
    [PacketCommand(typeof(GetUserNamePacket))]
    public class GetUserNameCommand : Command<GetUserNamePacket>
    {
        protected override async Task Execute()
        {
            var userInfoGrain = GrainClient.GrainFactory.GetGrain<IUserInfoGrain>(UserId);
            var name = await userInfoGrain.GetName();
            await Context.Send(new
            {
                userName = new
                {
                    name = name
                }
            });
        }
    }
}
