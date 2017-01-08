using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Common.Internal;
using Microsoft.Extensions.Logging;
using Orleans;
using SocketProxy.Decoders;
using SocketProxy.Handlers;
using SocketProxy.Users;
using SocketProxy.Users.Commands;

namespace SocketProxy
{
    public class CommandProcessor : IDisposable
    {
        private readonly UserContext _userContext;
        private readonly CommandFactoryProvider _factoryProvider;
        private ConcurrentQueue<UserPacket> _packets;

        public CommandProcessor(UserContext userContext, CommandFactoryProvider factoryProvider)
        {
            _userContext = userContext;
            _factoryProvider = factoryProvider;
            _packets = new ConcurrentQueue<UserPacket>();
        }

        public async Task<bool> ExecuteNext()
        {
            UserPacket packet;
            if (_packets.TryDequeue(out packet))
            {
                try
                {
                    ICommand command = _factoryProvider.CreateInstance(packet.ContentType);
                    if (command != null)
                    {
                        var result = command.Execute(_userContext, packet.Content);
                        await result;
                        if (result.Exception?.InnerException != null)
                        {
                            throw result.Exception.InnerException;
                        }
                    }
                }
                catch (Exception exception)
                {
                    //TODO:LOG ERROR
                }

                return !_packets.IsEmpty;
            }
            return false;
        }

        public void Enqueue(UserPacket msg)
        {
            _packets.Enqueue(msg);
        }

        public void Dispose()
        {
            _packets = null;
        }
    }
}
