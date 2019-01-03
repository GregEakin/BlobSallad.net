using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using NUnit.Framework;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Environment = BlobSallad.Environment;

namespace BlobSalladTests
{
    [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
    [Apartment(ApartmentState.STA)]
    public class BlobTests
    {
        [Test]
        public void CtorTest()
        {
            var blob = new Blob(71.0, 67.0, 11.0, 5);
            Assert.AreEqual(71.0, blob.XPos);
            Assert.AreEqual(67.0, blob.YPos);
            Assert.AreEqual(11.0, blob.Radius);

            var pointMas = blob.MiddlePointMass;
            Assert.AreEqual(71.0, pointMas.XPos);
            Assert.AreEqual(67.0, pointMas.YPos);
            Assert.AreEqual(1.0, pointMas.Mass);
        }

        [Test]
        public void CtorPointMassesTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            var pointMasses = blob.PointMasses;
            foreach (var pointMass in pointMasses)
                DrawDot(canvas, Brushes.Black, pointMass.Mass, pointMass.XPos, pointMass.YPos);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void CtorSticksTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            var sticks = blob.Sticks;
            foreach (var stick in sticks)
                stick.Draw(canvas, 1.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void CtorJointsTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            var middlePointMass = blob.MiddlePointMass;
            DrawDot(canvas, Brushes.Blue, middlePointMass.Mass, middlePointMass.XPos, middlePointMass.YPos);

            var joints = blob.Joints;
            foreach (var joint in joints)
            {
                var pointMassA = joint.PointMassA;
                var pointMassB = joint.PointMassB;
                DrawDot(canvas, Brushes.Red, pointMassA.Mass, pointMassA.XPos, pointMassA.YPos);
                DrawLine(canvas, Brushes.Black, 
                        pointMassA.XPos, pointMassA.YPos,
                        pointMassB.XPos, pointMassB.YPos);
            }

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        private void DrawDot(Canvas canvas, Brush brush, double radius, double x, double y)
        {
            var circle = new Ellipse
            {
                Width = 2.0 * radius,
                Height = 2.0 * radius,
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 1.0,
                // RenderTransform = translateTransform,
            };

            Canvas.SetLeft(circle, x - radius);
            Canvas.SetTop(circle, y - radius);

            canvas.Children.Add(circle);
        }

        private void DrawLine(Canvas canvas, Brush brush, double x1, double y1, double x2, double y2)
        {
            var startPoint = new System.Windows.Point(x1, y1);
            var pathFigure = new PathFigure { StartPoint = startPoint };

            var point = new System.Windows.Point(x2, y2);
            var lineSegment1A = new LineSegment { Point = point };
            pathFigure.Segments.Add(lineSegment1A);

            var pathFigureCollection = new PathFigureCollection { pathFigure };
            var pathGeometry = new PathGeometry { Figures = pathFigureCollection };

            var path = new Path
            {
                Stroke = brush,
                StrokeThickness = 1.0,
                Data = pathGeometry,
                // RenderTransform = translateTransform
            };

            canvas.Children.Add(path);
        }

        [Test]
        public void CtorPointMassTest()
        {
            var blob = new Blob(71.0, 67.0, 11.0, 5);
            for (var i = 0; i < 5; i++)
            {
                var pointMas = blob[i];

                var mass = i < 2 ? 4.0 : 1.0;
                Assert.AreEqual(mass, pointMas.Mass);

                var theta = (double) i * 2.0 * Math.PI / (double) 5;
                var cx = Math.Cos(theta) * 11.0 + 71.0;
                var cy = Math.Sin(theta) * 11.0 + 67.0;
                Assert.AreEqual(cx, pointMas.XPos);
                Assert.AreEqual(cy, pointMas.YPos);
            }
        }

        [Test]
        public void AddBlob2Test()
        {
            var blob1 = new Blob(17.0, 19.0, 11.0, 5);
            var blob2 = new Blob(59.0, 61.0, 13.0, 5);
            blob1.AddBlob(blob2);
            var joint = blob1.Joints[10];
            Assert.AreEqual(22.800, joint.ShortConst, 0.01);
            Assert.AreEqual(0.0, joint.LongConst, 0.01);
        }

        [Test]
        public void AddBlobTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob1 = new Blob(17.0, 19.0, 11.0, 5);
            var blob2 = new Blob(59.0, 61.0, 13.0, 5);
            blob1.AddBlob(blob2);

            var middlePointMass = blob1.MiddlePointMass;
            DrawDot(canvas, Brushes.Blue, middlePointMass.Mass, middlePointMass.XPos, middlePointMass.YPos);

            blob2.DrawSimpleBody(canvas, 1.0);

            var joints = blob1.Joints;
            foreach (var joint in joints)
            {
                var pointMassA = joint.PointMassA;
                var pointMassB = joint.PointMassB;
                DrawDot(canvas, Brushes.Red, 2.0, pointMassA.XPos, pointMassA.YPos);
                DrawLine(canvas, Brushes.Black, 
                        pointMassA.XPos, pointMassA.YPos,
                        pointMassB.XPos, pointMassB.YPos);
            }

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void ScaleTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var translateTransform = new TranslateTransform(41.0, 43.0);
            var blob = new Blob(41.0, 43.0, 23.0, 5);
            blob.Scale(3.0);
            blob.DrawSmile(canvas, 1.0, translateTransform);
            blob.DrawEyesOpen(canvas, 1.0, translateTransform);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void MoveTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            blob.Force = new Vector(3.0, 3.0);
            blob.Move(2.0);

            var middlePointMass = blob.MiddlePointMass;
            DrawDot(canvas, Brushes.Blue, 2.0, middlePointMass.XPos, middlePointMass.YPos);
            blob.DrawSimpleBody(canvas, 1.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void ScTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var environment = new Environment(0.0, 0.0, 100.0, 100.0);
            var blob = new Blob(71.0, 67.0, 23.0, 5);
            blob.Force = new Vector(3.0, 3.0);
            blob.Move(3.0);
            blob.Sc(environment);
            blob.DrawSimpleBody(canvas, 1.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void SetForceTest()
        {
            // TODO:
        }

        [Test]
        public void MoveToTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            blob.MoveTo(61.0, 59.0);
            blob.DrawSimpleBody(canvas, 1.0);

            var middlePointMass = blob.MiddlePointMass;
            DrawDot(canvas, Brushes.Blue, 2.0, middlePointMass.XPos, middlePointMass.XPos);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void AddForceTest()
        {
            // TODO:
        }

        [Test]
        public void DrawEyesOpenTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.DrawEyesOpen(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void DrawEyesClosedTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.DrawEyesClosed(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void DrawSmileTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.DrawSmile(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void DrawOpenMouthTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.DrawOpenMouth(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void DrawOohFaceTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.DrawOohFace(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void DrawFaceTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.DrawFace(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void DrawSimpleBodyTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.DrawSimpleBody(canvas, 1.0);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void DrawTest()
        {
            var canvas = new Canvas {Width = 200, Height = 200};

            var blob = new Blob(13.0, 17.0, 11.0, 5);
            blob.Draw(canvas, 5.0);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }
    }
}