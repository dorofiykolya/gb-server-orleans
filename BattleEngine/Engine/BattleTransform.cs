using BattleEngine.Actors;
using BattleEngine.Utils;

namespace BattleEngine.Engine
{
    public class BattleTransform
    {
        private BattleObject _battleObject;
        private double _x;
        private double _y;
        private double _z;

        public BattleTransform(BattleObject battleObject)
        {
            _battleObject = battleObject;
        }

        public BattleObject target
        {
            get { return _battleObject; }
        }

        public void setFrom(BattleTransform transform)
        {
            _x = transform._x;
            _y = transform._y;
            _z = transform._z;
        }

        public void copyFrom(BattleTransform transform)
        {
            _x = transform._x;
            _y = transform._y;
            _z = transform._z;
        }

        public double positionDistance(BattleTransform transform)
        {
            var fromX = _x;
            var fromY = _y;

            var toX = transform.x;
            var toY = transform.y;

            var distance = Math2.distance(fromX, fromY, toX, toY);
            return distance;
        }

        public double positionDistanceTo(double x, double y)
        {
            var fromX = _x;
            var fromY = _y;

            var toX = x;
            var toY = y;

            var distance = Math2.distance(fromX, fromY, toX, toY);
            return distance;
        }

        public double distance(BattleTransform transform)
        {
            var fromX = _x;
            var fromY = _y;
            var fromZ = _z;

            var toX = transform.x;
            var toY = transform.y;
            var toZ = transform.z;

            var distance = Math2.distance3(fromX, fromY, fromZ, toX, toY, toZ);
            return distance;
        }

        public double distanceTo(double x, double y, double z)
        {
            var fromX = _x;
            var fromY = _y;
            var fromZ = _z;

            var toX = x;
            var toY = y;
            var toZ = z;

            var distance = Math2.distance3(fromX, fromY, fromZ, toX, toY, toZ);
            return distance;
        }

        public void setPosition(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public void setFromPoint3(Point3 position)
        {
            _x = position.x;
            _y = position.y;
            _z = position.z;
        }

        public void setFromPoint(Point position)
        {
            _x = position.x;
            _y = position.y;
        }

        public double x
        {
            get { return _x; }
            set { _x = value; }
        }

        public double y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double z
        {
            get { return _z; }
            set { _z = value; }
        }
    }
}
