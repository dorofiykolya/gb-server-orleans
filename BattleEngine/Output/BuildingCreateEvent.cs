namespace BattleEngine.Output
{
    public class BuildingCreateEvent : OutputEvent
    {
        public int objectId;
        public int buildingId;
        public int level;
        public int ownerId;
        public double x;
        public double y;
        public int units;

        public BuildingCreateEvent()
        {

        }
    }
}
