using System;
using BattleEngine.Actors.Bullets;

namespace BattleEngine.Actors.Factories
{
    public class BulletFactory
    {
        private BattleObjectFactory _factory;

        public BulletFactory(BattleObjectFactory factory, Engine.BattleEngine battleEngine)
        {
            _factory = factory;
        }

        public BattleBullet instantiate(Type type)
        {
            var result = _factory.instantiate(type) as BattleBullet;
            return result;
        }

        public T instantiate<T>() where T : BattleBullet
        {
            var result = _factory.instantiate(typeof(T)) as T;
            return result;
        }
    }
}
