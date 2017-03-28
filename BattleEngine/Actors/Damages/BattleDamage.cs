using BattleEngine.Utils;

namespace BattleEngine.Actors.Damages
{
    public class BattleDamage : BattleObject
    {
        private int _tick;
        private int _deltaTick;

        public BattleDamage()
        {

        }

        public int tick
        {
            get { return _tick; }
        }

        public virtual void update(int tick, int deltaTick)
        {
            _tick = tick;
            _deltaTick = deltaTick;
        }

        public virtual Vector<ApplyDamageResult> applyDamage(int tick, Vector<ApplyDamageResult> result = null)
        {
            return result;
        }

        public virtual bool needApplyDamage
        {
            get { return false; }
        }

        public virtual bool needRemove
        {
            get { return true; }
        }

        public int deltaTick
        {
            get
            {
                return _deltaTick;
            }
        }
    }
}
