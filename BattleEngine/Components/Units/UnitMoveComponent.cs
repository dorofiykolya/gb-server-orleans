using BattleEngine.Actors.Units;
using BattleEngine.Engine;
using BattleEngine.Utils;
using Common.Composite;

namespace BattleEngine.Components.Units
{
    public class UnitMoveComponent : BattleComponent
    {
        private Vector<Component> _temp = new Vector<Component>();
        private BattleTransform _moveTo;

        public UnitMoveComponent()
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
                double currentDistancePerTick = ((BattleUnit)target).info.Speed;

                _temp.length = 0;

                GetComponents(typeof(IMoveModifier), false, _temp);
                foreach (IMoveModifier item in _temp)
                {
                    currentDistancePerTick *= item.moveModifierPercent;
                }
                var player = engine.players.getPlayer(target.ownerId);

                if (player != null)
                {
                    currentDistancePerTick = player.modifier.calculate(Modifiers.ModifierType.UNITS_SPEED, currentDistancePerTick, ((BattleUnit)target).unitId);
                }

                return currentDistancePerTick;
            }
        }

        public BattleTransform targetBuilding
        {
            get { return _moveTo; }
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
            if (totalDistance == 0)
            {
                return false;
            }
            if (distance > totalDistance)
            {
                distance = totalDistance;
            }
            var ratio = distance / totalDistance;

            var newX = Math2.interpolate(fromX, toX, ratio);
            var newY = Math2.interpolate(fromY, toY, ratio);

            target.transform.setPosition(newX, newY);

            return newX != fromX || newY != fromY;
        }

    }
}
