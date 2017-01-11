using System;
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

        public static T FromPool<T>(object type, bool bubbles = false, object data = null) where T : Event
        {
            return null;
        }

        /* INTERFACE common.system.IDisposable */

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
    }
}
