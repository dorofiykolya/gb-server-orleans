namespace BattleEngine.Utils
{
    public class Point
    {
        public double x;
        public double y;

        public void offset(double x, double y)
        {
            this.x += x;
            this.y += y;
        }
    }
}
