using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    class BattleModulesProcessor
    {
        private BattleContext _context;
        private Vector<BattleModule> _modules;

        public BattleModulesProcessor(BattleContext context, BattleModulesProvider modulesProvider)
        {
            _context = context;
            _modules = new Vector<BattleModule>();

            foreach (var module in modulesProvider.modules)

            {
                _modules.push(module);
                _context.battleEngine.AddComponent(module);
            }
        }

        public void preTick(BattleContext context, int tick, int deltaTick)
        {
            foreach (var item in _modules)
            {
                item.preTick(context, tick, deltaTick);
            }
        }

        public void postTick(BattleContext context, int tick, int deltaTick)
        {
            foreach (var item in _modules)
            {
                item.postTick(context, tick, deltaTick);
            }
        }
    }
}
