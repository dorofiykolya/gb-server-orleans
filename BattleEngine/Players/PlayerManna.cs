using Common.Composite;

namespace BattleEngine.Players
{
    public class PlayerManna : Component
    {
        private double _manna = 0;
        private double _maxManna = 100;

        public PlayerManna()
        {

        }

        public bool add(double value)
        {
            _manna += value;
            return true;
        }

        public bool remove(double value)
        {
            _manna -= value;
            return true;
        }

        public double max
        {
            get { return _maxManna; }
        }

        public double value
        {
            get { return _manna; }
        }
    }
}
