using System.Windows.Controls;

namespace BlobSallad
{
    public class Environment
    {
        private readonly double _left;
        private readonly double _right;
        private readonly double _top;
        private readonly double _bottom;

        public Environment(double x, double y, double w, double h)
        {
            _left = x;
            _right = x + w;
            _top = y;
            _bottom = y + h;
        }

        public double GetLeft()
        {
            return _left;
        }

        public double GetRight()
        {
            return _right;
        }

        public double GetTop()
        {
            return _top;
        }

        public double GetBottom()
        {
            return _bottom;
        }

        public Environment SetWidth(double w)
        {
            return new Environment(_left, _top, w, _bottom - _top);
        }

        public Environment SetHeight(double h)
        {
            return new Environment(_left, _top, _right - _left, h);
        }

        public bool Collision(Vector curPos, Vector prePos)
        {
            var x = curPos.X;
            if (x < _left)
            {
                curPos.X = _left;
                return true;
            }

            if (x > _right)
            {
                curPos.X = _right;
                return true;
            }

            var y = curPos.Y;
            if (y < _top)
            {
                curPos.Y = _top;
                return true;
            }

            if (y > _bottom)
            {
                curPos.Y = _bottom;
                return true;
            }

            return false;
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
        }
    }
}