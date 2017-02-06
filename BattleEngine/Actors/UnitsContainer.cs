using BattleEngine.Output;

namespace BattleEngine.Actors
{
    public class UnitsContainer
    {
        private int _count;
        private BattleObject _battleObject;

        public UnitsContainer(BattleObject battleObject)
        {
            _battleObject = battleObject;
        }

        public void setCount(int count)
        {
            _count = count;
        }

        public bool change(int count)
        {
            if (_count != count)
            {
                _count = count;

                outputEvent();
                return true;
            }
            return false;
        }

        public int add(int count)
        {
            if (count != 0)
            {
                _count += count;

                outputEvent();
            }
            return _count;
        }

        public int remove(int count)
        {
            if (_count != 0)
            {
                _count -= count;
                if (_count < 0) _count = 0;

                outputEvent();
            }
            return _count;
        }

        public int count
        {
            get { return _count; }
        }

        private void outputEvent()
        {
            var evt = _battleObject.engine.output.enqueueByFactory<UnitsChangeEvent>();
            evt.tick = _battleObject.engine.tick;
            evt.objectId = _battleObject.objectId;
            evt.units = _count;
        }
    }
}
