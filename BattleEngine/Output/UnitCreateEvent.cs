namespace BattleEngine.Output
{
    public class UnitCreateEvent : OutputEvent
    {
        public int objectId;
        public int unitId;
        public int level;
        public int ownerId;
        public double x;
        public double y;
        public int units;
        public int toObjectId;
    }
}
