namespace BattleEngine.Output
{
    public class BulletCreateEvent : OutputEvent
    {
        public int objectId;
        public int fromObjectId;
        public int toObjectId;

        public BulletCreateEvent()
        {

        }
    }
}
