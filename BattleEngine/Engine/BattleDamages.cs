using BattleEngine.Actors.Bullets;

namespace BattleEngine.Engine
{
    public class BattleDamages
    {
        private BattleContext _context;

        public BattleDamages(BattleContext context)
        {
            _context = context;
        }

        public void generateByBullet(BattleBullet bullet, int tick, int deltaTick)
        {
            var damage = bullet.generateDamage();
            _context.actors.damages.AddComponent(damage);
        }
    }
}
