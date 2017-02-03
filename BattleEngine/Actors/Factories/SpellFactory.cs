using System;
using BattleEngine.Actors.Spells;

namespace BattleEngine.Actors.Factories
{
    public class SpellFactory
    {
        private BattleObjectFactory _factory;
		private Engine.BattleEngine _battleEngine;
		
		public SpellFactory(BattleObjectFactory factory, Engine.BattleEngine battleEngine)
        {
            _battleEngine = battleEngine;
            _factory = factory;
        }

        public BattleSpell instantiate(Type type)
		{
			var result = _factory.instantiate(type) as BattleSpell;
			return result;
		}
}
}
