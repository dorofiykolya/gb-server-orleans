using BattleEngine.Engine;
using Common.Composite;

namespace BattleEngine.Actors
{
    public class BattleObject : Entity
    {
        private Engine.BattleEngine _engine;
        private BattleTransform _transform;
        private int _objectId;
        private int _ownerId;

        public BattleObject()
        {
            _transform = new BattleTransform(this);
        }

        public int ownerId
        {
            get { return _ownerId; }
        }

        public void setOwnerId(int value)
        {
            _ownerId = value;
        }

        public int objectId
        {
            get { return _objectId; }
        }

        public void setObjectId(int value)
        {
            _objectId = value;
        }

        public BattleTransform transform
        {
            get { return _transform; }
        }

        public Engine.BattleEngine engine
        {
            get { return Root as Engine.BattleEngine; }
        }
    }
}
