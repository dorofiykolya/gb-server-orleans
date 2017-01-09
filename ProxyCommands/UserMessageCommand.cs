using System.Threading.Tasks;
using Grains;
using Grains.Objects;
using Orleans;
using ProxyPackets;
using SocketProxy.Users;

namespace ProxyCommands
{
    [PacketCommand(typeof(UserMessagePacket))]
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
