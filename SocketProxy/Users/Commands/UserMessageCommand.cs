using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grains;
using Grains.Objects;
using Orleans;
using SocketProxy.Packets;

namespace SocketProxy.Users.Commands
{
    public class UserMessageCommand : Command<UserMessagePacket>
    {
        protected override Task Execute()
        {
            var user = GrainClient.GrainFactory.GetGrain<IUserConnectionGrain>(Data.UserId);
            return user.Request(new UserRequest
            {
                Content = new UserMessage
                {
                    UserId = Data.UserId,
                    Message = Data.Message
                }
            });
        }
    }
}
