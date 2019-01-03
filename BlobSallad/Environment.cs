using System.Windows.Controls;

namespace BlobSallad
{
    public class Environment
    {
        public Environment(double x, double y, double w, double h)
        {
            Left = x;
            Right = x + w;
            Top = y;
            Bottom = y + h;
        }

        public double Left { get; }

        public double Right { get; }

        public double Top { get; }

        public double Bottom { get; }

        public Environment SetWidth(double w)
        {
            return new Environment(Left, Top, w, Bottom - Top);
        }

        public Environment SetHeight(double h)
        {
            return new Environment(Left, Top, Right - Left, h);
        }

        public bool Collision(Vector curPos, Vector prePos)
        {
            var x = curPos.X;
            if (x < Left)
            {
                curPos.X = Left;
                return true;
            }

            if (x > Right)
            {
                curPos.X = Right;
                return true;
            }

            var y = curPos.Y;
            if (y < Top)
            {
                curPos.Y = Top;
                return true;
            }

            if (y > Bottom)
            {
                curPos.Y = Bottom;
                return true;
            }

            return false;
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
        }
    }
}