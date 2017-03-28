using BattleEngine.Engine;
using BattleEngine.Utils;
using Common.Composite;

namespace BattleEngine.Actors.Bullets
{
    public class BulletMoveComponent : BattleComponent
    {
        private BattleTransform _moveTo;

        public BulletMoveComponent()
        {

        }

        override public bool needRemove
        {
            get { return false; }
        }

        public double distancePerTick
        {
            get
            {
                var currentDistancePerTick = 5;
                return currentDistancePerTick;
            }
        }

        public bool moveCompleted
        {
            get { return target.transform.positionDistance(_moveTo) <= 0; }
        }

        public void moveTo(BattleTransform transform)
        {
            _moveTo = transform;
        }

        public bool updatePosition(int deltaTick)
        {
            var distance = distancePerTick * deltaTick;

            var fromX = target.transform.x;
            var fromY = target.transform.y;

            var toX = _moveTo.x;
            var toY = _moveTo.y;

            var totalDistance = Math2.distance(fromX, fromY, toX, toY);
            if (distance > totalDistance)
            {
                distance = totalDistance;
            }
            var ratio = totalDistance == 0 ? 1 : distance / totalDistance;

            var newX = Math2.interpolate(fromX, toX, ratio);
            var newY = Math2.interpolate(fromY, toY, ratio);

            target.transform.setPosition(newX, newY);

            return newX != fromX || newY != fromY;
        }
    }
}
