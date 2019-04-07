// Found from htt0ps://blobsallad.se/
// Originally Written by: bjoern.lindberg@gmail.com
// Translated to C# by Greg Eakin

using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BlobSallad
{
    public class Skin : Connection
    {
        public Skin(PointMass pointMassA, PointMass pointMassB)
            : base(pointMassA, pointMassB)
        {
            var aXbX = pointMassA.XPos - pointMassB.XPos;
            var aYbY = pointMassA.YPos - pointMassB.YPos;
            LengthSquared = aXbX * aXbX + aYbY * aYbY;
            Length = Math.Sqrt(LengthSquared);
        }

        public double Length { get; private set; }

        public double LengthSquared { get; private set; }

        public override void Scale(double scaleFactor)
        {
            Length *= scaleFactor;
            LengthSquared = Length * Length;
        }

        public override void Sc(Environment env)
        {
            var delta = PointMassB.Pos - PointMassA.Pos;
            var dotProd = delta.DotProd(delta);
            var scaleFactor = LengthSquared / (dotProd + LengthSquared) - 0.5;
            delta.Scale(scaleFactor);
            PointMassA.Pos.Sub(delta);
            PointMassB.Pos.Add(delta);
        }

        public override void Draw(Canvas canvas, double scaleFactor)
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