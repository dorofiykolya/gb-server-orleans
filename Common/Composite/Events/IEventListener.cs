using Common.Composite.Events;

namespace Common.Events
{
    public interface IEventListener
    {
        void AddEventListener(object type, EventListenerDelegate<Event> listener);
        void RemoveEventListener(object type, EventListenerDelegate<Event> listener);
        void RemoveEventListeners(object type);
        void RemoveEventListeners();
    }
}
