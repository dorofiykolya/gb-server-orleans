using BattleEngine.Actors;
using BattleEngine.Actors.Buildings;
using BattleEngine.Actors.Units;
using BattleEngine.Engine;
using BattleEngine.Utils;

namespace BattleEngine.Modules
{
    public class BattleBuildingAttackModule : BattleModule
    {

        private Vector<BattleObject> _buildings = new Vector<BattleObject>();
        private Vector<BattleObject> _units = new Vector<BattleObject>();

        public BattleBuildingAttackModule()
        {

        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            _buildings.length = 0;
            context.actors.buildings.getActors(typeof(BattleBuilding), _buildings);

            _units.length = 0;
            context.actors.units.getActors(typeof(BattleUnit), _units);

            foreach (var building in _buildings)
            {
                var component = building.GetComponent<BuildingAttackDefenseComponent>();
                if (component && component.canAttack)
                {
                    var range = component.range;
                    BattleUnit target = null;
                    var minDistance = int.MaxValue;
                    foreach (var unit in _units)
                    {
                        if (unit.ownerId != building.ownerId)
                        {
                            var distance = unit.transform.positionDistance(building.transform);
                            if (distance <= range)
                            {
                                if (distance < minDistance)
                                {
                                    minDistance = distance;
                                    target = unit;
                                }
                            }
                        }
                    }
                    if (target != null)
                    {
                        component.attack(target);

                        var evt = context.output.enqueueByFactory<BuildingAttackEvent>();
                        evt.objectId = building.objectId;
                        evt.targetId = target.objectId;
                        evt.tick = tick;
                    }
                }
            }
        }

    }
}
