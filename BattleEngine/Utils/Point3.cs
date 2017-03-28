namespace BattleEngine.Utils
{
    public class Point3 : Point
    {
        public double z;

        public void offset(double x, double y, double z)
        {
            this.x += x;
            this.y += y;
            this.z += z;
        }
    }
}
