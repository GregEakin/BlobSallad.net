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
            this._left = x;
            this._right = x + w;
            this._top = y;
            this._bottom = y + h;
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
            return new Environment(this._left, this._top, w, this._bottom - this._top);
        }

        public Environment SetHeight(double h)
        {
            return new Environment(this._left, this._top, this._right - this._left, h);
        }

        public bool Collision(Vector curPos, Vector prePos)
        {
            double x = curPos.GetX();
            if (x < this._left)
            {
                curPos.SetX(this._left);
                return true;
            }

            if (x > this._right)
            {
                curPos.SetX(this._right);
                return true;
            }

            double y = curPos.GetY();
            if (y < this._top)
            {
                curPos.SetY(this._top);
                return true;
            }

            if (y > this._bottom)
            {
                curPos.SetY(this._bottom);
                return true;
            }

            return false;
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
        }
    }
}