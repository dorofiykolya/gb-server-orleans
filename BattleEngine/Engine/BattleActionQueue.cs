using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattleActionQueue
    {
        private BattleConfiguration _configuration;
        private PriorityQueueComparable<BattleAction> _queue;

        public BattleActionQueue(BattleConfiguration configuration)
        {
            _configuration = configuration;
            _queue = new PriorityQueueComparable<BattleAction>();
        }

        public void enqueue(BattleAction action)
        {
            _queue.enqueue(action);
        }

        public BattleAction peek()
        {
            return _queue.peek();
        }

        public BattleAction dequeue()
        {
            return _queue.dequeue();
        }

        public int count
        {
            get { return _queue.count; }
        }

        public bool isEmpty
        {
            get { return _queue.count == 0; }
        }
    }
}
