using BattleEngine.Actors.Damages;

namespace BattleEngine.Actors.Bullets
{
    public class BattleBullet : BattleObject
    {
        public void update(int tick, int deltaTick)
        {

        }

        public BattleDamage generateDamage()
        {
            return null;
        }

        public bool needRemove
        {
            get { return false; }
        }

        public bool needGenerateDamage
        {
            get { return false; }
        }
    }
}
