using BattleEngine.Actors.Damages;
using BattleEngine.Engine;
using BattleEngine.Utils;

namespace BattleEngine.Modules
{
    public class BattleDamangeModule : BattleModule
    {

        private Vector<Component> _temp = new Vector<Component>();
        private Vector<ApplyDamageResult> _damages = new Vector<ApplyDamageResult>();

        public BattleDamangeModule()
        {

        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            _temp.length = 0;
            var actors = context.actors.damages;
            actors.GetComponents(typeof(BattleDamage), false, _temp);

            foreach (var damage in _temp)
            {
                damage.update(tick, deltaTick);
                if (damage.needApplyDamage)
                {
                    _damages.length = 0;
                    damage.applyDamage(tick, _damages);
                    foreach (var damageResult in _damages)
                    {
                        var evt = context.output.enqueueByFactory<DamageApplyEvent>();
                        evt.tick = tick;
                        evt.x = damageResult.x;
                        evt.y = damageResult.y;
                        evt.z = damageResult.z;
                        evt.targetId = damageResult.targetId;
                        evt.units = damageResult.units;
                        evt.damageId = damageResult.damageObjectId;

                        /*var unitsEvt:UnitsChangeEvent = context.output.enqueueByFactory(UnitsChangeEvent) as UnitsChangeEvent;
                        unitsEvt.tick = tick;
                        unitsEvt.objectId = damageResult.targetId;
                        unitsEvt.units = damageResult.units;*/
                    }
                }
                if (damage.needRemove)
                {
                    damage.dispose();
                }
            }
        }
    }
}
