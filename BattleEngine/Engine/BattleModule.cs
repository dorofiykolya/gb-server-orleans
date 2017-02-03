using Common.Composite;

namespace BattleEngine.Engine
{
    public class BattleModule : Component
    {
        public BattleEngine engine
        {
            get { return Entity as BattleEngine; }
        }

        public virtual void preTick(BattleContext context, int tick, int deltaTick)
        {

        }

        public virtual void postTick(BattleContext context, int tick, int deltaTick)
        {

        }
    }
}
