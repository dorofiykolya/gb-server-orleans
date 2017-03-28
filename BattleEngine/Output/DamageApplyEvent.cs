namespace BattleEngine.Output
{
    public class DamageApplyEvent : OutputEvent
    {
        public double x;
        public double y;
        public double z;
        public int targetId;
        public int units;
        public int damageId;

        public DamageApplyEvent()
        {

        }
    }
}
