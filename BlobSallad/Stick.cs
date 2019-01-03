using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BlobSallad
{
    public class Stick
    {
        public Stick(PointMass pointMassA, PointMass pointMassB)
        {
            PointMassA = pointMassA;
            PointMassB = pointMassB;
            Length = PointMassDist(pointMassA, pointMassB);
            LengthSquared = Length * Length;
        }

        public double Length { get; private set; }

        public double LengthSquared { get; private set; }

        public PointMass PointMassA { get; }

        public PointMass PointMassB { get; }

        public static double PointMassDist(PointMass pointMassA, PointMass pointMassB)
        {
            var aXbX = pointMassA.XPos - pointMassB.XPos;
            var aYbY = pointMassA.YPos - pointMassB.YPos;
            return Math.Sqrt(aXbX * aXbX + aYbY * aYbY);
        }

        public void Scale(double scaleFactor)
        {
            Length *= scaleFactor;
            LengthSquared = Length * Length;
        }

        public void Sc(Environment env)
        {
            var pointMassAPos = PointMassA.Pos;
            var pointMassBPos = PointMassB.Pos;

            var delta = new Vector(pointMassBPos);
            delta.Sub(pointMassAPos);
            var dotProd = delta.DotProd(delta);
            var scaleFactor = LengthSquared / (dotProd + LengthSquared) - 0.5;
            delta.Scale(scaleFactor);
            pointMassAPos.Sub(delta);
            pointMassBPos.Add(delta);
        }

        public void SetForce(Vector force)
        {
            PointMassA.Force = force;
            PointMassB.Force = force;
        }

        public void AddForce(Vector force)
        {
            PointMassA.AddForce(force);
            PointMassB.AddForce(force);
        }

        public void Move(double dt)
        {
            PointMassA.Move(dt);
            PointMassB.Move(dt);
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            PointMassA.Draw(canvas, scaleFactor);
            PointMassB.Draw(canvas, scaleFactor);

            var x1 = PointMassA.XPos * scaleFactor;
            var y1 = PointMassA.YPos * scaleFactor;
            var startPoint = new System.Windows.Point(x1, y1);
            var pathFigure = new PathFigure {StartPoint = startPoint};

            var x2 = PointMassB.XPos * scaleFactor;
            var y2 = PointMassB.YPos * scaleFactor;
            var point = new System.Windows.Point(x2, y2);
            var lineSegment1A = new LineSegment {Point = point};
            pathFigure.Segments.Add(lineSegment1A);

            var pathFigureCollection = new PathFigureCollection {pathFigure};
            var pathGeometry = new PathGeometry {Figures = pathFigureCollection};

            var path = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3.0,
                Data = pathGeometry,
                // RenderTransform = translateTransform
            };

            canvas.Children.Add(path);
        }
    }
}