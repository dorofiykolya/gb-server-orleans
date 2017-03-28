using BattleEngine.Actors;
using BattleEngine.Actors.Damages;
using BattleEngine.Actors.Units;
using BattleEngine.Components.Units;
using BattleEngine.Engine;
using BattleEngine.Output;
using BattleEngine.Utils;

namespace BattleEngine.Modules
{
    public class BattleUnitsMoveModule : BattleModule
    {
        private Vector<BattleObject> _temp = new Vector<BattleObject>();

        public BattleUnitsMoveModule()
        {

        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            _temp.length = 0;
            context.actors.units.getActors(typeof(BattleUnit), _temp);
            foreach (BattleUnit unit in _temp)
            {
                var move = unit.GetComponent<UnitMoveComponent>();
                if (move != null)
                {
                    if (move.updatePosition(deltaTick))
                    {
                        var evt = context.output.enqueueByFactory<UnitMoveEvent>();
                        evt.tick = tick;
                        evt.objectId = unit.objectId;
                        evt.x = unit.transform.x;
                        evt.y = unit.transform.y;
                        evt.z = unit.transform.z;
                    }
                    if (move.moveCompleted)
                    {
                        var damage = context.actors.factory.damageFactory.instantiate<BattleDamageUnitToBuilding>();
                        damage.setEnemy(unit, move.targetBuilding);
                        context.actors.damages.AddComponent(damage);
                    }
                }
            }
        }
    }
}
