using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace SocketProxy.Users.Commands
{
    public class InitUserCommand : Command<InitUserCommand>
    {
        protected override Task Execute()
        {
            
            return TaskDone.Done;
        }
    }
}
