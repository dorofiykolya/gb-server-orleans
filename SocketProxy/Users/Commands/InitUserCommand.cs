using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using SocketProxy.Packets;

namespace SocketProxy.Users.Commands
{
    public class InitUserCommand : Command<InitUserPacket>
    {
        protected override Task Execute()
        {

            return TaskDone.Done;
        }
    }
}
