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
            _cur = new Vector(cx, cy);
            _prev = new Vector(cx, cy);
            _mass = mass;
        }

        public double GetXPos()
        {
            return _cur.GetX();
        }

        public double GetYPos()
        {
            return _cur.GetY();
        }

        public Vector GetPos()
        {
            return _cur;
        }

        public double GetXPrevPos()
        {
            return _prev.GetX();
        }

        public double GetYPrevPos()
        {
            return _prev.GetY();
        }

        public Vector GetPrevPos()
        {
            return _prev;
        }

        public void AddXPos(double dx)
        {
            _cur.AddX(dx);
        }

        public void AddYPos(double dy)
        {
            _cur.AddY(dy);
        }

        public void SetForce(Vector force)
        {
            _force.Set(force);
        }

        public void AddForce(Vector force)
        {
            _force.Add(force);
        }

        public double GetMass()
        {
            return _mass;
        }

        public void SetMass(double mass)
        {
            _mass = mass;
        }

        public void Move(double dt)
        {
            var dtdt = dt * dt;

            var ax = _force.GetX() / _mass;
            var cx = _cur.GetX();
            var px = _prev.GetX();
            var tx = (2.0 - _friction) * cx - (1.0 - _friction) * px + ax * dtdt;
            _prev.SetX(cx);
            _cur.SetX(tx);

            var ay = _force.GetY() / _mass;
            var cy = _cur.GetY();
            var py = _prev.GetY();
            var ty = (2.0 - _friction) * cy - (1.0 - _friction) * py + ay * dtdt;
            _prev.SetY(cy);
            _cur.SetY(ty);
        }

        public double GetVelocity()
        {
            var cXpX = _cur.GetX() - _prev.GetX();
            var cYpY = _cur.GetY() - _prev.GetY();
            return cXpX * cXpX + cYpY * cYpY;
        }

        public void SetFriction(double friction)
        {
            _friction = friction;
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