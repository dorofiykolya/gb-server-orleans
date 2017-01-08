using Grains.Objects;
using Orleans;

namespace Grains.Observers
{
    public interface IUserConnectionObserver : IGrainObserver
    {
        void Disconnect(DisconnectCause cause);
        void Request(UserRequest request);
    }
}
