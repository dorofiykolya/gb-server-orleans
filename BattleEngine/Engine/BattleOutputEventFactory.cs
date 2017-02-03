using System;
using BattleEngine.Output;
using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattleOutputEventFactory
    {
        private Factory _factory;

        public BattleOutputEventFactory()
        {
            _factory = new Factory();
        }

        public OutputEvent getInstance(Type type)
        {
            return _factory.getInstance(type) as OutputEvent;
        }

        public T getInstance<T>() where T : OutputEvent
        {
            return _factory.getInstance<T>();
        }

        public void release(OutputEvent instance)
        {
            _factory.returnInstance(instance);
        }
    }
}
