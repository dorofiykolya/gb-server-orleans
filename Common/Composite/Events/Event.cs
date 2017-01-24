using System;
using System.Collections.Generic;
using Common.Events;

namespace Common.Composite.Events
{
    public class Event : IDisposable
    {
        public const string ADDED = "added";
        public const string REMOVED = "removed";
        public const string TRIGGERED = "triggered";
        public const string COMPLETE = "complete";
        public const string CHANGE = "change";
        public const string CANCEL = "cancel";
        public const string OPEN = "open";
        public const string CLOSE = "close";
        public const string SELECT = "select";


        internal bool _disposed;
        internal bool _inPool;

        private EventDispatcher _target;
        private EventDispatcher _currentTarget;
        private object _type;
        private bool _bubbles;
        private bool _stopsPropagation;
        private bool _stopsImmediatePropagation;
        private object _data;

        public Event(object type, bool bubbles = false, object data = null)
        {
            _type = type;
            _bubbles = bubbles;
            _data = data;
        }

        public void StopPropagation()
        {
            _stopsPropagation = true;
        }

        public void StopImmediatePropagation()
        {
            _stopsPropagation = _stopsImmediatePropagation = true;
        }

        public override string ToString()
        {
            return $"[{GetType().Name} type=\"{_type}\" bubbles={_bubbles}]";
        }

        public bool Bubbles
        {
            get { return _bubbles; }
        }

        public EventDispatcher Target
        {
            get { return _target; }
        }

        public EventDispatcher CurrentTarget
        {
            get { return _currentTarget; }
        }

        public object Type
        {
            get { return _type; }
        }

        public object Data
        {
            get { return _data; }
        }

        internal void SetTarget(EventDispatcher value)
        {
            _target = value;
        }

        internal void SetCurrentTarget(EventDispatcher value)
        {
            _currentTarget = value;
        }

        internal void SetData(object value)
        {
            _data = value;
        }

        internal bool StopsPropagation
        {
            get { return _stopsPropagation; }
        }

        internal bool StopsImmediatePropagation
        {
            get { return _stopsImmediatePropagation; }
        }

        internal Event ReinitializeEvent(object type, bool bubbles, object data, object[] args)
        {
            _type = type;
            _bubbles = bubbles;
            _data = data;
            _target = _currentTarget = null;
            _stopsPropagation = _stopsImmediatePropagation = false;
            InitializeEvent(args);
            return this;
        }

        public void Dispose()
        {
            _disposed = true;
            Reset(null, false);
        }

        internal Event Reset(object type, bool bubbles = false, object data = null)
        {
            _type = type;
            _bubbles = bubbles;
            _data = data;
            _target = _currentTarget = null;
            _stopsPropagation = _stopsImmediatePropagation = false;
            return this;
        }

        protected virtual Event InitializeEvent(object[] args)
        {
            return this;
        }

        [ThreadStatic]
        private static Dictionary<Type, Stack<Event>> eventPool = new Dictionary<Type, Stack<Event>>();

        public static T FromPool<T>(object type, bool bubbles = false, object data = null) where T : Event
        {
            Event result;
            Stack<Event> stack;
            if (eventPool.TryGetValue(typeof(T), out stack) && stack.Count != 0)
            {
                result = stack.Pop().ReinitializeEvent(type, bubbles, data, null);
            }
            else
            {
                result = Activator.CreateInstance(typeof(T), type) as Event;
                result.ReinitializeEvent(type, bubbles, data, null);
            }
            result._disposed = false;
            result._inPool = false;
            return (T)result;
        }

        public static void ToPool<T>(T evt) where T : Event
        {
            if (evt._inPool) return;
            evt._data = evt._target = evt._currentTarget = null;
            evt._inPool = true;
            Stack<Event> stack;
            if (!eventPool.TryGetValue(evt.GetType(), out stack))
            {
                eventPool[evt.GetType()] = stack = new Stack<Event>();
            }
            stack.Push(evt);
        }
    }
}
