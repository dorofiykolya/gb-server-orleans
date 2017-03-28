using BattleEngine.Engine;
using BattleEngine.Modules;

namespace BattleEngine.Providers
{
    public class BattleModules : BattleModulesProvider
    {
        public BattleModules()
        {
            add(new BattleUpdateObjectModule());
            add(new BattleMannaModule());
            add(new BattleUnitsRegenModule());
            add(new BattleUnitsMoveModule());
            add(new BattleDamangeModule());
            add(new BattleBuildingAttackModule());
            add(new BattleBulletModule());
            add(new BattleUnitDieModule());
        }
    }
}
