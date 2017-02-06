using System;
using Records;

namespace BattleEngine.Utils
{
    public class Math2
    {
        /**
		 * 
		 * @param	value
		 * @param	decimals 0-7
		 * @return
		 */
        public static double round(double value, int decimals = 0)
        {
            if (decimals <= 0)
            {
                return Math.Round(value);
            }
            decimals = (int)clamp(decimals, 1, 7);
            return Math.Round(value * (decimals * 10)) / (decimals * 10);
        }

        /**
         * 
         * @param	value
         * @param	decimals 0-7
         * @return
         */
        public static double floor(double value, int decimals = 0)
        {
            if (decimals <= 0)
            {
                return (int)(value);
            }
            return (int)(value * (decimals * 10)) / (decimals * 10);
        }

        public static double clamp(double input, double left, double right)
        {
            if (input < left) return left;
            if (input > right) return right;
            return input;
        }

        public static double interpolate(double left, double right, double amount)
        {
            return left * (1 - amount) + right * amount;
        }

        public static double radiansPoint(Point from, Point to)
        {
            return Math.Atan2(-(to.y - from.y), (to.x - from.x));
        }

        public static double degreesPoint(PointRecord from, PointRecord to)
        {
            return degrees(from.x, from.y, to.x, to.y);
        }

        public static double degrees(double fromx, double fromy, double tox, double toy)
        {
            var result = rad2deg(Math.Atan2(-(toy - fromy), (tox - fromx)));
            if (result < 0.0)
            {
                result = 360.0 - (-result);
            }
            else if (result > 360.0)
            {
                var count = result / 360.0;
                result = result - ((int)(count) * 360.0);
            }
            return result;
        }

        public static double fixDegrees(double deg)
        {
            var result = deg;
            if (result < 0.0)
            {
                result = 360.0 - (-result);
            }
            else if (result > 360.0)
            {
                var count = result / 360.0;
                result = result - ((int)(count) * 360.0);
            }
            if ((int)result == 360)
            {
                result = 0;
            }
            return result;
        }

        public static double deg2rad(double deg)
        {
            return deg / 180.0 * Math.PI;
        }

        public static double rad2deg(double rad)
        {
            return rad / Math.PI * 180.0;
        }

        public static Point polarDeg(double len, double angleDeg, Point result = null)
        {
            if (result == null)
            {
                result = new Point();
            }
            result.x = Math.Round(len * Math.Cos(angleDeg * Math.PI / 180));
            result.y = Math.Round(len * Math.Sin(angleDeg * Math.PI / 180));

            return result;
        }

        public static Point polarRad(double len, double angleRad, Point result = null)
        {
            if (result == null)
            {
                result = new Point();
            }

            result.x = Math.Round(len * Math.Cos(angleRad));
            result.y = Math.Round(len * Math.Sin(angleRad));

            return result;
        }

        public static Point polarFromDeg(Point startPoint, double len, double angleDeg, Point result = null)
        {
            var x = startPoint.x;
            var y = startPoint.y;
            result = polarDeg(len, angleDeg, result);
            result.offset(x, y);
            return result;
        }

        public static Point polarFromRad(Point startPoint, double len, double angleRad, Point result = null)
        {
            var x = startPoint.x;
            var y = startPoint.y;
            result = polarRad(len, angleRad, result);
            result.offset(x, y);
            return result;
        }

        public static double distance(double fromX, double fromY, double toX, double toY)
        {
            var x = toX - fromX;
            var y = toY - fromY;
            if (x < 0) x = -x;
            if (y < 0) y = -y;
            return Math.Sqrt(x * x + y * y);
        }

        public static double distance3(double fromX, double fromY, double fromZ, double toX, double toY, double toZ)
        {
            var x = toX - fromX;
            var y = toY - fromY;
            var z = toZ - fromZ;
            if (x < 0) x = -x;
            if (y < 0) y = -y;
            if (z < 0) z = -z;
            return Math.Sqrt(x * x + y * y + z * z);
        }
    }
}
