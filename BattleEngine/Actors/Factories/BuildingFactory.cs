using System;
using BattleEngine.Actors.Buildings;

namespace BattleEngine.Actors.Factories
{
    public class BuildingFactory
    {
        private BattleObjectFactory _factory;

        public BuildingFactory(BattleObjectFactory factory, Engine.BattleEngine battleEngine)
        {
            _factory = factory;
        }

        public BattleBuilding instantiate(Type type)
        {
            var result = _factory.instantiate(type) as BattleBuilding;
            return result;
        }
    }
}
