using System;
using BattleEngine.Actors.Buildings;
using BattleEngine.Modifiers;
using Records.Units;
using BattleEngine.Components.Units;

namespace BattleEngine.Actors.Units
{
    public class BattleUnit : BattleUnitOwner
    {
        private UnitMoveComponent _move;
        private UnitRecord _info;
        private UnitLevelRecord _infoLevel;
        private int _level;
        private double _hp;
        private double _unitHP;
        private bool _attachedToBuilding;

        public BattleUnit()
        {
            _move = AddComponent<UnitMoveComponent>();
        }

        public UnitRecord info
        {
            get { return _info; }
        }

        public UnitLevelRecord infoLevel
        {
            get { return _infoLevel; }
        }

        public void initialize(BattleBuilding from, BattleBuilding to, int unitCount)
        {
            _attachedToBuilding = false;

            setOwnerId(from.ownerId);
            _level = from.level;
            _info = engine.configuration.unitRecords.GetBy(from.info.Levels[from.level].UnitId, from.info.Race);
            _infoLevel = _info.Levels[_level];
            _move.moveTo(to.transform);
            transform.setFrom(from.transform);

            _unitHP = engine.players.getPlayer(from.ownerId).modifier.calculate(ModifierType.UNITS_HP, _infoLevel.Hp, _info.Id);
            _hp = _unitHP * unitCount;
            units.setCount(getUnits());
        }

        public double hp
        {
            get { return _hp; }
        }

        public bool isDied
        {
            get { return _hp <= 0; }
        }

        public void decreaseHp(double value)
        {
            _hp -= value;
        }

        public void die()
        {
            _hp = 0;
        }

        public void attachTo(BattleBuilding building)
        {
            building.units.add(units.count);
            _attachedToBuilding = true;
            _hp = 0;
            units.change(getUnits());
        }

        public void receiveDamage(double damage)
        {
            var unitDefense = oneUnitDefense;
            var unitHP = oneUnitHp;

            while (damage > 0 && units.count > 0)
            {
                var currentHP = unitHP;
                damage -= unitDefense;
                currentHP -= damage;
                damage -= unitHP;
                if (currentHP <= 0)
                {

                    decreaseHp(unitHP);
                }
                else
                {

                    decreaseHp(currentHP);
                }
            }
            units.change(getUnits());
        }

        public int level
        {
            get { return _level; }
        }

        public int getUnits()
        {
            return (int)Math.Ceiling(_hp / _unitHP);
        }

        public int unitId
        {
            get { return _info.Id; }
        }

        public double powerDamage
        {
            get { return units.count * damageOneUnit; }
        }

        public double damageOneUnit
        {
            get
            {
                var damage = engine.players.getPlayer(ownerId).modifier.calculate(ModifierType.UNITS_DAMAGE, infoLevel.Damage, info.Id);
                return damage;
            }
        }

        public double oneUnitDefense
        {
            get
            {
                var defense = GetComponent<UnitDefenseComponent>();
                if (defense != null)
                {
                    return defense.calculateDefense(_infoLevel.Defense);
                }
                return _infoLevel.Defense;
            }
        }
        public double oneUnitMagicDefense
        {
            get
            {
                var defense = GetComponent<UnitDefenseComponent>();
                if (defense != null)
                {
                    return defense.calculateMagicDefense(_infoLevel.MagicDefense);
                }
                return _infoLevel.MagicDefense;
            }
        }

        public double oneUnitHp
        {
            get { return _unitHP; }
        }

        public bool attachedToBuilding
        {
            get { return _attachedToBuilding; }
        }
    }
}
