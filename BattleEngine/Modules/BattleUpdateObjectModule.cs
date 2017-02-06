using System;
using System.Linq;
using BattleEngine.Engine;
using BattleEngine.Utils;
using Common.Composite;

namespace BattleEngine.Modules
{
    public class BattleUpdateObjectModule : BattleModule
    {
        private Vector<BattleActorsGroup> _list = new Vector<BattleActorsGroup>();
        private Vector<Component> _temp = new Vector<Component>();

        public BattleUpdateObjectModule()
        {

        }

        override public void preTick(BattleContext context, int tick, int deltaTick)
        {
            base.preTick(context, tick, deltaTick);

            _temp.length = 0;
            foreach (ActorsGroup e in Enum.GetValues(typeof(ActorsGroup)).Cast<ActorsGroup>())
            {
                context.actors.group(e).GetComponentsInChildren(typeof(BattleComponent), true, true, _temp);

                foreach (var item in _temp)
                {
                    item.update(tick, deltaTick);
                    if (item.needRemove)
                    {
                        item.dispose();
                    }
                }
            }
        }
    }
}
