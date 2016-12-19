using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProxy.Decoders;
using SocketProxy.Handlers;

namespace SocketProxy
{
    public class UserCommandProcessor
    {
        private readonly UserChannelHandler _userChannelHandler;

        public UserCommandProcessor(UserChannelHandler userChannelHandler)
        {
            _userChannelHandler = userChannelHandler;
        }

        public void Execute(Packet packet)
        {

        }

        public void Enqueue(UserPacket msg)
        {

        }
    }
}
