using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattleState
    {
        private BattleConfiguration _configuration;
        private int _tick;
        private bool _isFinished;
        private IntRandom _random;
        private int _prevTick;

        public BattleState(BattleConfiguration configuration)
        {
            _configuration = configuration;
            _random = new IntRandom(0);
            _tick = 0;
            _prevTick = 0;
        }

        public IntRandom random
        {
            get { return _random; }
        }

        internal int deltaTick
        {
            get { return _tick - _prevTick; }
        }

        internal void updateTick(int tick)
        {
            _prevTick = _tick;
            _tick = tick;
        }

        internal void finishBattle()
        {
            _isFinished = true;
        }

        public int tick
        {
            get { return _tick; }
        }

        public bool isFinished
        {
            get { return _isFinished; }
        }
    }
}
