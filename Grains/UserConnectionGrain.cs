using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grains.Objects;
using Grains.Observers;
using Orleans;

namespace Grains
{
    public class UserConnectionGrain : Grain, IUserConnectionGrain
    {
        private ObserverSubscriptionManager<IUserConnectionObserver> _subscriptionManager;

        public override Task OnActivateAsync()
        {
            _subscriptionManager = new ObserverSubscriptionManager<IUserConnectionObserver>();
            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            _subscriptionManager.Clear();
            _subscriptionManager = null;
            return base.OnDeactivateAsync();
        }

        public Task Subscribe(IUserConnectionObserver observer)
        {
            _subscriptionManager.Subscribe(observer);
            return TaskDone.Done;
        }

        public Task Unsubscribe(IUserConnectionObserver observer)
        {
            _subscriptionManager.Unsubscribe(observer);
            return TaskDone.Done;
        }

        public Task Disconnect(DisconnectCause cause)
        {
            try
            {
                _subscriptionManager.Notify((observer) =>
                {
                    observer.Disconnect(cause);
                });
            }
            catch (Exception exception)
            {
                //TODO:LOG ERROR
            }
            return TaskDone.Done;
        }

        public Task Request(UserRequest request)
        {

            try
            {
                _subscriptionManager.Notify((observer) =>
                {
                    observer.Request(request);
                });
            }
            catch (Exception exception)
            {
                //TODO:LOG ERROR
            }

            return TaskDone.Done;
        }
    }
}
