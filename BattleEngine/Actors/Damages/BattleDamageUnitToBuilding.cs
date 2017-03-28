using BattleEngine.Actors.Buildings;
using BattleEngine.Actors.Units;
using BattleEngine.Engine;
using BattleEngine.Output;
using BattleEngine.Utils;

namespace BattleEngine.Actors.Damages
{
    public class BattleDamageUnitToBuilding : BattleDamage
    {
        private int _unitObjectId;
        private int _buildingObjectId;

        public BattleDamageUnitToBuilding()
        {

        }

        public void setEnemy(BattleUnit unit, BattleTransform targetBuilding)
        {
            _unitObjectId = unit.objectId;
            _buildingObjectId = targetBuilding.target.objectId;
        }

        public override void update(int tick, int deltaTick)
        {

        }

        public override Vector<ApplyDamageResult> applyDamage(int tick, Vector<ApplyDamageResult> result = null)
        {
            if (result == null) result = new Vector<ApplyDamageResult>();

            var unit = engine.context.actors.getActorByObjectId(_unitObjectId) as BattleUnit;
            if (unit == null)
            {
                return result;
            }

            var building = engine.context.actors.getActorByObjectId(_buildingObjectId) as BattleBuilding;
            if (building.ownerId == unit.ownerId)
            {
                attachUnits(building, unit);
                return result;
            }

            var damageResult = new ApplyDamageResult();

            var buildingDamage = building.powerDamage;
            var unitDamage = unit.powerDamage;

            building.receiveDamage(unitDamage);
            unit.receiveDamage(buildingDamage);

            if (unit.units.count > building.units.count)
            {
                building.changeOwner(unit.ownerId);
                building.units.change(unit.units.count);

                var evt = engine.output.enqueueByFactory<BuildingChangeOwnerEvent>(tick);
                evt.objectId = building.objectId;
                evt.ownerId = unit.ownerId;
                evt.units = unit.units.count;
            }

            damageResult.x = building.transform.x;
            damageResult.y = building.transform.y;
            damageResult.z = building.transform.z;
            damageResult.ownerId = building.ownerId;
            damageResult.targetId = building.objectId;
            damageResult.damageObjectId = objectId;
            damageResult.units = building.units.count;

            result.push(damageResult);

            return result;
        }

        private void attachUnits(BattleBuilding building, BattleUnit unit)
        {
            unit.attachTo(building);

            var evt = engine.output.enqueueByFactory<UnitAttachedEvent>() as UnitAttachedEvent;
            evt.tick = tick;
            evt.buildingObjectId = _buildingObjectId;
            evt.unitObjectId = unit.objectId;
            evt.buildingUnits = building.units.count;
        }

        public override bool needApplyDamage
        {
            get { return true; }
        }

        public override bool needRemove
        {
            get { return true; }
        }

    }
}
