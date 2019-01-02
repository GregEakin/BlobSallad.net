using System;

namespace BlobSallad
{
    public class Vector
    {
        private double x;
        private double y;

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(Vector that)
        {
            this.x = that.x;
            this.y = that.y;
        }

        public void addX(double x)
        {
            this.x += x;
        }

        public void addY(double y)
        {
            this.y += y;
        }

        public void set(Vector that)
        {
            this.x = that.x;
            this.y = that.y;
        }

        public void add(Vector that)
        {
            this.x += that.x;
            this.y += that.y;
        }

        public void sub(Vector that)
        {
            this.x -= that.x;
            this.y -= that.y;
        }

        public double dotProd(Vector that)
        {
            return this.x * that.x + this.y * that.y;
        }

        public double length()
        {
            return Math.Sqrt(this.x * this.x + this.y * this.y);
        }

        public void scale(double scaleFactor)
        {
            this.x = this.x * scaleFactor;
            this.y = this.y * scaleFactor;
        }

        public string toString()
        {
            // return String.format("(X: %d, Y: %d)");
            return "(X: " + this.x + ", Y: " + this.y + ")";
        }

        public double getX()
        {
            return this.x;
        }

        public void setX(double x)
        {
            this.x = x;
        }

        public double getY()
        {
            return this.y;
        }

        public void setY(double y)
        {
            this.y = y;
        }
    }
}