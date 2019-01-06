// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

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

        public double Right { get; set; }

        public double Top { get; }

        public double Bottom { get; set; }

        public double Width
        {
            set => Right = Left + value;
        }

        public double Height
        {
            set => Bottom = Top + value;
        }

        public bool Collision(Vector curPos, Vector prePos)
        {
            if (curPos.X < Left)
            {
                curPos.X = Left;
                return true;
            }

            if (curPos.X > Right)
            {
                curPos.X = Right;
                return true;
            }

            if (curPos.Y < Top)
            {
                curPos.Y = Top;
                return true;
            }

            if (curPos.Y > Bottom)
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