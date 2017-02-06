using BattleEngine.Actors;
using BattleEngine.Actors.Buildings;
using BattleEngine.Engine;
using BattleEngine.Utils;

namespace BattleEngine.Modules
{
    public class BattleUnitsRegenModule : BattleModule
    {

        private Vector<BattleObject> _temp = new Vector<BattleObject>();

        public BattleUnitsRegenModule()
        {

        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            _temp.length = 0;
            context.actors.buildings.getActors(typeof(BattleBuilding), _temp);
            foreach (var item in _temp)
            {
                var regenComponent = item.GetComponent<UnitRegenComponent>();
                if (regenComponent != null)
                {
                    if (regenComponent.regen(deltaTick))
                    {
                        var evt = context.output.enqueueByFactory<BuildingChangeUnitEvent>();
                        evt.objectId = item.objectId;
                        evt.units = regenComponent.units;
                        evt.tick = tick;
                    }
                }
            }
        }
    }
}
