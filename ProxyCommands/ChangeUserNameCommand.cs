using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grains;
using Orleans;
using ProxyPackets;
using SocketProxy.Users;

namespace ProxyCommands
{
    [PacketCommand(typeof(ChangeUserNamePacket))]
    public class ChangeUserNameCommand : Command<ChangeUserNamePacket>
    {
        protected override async Task Execute()
        {
            var userInfoGrain = GrainClient.GrainFactory.GetGrain<IUserInfoGrain>(UserId);
            var newName = await userInfoGrain.ChangeName(Data.Name);
            await Context.Send(new
            {
                changeUserName = new
                {
                    name = newName
                }
            });
        }
    }
}
