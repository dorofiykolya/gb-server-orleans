using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Grains;
using Grains.Objects;
using Grains.Observers;
using Orleans;
using SocketProxy.Handlers;

namespace SocketProxy.Users
{
    public class UserContext : IDisposable, IUserConnectionObserver
    {
        private readonly object _sync = new object();
        private readonly IChannelHandlerContext _context;
        private readonly CommandProcessor _commandProcessor;
        private Task _initializeUserTask;
        private readonly int _userId;
        private IUserConnectionObserver _observer;
        private IUserConnectionGrain _userConnectionGrain;
        private bool _disposed;
        private Task _executeTask;

        public UserContext(IChannelHandlerContext context, int userId, CommandFactoryProvider provider)
        {
            _context = context;
            _userId = userId;
            _commandProcessor = new CommandProcessor(this, provider);
            _initializeUserTask = Task.Factory.StartNew(InitializeUserGrain);
        }

        public int UserId => _userId;

        public void Dispose()
        {
            lock (_sync)
            {
                _disposed = true;
                if (_userConnectionGrain != null)
                {
                    var grain = _userConnectionGrain;
                    var observer = _observer;
                    _userConnectionGrain = null;
                    _observer = null;
                    DisposeUserGrain(grain, observer);
                }
            }
        }

        public void Disconnect(DisconnectCause cause)
        {
            _context.WriteAndFlushAsync(new
            {
                disconnect = new
                {
                    cause = cause.Message
                }
            });
            _context.CloseAsync();
            Dispose();
        }

        public void Request(UserRequest request)
        {
            _context.WriteAndFlushAsync(request.Content);
        }

        public async void EnqueuePacket(UserPacket packet)
        {
            _commandProcessor.Enqueue(packet);
            if (Interlocked.CompareExchange(ref _executeTask, null, null) == null)
            {
                _executeTask = Task.Factory.StartNew(ExecutePackets);
                await _executeTask;
                _executeTask = null;
            }
        }

        private async void ExecutePackets()
        {
            while (true)
            {
                var hasNext = await _commandProcessor.ExecuteNext();
                if (!hasNext)
                {
                    break;
                }
            }
        }

        private async void DisposeUserGrain(IUserConnectionGrain grain, IUserConnectionObserver observer)
        {
            await grain.Unsubscribe(observer);
            await GrainClient.GrainFactory.DeleteObjectReference<IUserConnectionObserver>(observer);
        }

        private async void InitializeUserGrain()
        {
            var connectionGrain = GrainClient.GrainFactory.GetGrain<IUserConnectionGrain>(_userId);
            await connectionGrain.Disconnect(new DisconnectCause
            {
                Message = "new connection"
            });
            var observer = await GrainClient.GrainFactory.CreateObjectReference<IUserConnectionObserver>(this);
            await connectionGrain.Subscribe(observer);

            bool disposed;
            lock (_sync)
            {
                _userConnectionGrain = connectionGrain;
                _observer = observer;
                disposed = _disposed;
            }
            if (disposed)
            {
                DisposeUserGrain(connectionGrain, observer);
            }
        }
    }
}
