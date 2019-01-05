// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System;

namespace BlobSallad
{
    public class Vector
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Vector that)
        {
            X = that.X;
            Y = that.Y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public void AddX(double x)
        {
            X += x;
        }

        public void AddY(double y)
        {
            Y += y;
        }

        public void Set(Vector that)
        {
            X = that.X;
            Y = that.Y;
        }

        public void Add(Vector that)
        {
            X += that.X;
            Y += that.Y;
        }

        public void Sub(Vector that)
        {
            X -= that.X;
            Y -= that.Y;
        }

        public double DotProd(Vector that)
        {
            return X * that.X + Y * that.Y;
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public void Scale(double scaleFactor)
        {
            X *= scaleFactor;
            Y *= scaleFactor;
        }

        public override string ToString()
        {
            return $"(X: {X}, Y: {Y})";
        }

        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);
    }
}