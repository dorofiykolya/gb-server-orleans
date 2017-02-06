namespace BattleEngine.Actors
{
    public class BattleUnitOwner : BattleObject
    {
        private UnitsContainer _units;

        public BattleUnitOwner()
        {
            _units = new UnitsContainer(this);
        }

        public UnitsContainer units
        {
            get { return _units; }
        }
    }
}
