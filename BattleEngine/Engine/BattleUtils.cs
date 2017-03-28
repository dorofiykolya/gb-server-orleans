using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattleUtils
    {
        public static double floor(double value)
        {
            return Math2.floor(value, 5);
        }

        public static double round(double value)
        {
            return Math2.round(value, 5);
        }
    }
}
