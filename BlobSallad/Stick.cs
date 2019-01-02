using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BlobSallad
{
    public class Stick
    {
        private readonly PointMass pointMassA;
        private readonly PointMass pointMassB;
        private double length;
        private double lengthSquared;

        public Stick(PointMass pointMassA, PointMass pointMassB)
        {
            this.pointMassA = pointMassA;
            this.pointMassB = pointMassB;
            this.length = pointMassDist(pointMassA, pointMassB);
            this.lengthSquared = this.length * this.length;
        }

        public double getLength()
        {
            return this.length;
        }

        public double getLengthSquared()
        {
            return this.lengthSquared;
        }

        public static double pointMassDist(PointMass pointMassA, PointMass pointMassB)
        {
            double aXbX = pointMassA.getXPos() - pointMassB.getXPos();
            double aYbY = pointMassA.getYPos() - pointMassB.getYPos();
            return Math.Sqrt(aXbX * aXbX + aYbY * aYbY);
        }

        public PointMass getPointMassA()
        {
            return this.pointMassA;
        }

        public PointMass getPointMassB()
        {
            return this.pointMassB;
        }

        public void scale(double scaleFactor)
        {
            this.length *= scaleFactor;
            this.lengthSquared = this.length * this.length;
        }

        public void sc(Environment env)
        {
            Vector pointMassAPos = this.pointMassA.getPos();
            Vector pointMassBPos = this.pointMassB.getPos();

            Vector delta = new Vector(pointMassBPos);
            delta.sub(pointMassAPos);
            double dotProd = delta.dotProd(delta);
            double scaleFactor = this.lengthSquared / (dotProd + this.lengthSquared) - 0.5;
            delta.scale(scaleFactor);
            pointMassAPos.sub(delta);
            pointMassBPos.add(delta);
        }

        public void setForce(Vector force)
        {
            this.pointMassA.setForce(force);
            this.pointMassB.setForce(force);
        }

        public void addForce(Vector force)
        {
            this.pointMassA.addForce(force);
            this.pointMassB.addForce(force);
        }

        public void move(double dt)
        {
            this.pointMassA.move(dt);
            this.pointMassB.move(dt);
        }

        public void draw(Canvas canvas, double scaleFactor)
        {
            pointMassA.draw(canvas, scaleFactor);
            pointMassB.draw(canvas, scaleFactor);

            var x1 = pointMassA.getXPos() * scaleFactor;
            var y1 = pointMassA.getYPos() * scaleFactor;
            var startPoint = new System.Windows.Point(x1, y1);
            var pathFigure = new PathFigure {StartPoint = startPoint};

            var x2 = pointMassB.getXPos() * scaleFactor;
            var y2 = pointMassB.getYPos() * scaleFactor;
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