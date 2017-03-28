using BattleEngine.Components.Buildings;
using BattleEngine.Engine;
using BattleEngine.Modifiers;
using BattleEngine.Output;
using BattleEngine.Records;
using Records.Buildings;

namespace BattleEngine.Actors.Buildings
{
    public class BattleBuilding : BattleUnitOwner
    {
        private BuildingRecord _record;
        private BattleBuildingRecord _battleRecord;

        public BattleBuilding()
        {

        }

        public BattleBuildingRecord battleInfo
        {
            get { return _battleRecord; }
        }

        public BuildingRecord info
        {
            get { return _record; }
        }

        public BuildingLevelRecord infoLevel
        {
            get { return _record.Levels[_battleRecord.level]; }
        }

        public int level
        {
            get { return _battleRecord.level; }
        }

        public double getOneUnitDefense(DefenseType defenseType)
        {
            if (defenseType == DefenseType.MAGIC_DEFENSE)
            {
                return oneUnitMagicDefense;
            }
            return oneUnitDefense;
        }

        public int unitId
        {
            get { return infoLevel.UnitId; }
        }

        public double oneUnitDefense
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.GetBy(unitId, info.Race).Levels[level];
                var unitDefense = engine.players.getPlayer(ownerId)
                .modifier.calculate(ModifierType.UNITS_DEFENSE, unitRecord.Defense, unitId);
                return unitDefense;
            }
        }

        public double oneUnitMagicDefense
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.GetBy(unitId, info.Race).Levels[level];
                var unitDefense = engine.players.getPlayer(ownerId)
                    .modifier.calculate(ModifierType.UNITS_MAGIC_DEFENSE, unitRecord.MagicDefense, unitId);
                return unitDefense;
            }
        }

        public double oneUnitHp
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.GetBy(unitId, info.Race).Levels[level];
                var unitHp = engine.players.getPlayer(ownerId)
                    .modifier.calculate(ModifierType.UNITS_HP, unitRecord.Hp, unitId);
                return unitHp;
            }
        }

        public double oneUnitDamage
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.GetBy(unitId, info.Race).Levels[level];
                var damage = engine.players.getPlayer(ownerId)
                    .modifier.calculate(ModifierType.UNITS_DAMAGE, unitRecord.Damage, unitId);
                return damage;
            }
        }

        public double hp
        {
            get { return oneUnitHp * units.count; }
        }

        public double powerDefense
        {
            get { return (oneUnitDefense + oneUnitHp) * units.count; }
        }

        public double powerMagicDefense
        {
            get { return (oneUnitMagicDefense + oneUnitHp) * units.count; }
        }

        public void initialize(BattleBuildingRecord info, BattleConfiguration configuration)
        {
            _record = configuration.buildingRecords.GetByBuildingId(info.id, info.race);
            _battleRecord = info;

            setOwnerId(info.ownerId);
            transform.setFromPoint(info.position);

            units.setCount(_battleRecord.units);

            var unitsPerSecond = infoLevel.UnitsProduction;

            AddComponent<UnitRegenComponent>().setUnitsPerTick((unitsPerSecond / engine.configuration.ticksPerSecond));

            switch (_record.Type)
            {
                case BuildingType.DEFENSE:

                    AddComponent<BuildingAttackDefenseComponent>();
                    break;
                case BuildingType.MANNA:

                    AddComponent<MannaRegenComponent>();
                    break;
                case BuildingType.PRODUCTION:
                    break;
            }
        }

        public void changeOwner(int ownerId)
        {
            setOwnerId(ownerId);
        }

        public void receiveDamage(double damage)
        {
            var unitDefense = oneUnitDefense;
            var unitHP = oneUnitHp;

            while (damage > 0 && units.count > 0)
            {
                var currentHp = unitHP;
                damage -= unitDefense;
                currentHp -= damage;
                damage -= unitHP;
                if (currentHp <= 0)
                {
                    units.remove(1);
                }
            }
        }

        public double unitsPerSecond
        {
            get { return infoLevel.UnitsProduction; }
        }

        public double mannaPerSecond
        {
            get { return infoLevel.MannaProduction; }
        }

        public double powerDamage
        {
            get { return units.count * oneUnitDamage; }
        }

        private void outputEvent()
        {
            var evt = engine.output.enqueueByFactory<BuildingChangeUnitEvent>();
            evt.tick = engine.tick;
            evt.objectId = objectId;
            evt.units = units.count;
        }

    }
}
