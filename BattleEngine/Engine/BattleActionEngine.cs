namespace BattleEngine.Engine
{
    public class BattleActionEngine
    {
        private BattleActionQueue _actionQueue;

        public BattleActionEngine(BattleActionQueue actionQueue)
        {
            _actionQueue = actionQueue;
        }

        public void enqueue(BattleAction action)
        {
            _actionQueue.enqueue(action);
        }

        public bool isEmpty
        {
            get { return _actionQueue.isEmpty; }
        }

        public int count
        {
            get { return _actionQueue.count; }
        }
    }
}
