using System;

namespace BattleEngine.Engine
{
    public class BattleEngineCommand
    {
        private Type _actionType;

        public BattleEngineCommand(Type actionType)
        {
            _actionType = actionType;
        }

        public virtual void execute(BattleAction action, BattleContext context)
        {

        }

        public Type actionType
        {
            get { return _actionType; }
        }
    }
}
