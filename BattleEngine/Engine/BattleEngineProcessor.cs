using System;
using System.Collections.Generic;

namespace BattleEngine.Engine
{
    class BattleEngineProcessor
    {
        private BattleConfiguration _configuration;
        private Dictionary<Type, BattleEngineCommand> _commandMap;

        public BattleEngineProcessor(BattleConfiguration configuration, BattleCommandsProvider commandsProvider)
        {
            _configuration = configuration;
            _commandMap = new Dictionary<Type, BattleEngineCommand>();

            foreach (var item in commandsProvider.commands)
            {
                map(item.actionType, item);
            }
        }

        public void map(Type type, BattleEngineCommand command)
        {
            _commandMap[type] = command;
        }

        public void execute(BattleAction action, BattleContext context)
        {
            var type = action.GetType();
            var command = _commandMap[type];
            if (command != null)
            {
                command.execute(action, context);
            }
        }
    }
}
