using System;
using System.Collections.Generic;
using BattleEngine.Output;

namespace BattleEngine.Engine
{
    public class BattleOutput
    {
        private Queue<OutputEvent> _queue;
        private BattleOutputEventFactory _factory;

        public BattleOutput()
        {
            _queue = new Queue<OutputEvent>();
            _factory = new BattleOutputEventFactory();
        }

        public void enqueue(OutputEvent value)
        {
            _queue.Enqueue(value);
        }

        public OutputEvent dequeue()
        {
            return _queue.Dequeue();
        }

        public int count
        {
            get { return _queue.Count; }
        }

        public bool isEmpty
        {
            get { return _queue.Count == 0; }
        }

        public OutputEvent enqueueByFactory(Type type, int tick = 0)
        {
            var result = _factory.getInstance(type);
            result.tick = tick;
            _queue.Enqueue(result);
            return result;
        }

        public T enqueueByFactory<T>(int tick = 0) where T : OutputEvent
        {
            var result = _factory.getInstance<T>();
            result.tick = tick;
            _queue.Enqueue(result);
            return result;
        }

        public BattleOutputEventFactory factory
        {
            get { return _factory; }
        }
    }
}
