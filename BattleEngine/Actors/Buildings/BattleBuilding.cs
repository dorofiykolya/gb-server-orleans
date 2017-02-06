using BattleEngine.Engine;
using BattleEngine.Modifiers;
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
            get { return _record.levels[_battleRecord.level]; }
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
            get { return infoLevel.unitId; }
        }

        public double oneUnitDefense
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.getBy(unitId, info.race).levels[level];
                var unitDefense = engine.players.getPlayer(ownerId)
                .modifier.calculate(ModifierType.UNITS_DEFENSE, unitRecord.defense, unitId);
                return unitDefense;
            }
        }

        public double oneUnitMagicDefense
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.getBy(unitId, info.race).levels[level];
                var unitDefense = engine.players.getPlayer(ownerId)
                    .modifier.calculate(ModifierType.UNITS_MAGIC_DEFENSE, unitRecord.magicDefense, unitId);
                return unitDefense;
            }
        }

        public double oneUnitHp
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.getBy(unitId, info.race).levels[level];
                var unitHp = engine.players.getPlayer(ownerId)
                    .modifier.calculate(ModifierType.UNITS_HP, unitRecord.hp, unitId);
                return unitHp;
            }
        }

        public double oneUnitDamage
        {
            get
            {
                var unitRecord = engine.configuration.unitRecords.getBy(unitId, info.race).levels[level];
                var damage = engine.players.getPlayer(ownerId)
                    .modifier.calculate(ModifierType.UNITS_DAMAGE, unitRecord.damage, unitId);
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
            _record = configuration.buildingRecords.getByBuildingId(info.id, info.race);
            _battleRecord = info;

            setOwnerId(info.ownerId);
            transform.setFromPoint(info.position);

            units.setCount(_battleRecord.units);

            var unitsPerSecond = infoLevel.unitsProduction;

            AddComponent<UnitRegenComponent>().setUnitsPerTick((unitsPerSecond / engine.configuration.ticksPerSecond));

            switch (_record.type)
            {
                case BuildingType.DEFENSE:

                    AddComponent(BuildingAttackDefenseComponent);
                    break;
                case BuildingType.MANNA:

                    AddComponent(MannaRegenComponent);
                    break;
                case BuildingType.PRODUCTION:
                    break;
            }
        }

        public function changeOwner(ownerId:int):void
		{

            setOwnerId(ownerId);
    }

    public function receiveDamage(damage:Number):void
		{
			var unitDefense:Number = oneUnitDefense;
			var unitHP:Number = oneUnitHp;
			
			while (damage > 0 && units.count > 0)
			{
				var currentHp:Number = unitHP;
				damage -= unitDefense;
				currentHp -= damage;
				damage -= unitHP;
				if (currentHp <= 0)
				{
					units.remove(1);
				}
			}
		}
		
		public function get unitsPerSecond():Number
		{
			return infoLevel.unitsProduction;
		}
		
		public function get mannaPerSecond():Number
		{
			return infoLevel.mannaProduction;
		}
		
		public function get powerDamage():Number
		{
			return units.count* oneUnitDamage;
		}
		
		private function outputEvent():void
		{
			var evt:BuildingChangeUnitEvent = engine.output.enqueueByFactory(BuildingChangeUnitEvent) as BuildingChangeUnitEvent;
			evt.tick = engine.tick;
			evt.objectId = objectId;
			evt.units = units.count;
		}
	
	}
}
