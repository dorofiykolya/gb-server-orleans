namespace BattleEngine.Output
{
    public class UnitAttackEvent : OutputEvent
    {
        public int from;
        public int to;
        public int count;
        public int unitLevel;
        public int unitId;
    }
}
