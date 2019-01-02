using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BlobSallad
{
    public class PointMass
    {
        private readonly Vector _cur;
        private readonly Vector _prev;
        private readonly Vector _force = new Vector(0.0, 0.0);
        private readonly Vector _result = new Vector(0.0, 0.0);
        private double _mass;
        private double _friction = 0.01;

        public PointMass(double cx, double cy, double mass)
        {
            this._cur = new Vector(cx, cy);
            this._prev = new Vector(cx, cy);
            this._mass = mass;
        }

        public double GetXPos()
        {
            return this._cur.GetX();
        }

        public double GetYPos()
        {
            return this._cur.GetY();
        }

        public Vector GetPos()
        {
            return this._cur;
        }

        public double GetXPrevPos()
        {
            return this._prev.GetX();
        }

        public double GetYPrevPos()
        {
            return this._prev.GetY();
        }

        public Vector GetPrevPos()
        {
            return this._prev;
        }

        public void AddXPos(double dx)
        {
            this._cur.AddX(dx);
        }

        public void AddYPos(double dy)
        {
            this._cur.AddY(dy);
        }

        public void SetForce(Vector force)
        {
            this._force.Set(force);
        }

        public void AddForce(Vector force)
        {
            this._force.Add(force);
        }

        public double GetMass()
        {
            return this._mass;
        }

        public void SetMass(double mass)
        {
            this._mass = mass;
        }

        public void Move(double dt)
        {
            double dtdt = dt * dt;

            double ax = this._force.GetX() / this._mass;
            double cx = this._cur.GetX();
            double px = this._prev.GetX();
            double tx = (2.0 - this._friction) * cx - (1.0 - this._friction) * px + ax * dtdt;
            this._prev.SetX(cx);
            this._cur.SetX(tx);

            double ay = this._force.GetY() / this._mass;
            double cy = this._cur.GetY();
            double py = this._prev.GetY();
            double ty = (2.0 - this._friction) * cy - (1.0 - this._friction) * py + ay * dtdt;
            this._prev.SetY(cy);
            this._cur.SetY(ty);
        }

        public double GetVelocity()
        {
            double cXpX = this._cur.GetX() - this._prev.GetX();
            double cYpY = this._cur.GetY() - this._prev.GetY();
            return cXpX * cXpX + cYpY * cYpY;
        }

        public void SetFriction(double friction)
        {
            this._friction = friction;
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            var circle = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 2.0,
            };

            Canvas.SetLeft(circle, (_cur.GetX() - 4) * scaleFactor);
            Canvas.SetTop(circle, (_cur.GetY() - 4) * scaleFactor);

            canvas.Children.Add(circle);
        }
    }
}