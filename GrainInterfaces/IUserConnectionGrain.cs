using System.Threading.Tasks;
using Grains.Objects;
using Grains.Observers;
using Orleans;

namespace Grains
{
    public interface IUserConnectionGrain : IGrainWithIntegerKey
    {
        Task Subscribe(IUserConnectionObserver observer);
        Task Unsubscribe(IUserConnectionObserver observer);
        Task Disconnect(DisconnectCause cause);
        Task Request(UserRequest request);

    }
}
