namespace BattleEngine.Output
{
    public class BulletMoveEvent : OutputEvent
    {
        public int objectId;
        public double x;
        public double y;
        public double z;

        public BulletMoveEvent()
        {

        }
    }
}
