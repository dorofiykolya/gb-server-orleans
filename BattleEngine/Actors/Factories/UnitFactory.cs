using System;
using BattleEngine.Actors.Units;

namespace BattleEngine.Actors.Factories
{
    public class UnitFactory
    {
        private BattleObjectFactory _factory;

        public UnitFactory(BattleObjectFactory factory, Engine.BattleEngine battleEngine)
        {
            _factory = factory;
        }

        public BattleUnit instantiate(Type type)
        {
            var result = _factory.instantiate(type) as BattleUnit;
            return result;
        }

        public T instantiate<T>() where T : BattleUnitOwner
        {
            var result = _factory.instantiate(typeof(T)) as T;
            return result;
        }
    }
}
