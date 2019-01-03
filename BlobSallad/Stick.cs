using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BlobSallad
{
    public class Stick
    {
        private readonly PointMass _pointMassA;
        private readonly PointMass _pointMassB;
        private double _length;
        private double _lengthSquared;

        public Stick(PointMass pointMassA, PointMass pointMassB)
        {
            _pointMassA = pointMassA;
            _pointMassB = pointMassB;
            _length = PointMassDist(pointMassA, pointMassB);
            _lengthSquared = _length * _length;
        }

        public double GetLength()
        {
            return _length;
        }

        public double GetLengthSquared()
        {
            return _lengthSquared;
        }

        public static double PointMassDist(PointMass pointMassA, PointMass pointMassB)
        {
            var aXbX = pointMassA.XPos - pointMassB.XPos;
            var aYbY = pointMassA.YPos - pointMassB.YPos;
            return Math.Sqrt(aXbX * aXbX + aYbY * aYbY);
        }

        public PointMass GetPointMassA()
        {
            return _pointMassA;
        }

        public PointMass GetPointMassB()
        {
            return _pointMassB;
        }

        public void Scale(double scaleFactor)
        {
            _length *= scaleFactor;
            _lengthSquared = _length * _length;
        }

        public void Sc(Environment env)
        {
            var pointMassAPos = _pointMassA.Pos;
            var pointMassBPos = _pointMassB.Pos;

            var delta = new Vector(pointMassBPos);
            delta.Sub(pointMassAPos);
            var dotProd = delta.DotProd(delta);
            var scaleFactor = _lengthSquared / (dotProd + _lengthSquared) - 0.5;
            delta.Scale(scaleFactor);
            pointMassAPos.Sub(delta);
            pointMassBPos.Add(delta);
        }

        public void SetForce(Vector force)
        {
            _pointMassA.Force = force;
            _pointMassB.Force = force;
        }

        public void AddForce(Vector force)
        {
            _pointMassA.AddForce(force);
            _pointMassB.AddForce(force);
        }

        public void Move(double dt)
        {
            _pointMassA.Move(dt);
            _pointMassB.Move(dt);
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            _pointMassA.Draw(canvas, scaleFactor);
            _pointMassB.Draw(canvas, scaleFactor);

            var x1 = _pointMassA.XPos * scaleFactor;
            var y1 = _pointMassA.YPos * scaleFactor;
            var startPoint = new System.Windows.Point(x1, y1);
            var pathFigure = new PathFigure {StartPoint = startPoint};

            var x2 = _pointMassB.XPos * scaleFactor;
            var y2 = _pointMassB.YPos * scaleFactor;
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