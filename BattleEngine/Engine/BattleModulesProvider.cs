using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattleModulesProvider
    {
        private Vector<BattleModule> _modules;

        public BattleModulesProvider()
        {
            _modules = new Vector<BattleModule>();
        }

        public void add(BattleModule module)
        {
            _modules.push(module);
        }

        public Vector<BattleModule> modules
        {
            get { return _modules; }
        }
    }
}
