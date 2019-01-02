using System;

namespace BlobSallad
{
    public class Vector
    {
        private double _x;
        private double _y;

        public Vector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Vector(Vector that)
        {
            _x = that._x;
            _y = that._y;
        }

        public void AddX(double x)
        {
            _x += x;
        }

        public void AddY(double y)
        {
            _y += y;
        }

        public void Set(Vector that)
        {
            _x = that._x;
            _y = that._y;
        }

        public void Add(Vector that)
        {
            _x += that._x;
            _y += that._y;
        }

        public void Sub(Vector that)
        {
            _x -= that._x;
            _y -= that._y;
        }

        public double DotProd(Vector that)
        {
            return _x * that._x + _y * that._y;
        }

        public double Length()
        {
            return Math.Sqrt(_x * _x + _y * _y);
        }

        public void Scale(double scaleFactor)
        {
            _x *= scaleFactor;
            _y *= scaleFactor;
        }

        public override string ToString()
        {
            // return String.format("(X: %d, Y: %d)");
            return "(X: " + _x + ", Y: " + _y + ")";
        }

        public double GetX()
        {
            return _x;
        }

        public void SetX(double x)
        {
            _x = x;
        }

        public double GetY()
        {
            return _y;
        }

        public void SetY(double y)
        {
            _y = y;
        }
    }
}