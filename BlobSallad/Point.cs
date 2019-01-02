namespace BlobSallad
{
    public class Point
    {
        private readonly double _x;
        private readonly double _y;

        public Point(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public double GetX()
        {
            return this._x;
        }

        public double GetY()
        {
            return this._y;
        }
    }
}