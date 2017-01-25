using System.Collections.Generic;

namespace BattleEngine.Engine
{
    public class BattleCommandsProvider
    {
        private List<BattleEngineCommand> _collection;

        public BattleCommandsProvider()
        {
            _collection = new List<BattleEngineCommand>();
        }

        public BattleCommandsProvider add(BattleEngineCommand command)
        {
            _collection.Add(command);
            return this;
        }

        public IEnumerable<BattleEngineCommand> commands
        {
            get { return _collection; }
        }
    }
}
