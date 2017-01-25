using System;

namespace BattleEngine.Engine
{
    public class BattleAction : IComparable
    {
        public int tick;

        public int CompareTo(object value)
        {
            if (tick > ((BattleAction)value).tick) return 1;
            if (tick < ((BattleAction)value).tick) return -1;
            return 0;
        }
    }
}
