using BattleEngine.Actors.Buildings;
using BattleEngine.Actors.Units;
using BattleEngine.Engine;

namespace BattleEngine.Commands
{
    public class BattleUnitAttackCommand : BattleEngineCommand
    {
        public BattleUnitAttackCommand() : base(typeof(BattleUnitAttackAction))
        {

        }

        override public void execute(BattleAction action, BattleContext context)
        {
            var attack = (BattleUnitAttackAction)action;
            var from = context.actors.getActorByObjectId(attack.fromObjectId) as BattleBuilding;
            var to = context.actors.getActorByObjectId(attack.toObjectId) as BattleBuilding;

            var regen = from.GetComponent<UnitRegenComponent>();
            var unitCount = regen.removeHalf();

            if (unitCount > 0)
            {
                var unit = context.actors.factory.unitFactory.instantiate<BattleUnit>();
                context.actors.units.AddComponent(unit);

                unit.initialize(from, to, unitCount);

                var createUnit = context.output.enqueueByFactory<UnitCreateEvent>();
                createUnit.objectId = unit.objectId;
                createUnit.unitId = unit.unitId;
                createUnit.level = unit.level;
                createUnit.ownerId = unit.ownerId;
                createUnit.units = unit.units.count;
                createUnit.x = unit.transform.x;
                createUnit.y = unit.transform.y;
                createUnit.toObjectId = to.objectId;

                var evt = context.output.enqueueByFactory<UnitAttackEvent>();
                evt.count = unitCount;
                evt.unitId = unit.unitId;
                evt.unitLevel = unit.level;
                evt.from = attack.fromObjectId;
                evt.to = attack.toObjectId;
                evt.tick = action.tick;
            }
        }
    }
}
