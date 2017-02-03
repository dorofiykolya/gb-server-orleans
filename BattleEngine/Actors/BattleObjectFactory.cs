using System;
using BattleEngine.Actors.Factories;
using BattleEngine.Utils;

namespace BattleEngine.Actors
{
    public class BattleObjectFactory
    {
        private int _idIndex;
        private Vector<BattleObject> _map;
        private BuildingFactory _buildingFactory;
        private UnitFactory _unitFactory;
        private BulletFactory _bulletFactory;
        private SpellFactory _spellFactory;
        private DamageFactory _damageFactory;

        public BattleObjectFactory(Vector<BattleObject> map, Engine.BattleEngine battleEngine)
        {
            _map = map;
            _buildingFactory = new BuildingFactory(this, battleEngine);
            _unitFactory = new UnitFactory(this, battleEngine);
            _bulletFactory = new BulletFactory(this, battleEngine);
            _spellFactory = new SpellFactory(this, battleEngine);
            _damageFactory = new DamageFactory(this, battleEngine);
        }

        public BuildingFactory buildingFactory
        {
            get { return _buildingFactory; }
        }

        public UnitFactory unitFactory
        {
            get { return _unitFactory; }
        }

        public BulletFactory bulletFactory
        {
            get { return _bulletFactory; }
        }

        public SpellFactory spellFactory
        {
            get { return _spellFactory; }
        }

        public DamageFactory damageFactory
        {
            get { return _damageFactory; }
        }

        public BattleObject instantiate(Type type)
        {
            var result = Activator.CreateInstance(type) as BattleObject;
            result.setObjectId(++_idIndex);
            _map[_idIndex] = result;

            return result;
        }

        public T instantiate<T>() where T : BattleObject
        {
            return (T)instantiate(typeof(T));
        }

    }
}
