using System;
using System.Collections.Generic;

namespace BattleEngine.Utils
{
    public class Factory : IDisposable
    {
        private Dictionary<Type, Stack<object>> _dictionary;

        public object newInstance(Type type, params object[] constructorArgs)
        {
            return Activator.CreateInstance(type, constructorArgs);
        }

        public object getInstance(Type type, params object[] constructorArgs)
        {
            _dictionary = _dictionary ?? (_dictionary = new Dictionary<Type, Stack<object>>());

            Stack<object> collection;
            if (_dictionary.TryGetValue(type, out collection) && collection.Count != 0)
            {
                return collection.Pop();
            }
            return Activator.CreateInstance(type, constructorArgs);
        }

        public T getInstance<T>()
        {
            return (T)getInstance(typeof(T));
        }

        public void returnInstance(object instance)
        {
            if (instance != null && !(instance is Type))
            {
                _dictionary = _dictionary ?? (_dictionary = new Dictionary<Type, Stack<object>>());
                var type = instance.GetType();

                Stack<object> collection;
                if (!_dictionary.TryGetValue(type, out collection))
                {
                    _dictionary[type] = collection = new Stack<object>();
                }
                collection.Push(instance);
            }
        }

        /* INTERFACE common.system.IDisposable */

        public void Dispose()
        {
            _dictionary = null;
        }
    }
}
