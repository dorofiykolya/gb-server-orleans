using BattleEngine.Actors.Damages;

namespace BattleEngine.Actors.Bullets
{
    public class BattleBullet : BattleObject
    {
        public virtual void update(int tick, int deltaTick)
        {

        }

        public virtual BattleDamage generateDamage()
        {
            return null;
        }

        public virtual bool needRemove
        {
            get { return false; }
        }

        public virtual bool needGenerateDamage
        {
            get { return false; }
        }
    }
}
