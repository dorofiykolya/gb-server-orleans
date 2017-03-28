using BattleEngine.Actors;
using Common.Composite;

namespace BattleEngine.Engine
{
    public class BattleComponent : Component
    {
        private int _lifeTick;
        private int _tick;

        public BattleComponent()
        {

        }

        public BattleEngine engine
        {
            get { return target.engine; }
        }

        public void setLifeTicks(int ticks)
        {
            _lifeTick = ticks;
        }

        public int lifeTick
        {
            get { return _lifeTick; }
        }

        public virtual bool needRemove
        {
            get { return _lifeTick <= 0; }
        }

        public BattleObject target
        {
            get { return Entity as BattleObject; }
        }

        public virtual void update(int tick, int deltaTick)
        {
            _tick = tick;
            _lifeTick -= deltaTick;
        }
    }
}
