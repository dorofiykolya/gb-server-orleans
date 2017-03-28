using BattleEngine.Engine;

namespace BattleEngine.Components.Units
{
    public class UnitDefenseComponent : BattleComponent
    {
        private int _defense;
        private int _mannaDefense;

        public UnitDefenseComponent()
        {
            _defense = 1;
        }

        public void setPercent(int defense, int mannaDefense)
        {
            _mannaDefense = mannaDefense;
            _defense = defense;
        }

        override public bool needRemove
        {
            get { return false; }
        }

        public double calculateDefense(double defense)
        {
            return defense + (defense * (_defense / 100.0));
        }

        public double calculateMagicDefense(double defense)
        {
            return defense + (defense * (_mannaDefense / 100.0));
        }
    }
}
