using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Common.Internal;
using SocketProxy.Handlers;

namespace SocketProxy.Users
{
    public class UserPacketsQueue
    {
        private ConcurrentQueue<UserPacket> _packets = new CompatibleConcurrentQueue<UserPacket>();

        public bool TryDequeue(out UserPacket result)
        {
            return _packets.TryDequeue(out result);
        }

        public void Enqueue(UserPacket msg)
        {
            _packets.Enqueue(msg);
        }
    }
}
