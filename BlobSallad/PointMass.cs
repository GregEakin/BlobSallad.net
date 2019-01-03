// Found from https://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System.Windows;
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
            Prev = new Vector(cx, cy);
            Mass = mass;
        }

        public Vector Pos { get; }
        public double XPos => Pos.X;
        public double YPos => Pos.Y;

        public Vector Prev { get; }
        public double XPrevPos => Prev.X;
        public double YPrevPos => Prev.Y;

        public double Mass { get; set; }
        public double Velocity
        {
            get
            {
                var cXpX = Pos.X - Prev.X;
                var cYpY = Pos.Y - Prev.Y;
                return cXpX * cXpX + cYpY * cYpY;
            }
        }

        public double Friction { get; set; } = 0.01;

        public Vector Force
        {
            get => _force;
            set => _force.Set(value);
        }

        public void AddXPos(double dx)
        {
            Pos.AddX(dx);
        }

        public void AddYPos(double dy)
        {
            Pos.AddY(dy);
        }

        public void AddForce(Vector force)
        {
            Force.Add(force);
        }

        public void Move(double dt)
        {
            var dtdt = dt * dt;

            var ax = Force.X / Mass;
            var cx = Pos.X;
            var px = Prev.X;
            var tx = (2.0 - Friction) * cx - (1.0 - Friction) * px + ax * dtdt;
            Prev.X = cx;
            Pos.X = tx;

            var ay = Force.Y / Mass;
            var cy = Pos.Y;
            var py = Prev.Y;
            var ty = (2.0 - Friction) * cy - (1.0 - Friction) * py + ay * dtdt;
            Prev.Y = cy;
            Pos.Y = ty;
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            var radius = 4;
            var x = Pos.X * scaleFactor;
            var y = Pos.Y * scaleFactor;
            var circle = new EllipseGeometry(new Point(x, y), radius, radius);
            var path = new Path
            {
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 2.0,
                Data = circle,
            };
            canvas.Children.Add(path);
        }
    }
}