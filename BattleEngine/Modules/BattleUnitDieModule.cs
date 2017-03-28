using BattleEngine.Actors;
using BattleEngine.Actors.Units;
using BattleEngine.Engine;
using BattleEngine.Output;
using BattleEngine.Utils;

namespace BattleEngine.Modules
{
    public class BattleUnitDieModule : BattleModule
    {
        private Vector<BattleObject> _temp = new Vector<BattleObject>();

        public BattleUnitDieModule()
        {

        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            _temp.length = 0;
            context.actors.units.getActors(typeof(BattleUnit), _temp);

            foreach (BattleUnit unit in _temp)
            {
                if (unit.isDied)
                {
                    var evt = context.output.enqueueByFactory<UnitDieEvent>();
                    evt.tick = tick;
                    evt.objectId = unit.objectId;

                    var removeEvt = context.output.enqueueByFactory<UnitRemoveEvent>();
                    removeEvt.tick = tick;
                    removeEvt.objectId = unit.objectId;

                    unit.Dispose();
                }
            }
        }
    }
}
