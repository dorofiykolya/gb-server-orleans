using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProxy.Packets;
using SocketProxy.Users.Commands;

namespace SocketProxy.Users
{
    public class UserCommandFactoryProvider : CommandFactoryProvider
    {
        public UserCommandFactoryProvider()
        {
            Add<InitUserPacket, InitUserCommand>();
            Add<UserMessagePacket, UserMessageCommand>();
        }
    }
}
