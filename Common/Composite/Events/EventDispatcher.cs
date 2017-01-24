using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Composite;
using Common.Composite.Events;

namespace Common.Events
{
    public class EventDispatcher : IEventDispatcher, IDisposable
    {
        [ThreadStatic]
        private static List<List<EventDispatcher>> _bubbleChains;

        private Dictionary<object, List<EventListenerDelegate<Event>>> _eventListeners;
        private Dictionary<object, List<EventListenerDelegate<Event>>> _nativeEventListeners;

        protected void MapEvent<T>(object type, EventListenerDelegate<T> listener) where T : Event
        {
            if (_nativeEventListeners == null) _nativeEventListeners = new Dictionary<object, List<EventListenerDelegate<Event>>>();
            List<EventListenerDelegate<Event>> listeners;
            if (!_nativeEventListeners.TryGetValue(type, out listeners))
            {
                listeners = new List<EventListenerDelegate<Event>>();
                _nativeEventListeners[type] = listeners;
            }
            if (!listeners.Contains(listener))
            {
                listeners.Add((EventListenerDelegate<Event>)listener);
            }
        }

        public virtual void Dispose()
        {
            RemoveEventListeners();
        }

        public void AddEventListener(object type, EventListenerDelegate<Event> listener)
        {
            if (_eventListeners == null)
            {
                _eventListeners = new Dictionary<object, List<EventListenerDelegate<Event>>>();
            }
            List<EventListenerDelegate<Event>> listeners;
            if (!_eventListeners.TryGetValue(type, out listeners))
            {
                listeners = new List<EventListenerDelegate<Event>>();
                _eventListeners[type] = listeners;
            }
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveEventListener(object type, EventListenerDelegate<Event> listener)
        {
            if (_eventListeners != null)
            {
                List<EventListenerDelegate<Event>> listeners;
                if (_eventListeners.TryGetValue(type, out listeners) && listeners != null)
                {
                    var numListeners = listeners.Count;
                    var index = 0;
                    var restListeners = new List<EventListenerDelegate<Event>>(listeners.Count - 1);

                    for (var i = 0; i < numListeners; ++i)
                    {
                        var otherListener = listeners[i];
                        if (otherListener != listener)
                        {
                            restListeners[index++] = otherListener;
                        }
                    }

                    _eventListeners[type] = restListeners;
                }
            }
        }

        public void RemoveEventListeners(object type)
        {
            if (type != null && _eventListeners != null)
            {
                _eventListeners.Remove(type);
            }
            else
            {
                _eventListeners = null;
            }
        }

        public void RemoveEventListeners()
        {
            RemoveEventListeners(null);
        }

        public bool HasEventListener(object type, EventListenerDelegate<Event> listener)
        {
            List<EventListenerDelegate<Event>> listeners;
            _eventListeners.TryGetValue(type, out listeners);

            List<EventListenerDelegate<Event>> nativeListeners;
            _nativeEventListeners.TryGetValue(type, out nativeListeners);

            return HasEventListener(nativeListeners, type, listener) || HasEventListener(listeners, type, listener);
        }

        protected bool HasEventListener(List<EventListenerDelegate<Event>> listeners, object type, EventListenerDelegate<Event> listener)
        {
            if (listeners != null)
            {
                if (listeners.Count != 0)
                {
                    if (listener != null)
                    {
                        return listeners.Contains(listener);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool HasEventListener(object type)
        {
            return HasEventListener(type, null);
        }

        public void DispatchEvent(Event evt)
        {
            var bubbles = evt.Bubbles;

            if (!bubbles && (_eventListeners == null || !(_eventListeners.ContainsKey(evt.Type))))
            {
                return;
            }

            var previousTarget = evt.Target;
            evt.SetTarget(this);

            if (bubbles && this is Component)
            {
                BubbleEvent(evt);
            }
            else
            {
                InvokeEvent(evt);
            }
            if (previousTarget != null)
            {
                evt.SetTarget(previousTarget);
            }
        }

        internal bool InvokeEvent(Event evt)
        {
            int numListeners = 0;
            List<EventListenerDelegate<Event>> listeners = null;
            if (_eventListeners != null && _eventListeners.TryGetValue(evt.Type, out listeners) && listeners != null)
            {
                numListeners = listeners.Count;
            }

            int nativeNumListeners = 0;
            List<EventListenerDelegate<Event>> nativeListeners = null;
            if (_nativeEventListeners != null && _nativeEventListeners.TryGetValue(evt.Type, out nativeListeners) && nativeListeners != null)
            {
                nativeNumListeners = nativeListeners.Count;
            }

            if (nativeNumListeners != 0)
            {
                evt.SetCurrentTarget(this);

                for (var i = 0; i < nativeNumListeners; ++i)
                {
                    var listener = nativeListeners[i];
                    if (listener != null)
                    {
                        listener(evt);
                    }
                    if (evt.StopsImmediatePropagation)
                    {
                        return true;
                    }
                }

                return evt.StopsPropagation;
            }

            if (numListeners != 0)
            {
                evt.SetCurrentTarget(this);

                for (var i = 0; i < numListeners; ++i)
                {
                    var listener = listeners[i];
                    if (listener != null)
                    {
                        listener(evt);
                    }
                    if (evt.StopsImmediatePropagation)
                    {
                        return true;
                    }
                }

                return evt.StopsPropagation;
            }
            return false;
        }

        internal void BubbleEvent(Event evt)
        {
            List<EventDispatcher> chain;
            var element = this as Component;
            var length = 1;
            if (_bubbleChains == null) _bubbleChains = new List<List<EventDispatcher>>();
            if (_bubbleChains.Count > 0)
            {
                chain = PopFromChains();
                chain[0] = element;
            }
            else
            {
                chain = new List<EventDispatcher>();
                chain.Add(element);
            }

            while ((element = element.Parent) != null)
            {
                chain[length++] = element;
            }

            for (var i = 0; i < length; ++i)
            {
                var stopPropagation = chain[i].InvokeEvent(evt);
                if (stopPropagation)
                {
                    break;
                }
            }

            chain.Clear();
            PushToChains(chain);
        }

        public void DispatchEvent(object type, bool bubbles = false, object data = null)
        {
            if (bubbles || HasEventListener(type))
            {
                var evt = Event.FromPool<Event>(type, bubbles, data);
                DispatchEvent(evt);
                if (!evt._disposed)
                {
                    evt.Dispose();
                }
                Event.ToPool(evt);
            }
        }

        private static List<EventDispatcher> PopFromChains()
        {
            var result = _bubbleChains[_bubbleChains.Count - 1];
            _bubbleChains.RemoveAt(_bubbleChains.Count - 1);
            return result;
        }

        private void PushToChains(List<EventDispatcher> evt)
        {
            _bubbleChains.Add(evt);
        }
    }
}
