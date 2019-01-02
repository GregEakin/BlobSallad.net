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
            this._pointMassA = pointMassA;
            this._pointMassB = pointMassB;
            this._length = PointMassDist(pointMassA, pointMassB);
            this._lengthSquared = this._length * this._length;
        }

        public double GetLength()
        {
            return this._length;
        }

        public double GetLengthSquared()
        {
            return this._lengthSquared;
        }

        public static double PointMassDist(PointMass pointMassA, PointMass pointMassB)
        {
            double aXbX = pointMassA.GetXPos() - pointMassB.GetXPos();
            double aYbY = pointMassA.GetYPos() - pointMassB.GetYPos();
            return Math.Sqrt(aXbX * aXbX + aYbY * aYbY);
        }

        public PointMass GetPointMassA()
        {
            return this._pointMassA;
        }

        public PointMass GetPointMassB()
        {
            return this._pointMassB;
        }

        public void Scale(double scaleFactor)
        {
            this._length *= scaleFactor;
            this._lengthSquared = this._length * this._length;
        }

        public void Sc(Environment env)
        {
            Vector pointMassAPos = this._pointMassA.GetPos();
            Vector pointMassBPos = this._pointMassB.GetPos();

            Vector delta = new Vector(pointMassBPos);
            delta.Sub(pointMassAPos);
            double dotProd = delta.DotProd(delta);
            double scaleFactor = this._lengthSquared / (dotProd + this._lengthSquared) - 0.5;
            delta.Scale(scaleFactor);
            pointMassAPos.Sub(delta);
            pointMassBPos.Add(delta);
        }

        public void SetForce(Vector force)
        {
            this._pointMassA.SetForce(force);
            this._pointMassB.SetForce(force);
        }

        public void AddForce(Vector force)
        {
            this._pointMassA.AddForce(force);
            this._pointMassB.AddForce(force);
        }

        public void Move(double dt)
        {
            this._pointMassA.Move(dt);
            this._pointMassB.Move(dt);
        }

        public void Draw(Canvas canvas, double scaleFactor)
        {
            _pointMassA.Draw(canvas, scaleFactor);
            _pointMassB.Draw(canvas, scaleFactor);

            var x1 = _pointMassA.GetXPos() * scaleFactor;
            var y1 = _pointMassA.GetYPos() * scaleFactor;
            var startPoint = new System.Windows.Point(x1, y1);
            var pathFigure = new PathFigure {StartPoint = startPoint};

            var x2 = _pointMassB.GetXPos() * scaleFactor;
            var y2 = _pointMassB.GetYPos() * scaleFactor;
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