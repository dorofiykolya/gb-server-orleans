using BattleEngine.Actors;
using BattleEngine.Actors.Bullets;
using BattleEngine.Engine;
using BattleEngine.Output;
using BattleEngine.Utils;

namespace BattleEngine.Modules
{
    public class BattleBulletModule : BattleModule
    {
        private Vector<BattleObject> _temp = new Vector<BattleObject>();

        public BattleBulletModule()
        {

        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            base.preTick(context, tick, deltaTick);

            _temp.length = 0;
            context.actors.bullets.getActors(typeof(BattleBullet), _temp);

            foreach (BattleBullet bullet in _temp)
            {
                bullet.update(tick, deltaTick);

                if (bullet.needGenerateDamage)
                {
                    generateDamage(bullet, context, tick, deltaTick);
                }
                if (bullet.needRemove)
                {
                    var evt = context.output.enqueueByFactory<BulletRemoveEvent>();
                    evt.objectId = bullet.objectId;
                    evt.tick = tick;

                    bullet.Dispose();
                }
            }
        }

        private void generateDamage(BattleBullet bullet, BattleContext context, int tick, int deltaTick)
        {
            context.actors.damagesFactory.generateByBullet(bullet, tick, deltaTick);
        }

    }
}
