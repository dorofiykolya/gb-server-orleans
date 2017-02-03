using BattleEngine.Modifiers;
using Common.Composite;

namespace BattleEngine.Players
{
    public class PlayerModifier : Component
    {
        public double calculate(ModifierType type, double value, int id = 0)
		{
			return value;
		}
}
}
