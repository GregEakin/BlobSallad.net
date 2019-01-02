using System.Windows.Controls;

namespace BlobSallad
{
    public class Environment
    {
        private readonly double left;
        private readonly double right;
        private readonly double top;
        private readonly double bottom;

        public Environment(double x, double y, double w, double h)
        {
            this.left = x;
            this.right = x + w;
            this.top = y;
            this.bottom = y + h;
        }

        public double getLeft()
        {
            return left;
        }

        public double getRight()
        {
            return right;
        }

        public double getTop()
        {
            return top;
        }

        public double getBottom()
        {
            return bottom;
        }

        public Environment setWidth(double w)
        {
            return new Environment(this.left, this.top, w, this.bottom - this.top);
        }

        public Environment setHeight(double h)
        {
            return new Environment(this.left, this.top, this.right - this.left, h);
        }

        public bool collision(Vector curPos, Vector prePos)
        {
            double x = curPos.getX();
            if (x < this.left)
            {
                curPos.setX(this.left);
                return true;
            }

            if (x > this.right)
            {
                curPos.setX(this.right);
                return true;
            }

            double y = curPos.getY();
            if (y < this.top)
            {
                curPos.setY(this.top);
                return true;
            }

            if (y > this.bottom)
            {
                curPos.setY(this.bottom);
                return true;
            }

            return false;
        }

        public void draw(Canvas canvas, double scaleFactor)
        {
        }
    }
}