using System;
using BattleEngine.Actors.Damages;

namespace BattleEngine.Actors.Factories
{
    public class DamageFactory
    {
        private BattleObjectFactory _factory;
        private Engine.BattleEngine _battleEngine;

        public DamageFactory(BattleObjectFactory factory, Engine.BattleEngine battleEngine)
        {
            _battleEngine = battleEngine;
            _factory = factory;
        }

        public BattleDamage instantiate(Type type)
        {
            var result = _factory.instantiate(type) as BattleDamage;
            return result;
        }

    }
}
