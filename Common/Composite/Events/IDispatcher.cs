using Common.Composite.Events;

namespace Common.Events
{
    public interface IDispatcher
    {
        void DispatchEvent(Event evt);
        void DispatchEvent(object type, bool bubbles = false, object data = null);
        bool HasEventListener(object type, EventListenerDelegate<Event> listener = null);
    }
}
