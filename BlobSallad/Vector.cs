using System;

namespace BlobSallad
{
    public class Vector
    {
        private double _x;
        private double _y;

        public Vector(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public Vector(Vector that)
        {
            this._x = that._x;
            this._y = that._y;
        }

        public void AddX(double x)
        {
            this._x += x;
        }

        public void AddY(double y)
        {
            this._y += y;
        }

        public void Set(Vector that)
        {
            this._x = that._x;
            this._y = that._y;
        }

        public void Add(Vector that)
        {
            this._x += that._x;
            this._y += that._y;
        }

        public void Sub(Vector that)
        {
            this._x -= that._x;
            this._y -= that._y;
        }

        public double DotProd(Vector that)
        {
            return this._x * that._x + this._y * that._y;
        }

        public double Length()
        {
            return Math.Sqrt(this._x * this._x + this._y * this._y);
        }

        public void Scale(double scaleFactor)
        {
            this._x *= scaleFactor;
            this._y *= scaleFactor;
        }

        public override string ToString()
        {
            // return String.format("(X: %d, Y: %d)");
            return "(X: " + this._x + ", Y: " + this._y + ")";
        }

        public double GetX()
        {
            return this._x;
        }

        public void SetX(double x)
        {
            this._x = x;
        }

        public double GetY()
        {
            return this._y;
        }

        public void SetY(double y)
        {
            this._y = y;
        }
    }
}