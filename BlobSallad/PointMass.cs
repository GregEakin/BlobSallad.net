using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BlobSallad
{
    public class PointMass
    {
        private readonly Vector _force = new Vector(0.0, 0.0);

        public PointMass(double cx, double cy, double mass)
        {
            Pos = new Vector(cx, cy);
            PrevPos = new Vector(cx, cy);
            Mass = mass;
        }

        public Vector Pos { get; }
        public double XPos => Pos.X;
        public double YPos => Pos.Y;

        public Vector PrevPos { get; }
        public double XPrevPos => PrevPos.X;
        public double YPrevPos => PrevPos.Y;

        public void AddXPos(double dx)
        {
            Pos.AddX(dx);
        }

        public void AddYPos(double dy)
        {
            Pos.AddY(dy);
        }

        public Vector Force
        {
            get => _force;
            set => _force.Set(value);
        }

        public void AddForce(Vector force)
        {
            _force.Add(force);
        }

        public double Mass { get; set; }

        public void Move(double dt)
        {
            var dtdt = dt * dt;

            var ax = _force.X / Mass;
            var cx = Pos.X;
            var px = PrevPos.X;
            var tx = (2.0 - Friction) * cx - (1.0 - Friction) * px + ax * dtdt;
            PrevPos.X = cx;
            Pos.X = tx;

            var ay = _force.Y / Mass;
            var cy = Pos.Y;
            var py = PrevPos.Y;
            var ty = (2.0 - Friction) * cy - (1.0 - Friction) * py + ay * dtdt;
            PrevPos.Y = cy;
            Pos.Y = ty;
        }

        public double Velocity
        {
            get
            {
                var cXpX = Pos.X - PrevPos.X;
                var cYpY = Pos.Y - PrevPos.Y;
                return cXpX * cXpX + cYpY * cYpY;
            }
        }

        public double Friction { get; set; } = 0.01;

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

            Canvas.SetLeft(circle, (Pos.X - 4) * scaleFactor);
            Canvas.SetTop(circle, (Pos.Y - 4) * scaleFactor);

            canvas.Children.Add(circle);
        }
    }
}