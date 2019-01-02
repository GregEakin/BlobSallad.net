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
        public void ctorTest()
        {
            Blob blob = new Blob(71.0, 67.0, 11.0, 5);
            Assert.AreEqual(71.0, blob.getXPos());
            Assert.AreEqual(67.0, blob.getYPos());
            Assert.AreEqual(11.0, blob.getRadius());

            PointMass pointMas = blob.getMiddlePointMass();
            Assert.AreEqual(71.0, pointMas.getXPos());
            Assert.AreEqual(67.0, pointMas.getYPos());
            Assert.AreEqual(1.0, pointMas.getMass());
        }

        [Test]
        public void ctorPointMassesTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            var pointMasses = blob.getPointMasses();
            foreach (var pointMass in pointMasses)
                DrawDot(canvas, Brushes.Black, pointMass.getMass(), pointMass.getXPos(), pointMass.getYPos());

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void ctorSticksTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            var sticks = blob.getSticks();
            foreach (var stick in sticks)
                stick.draw(canvas, 1.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void ctorJointsTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var blob = new Blob(41.0, 43.0, 23.0, 5);
            var middlePointMass = blob.getMiddlePointMass();
            DrawDot(canvas, Brushes.Blue, middlePointMass.getMass(), middlePointMass.getXPos(), middlePointMass.getYPos());

            var joints = blob.getJoints();
            foreach (var joint in joints)
            {
                PointMass pointMassA = joint.getPointMassA();
                PointMass pointMassB = joint.getPointMassB();
                DrawDot(canvas, Brushes.Red, pointMassA.getMass(), pointMassA.getXPos(), pointMassA.getYPos());
                DrawLine(canvas, Brushes.Black, 
                        pointMassA.getXPos(), pointMassA.getYPos(),
                        pointMassB.getXPos(), pointMassB.getYPos());
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
                Fill = System.Windows.Media.Brushes.Black,
                Stroke = System.Windows.Media.Brushes.Black,
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
        public void ctorPointMassTest()
        {
            Blob blob = new Blob(71.0, 67.0, 11.0, 5);
            for (int i = 0; i < 5; i++)
            {
                PointMass pointMas = blob.getPointMass(i);

                double mass = i < 2 ? 4.0 : 1.0;
                Assert.AreEqual(mass, pointMas.getMass());

                double theta = (double) i * 2.0 * Math.PI / (double) 5;
                double cx = Math.Cos(theta) * 11.0 + 71.0;
                double cy = Math.Sin(theta) * 11.0 + 67.0;
                Assert.AreEqual(cx, pointMas.getXPos());
                Assert.AreEqual(cy, pointMas.getYPos());
            }
        }

        [Test]
        public void addBlob2Test()
        {
            Blob blob1 = new Blob(17.0, 19.0, 11.0, 5);
            Blob blob2 = new Blob(59.0, 61.0, 13.0, 5);
            blob1.addBlob(blob2);
            Joint joint = blob1.getJoints()[10];
            Assert.AreEqual(22.800, joint.getShortConst(), 0.01);
            Assert.AreEqual(0.0, joint.getLongConst(), 0.01);
        }

        [Test]
        public void addBlobTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            Blob blob1 = new Blob(17.0, 19.0, 11.0, 5);
            Blob blob2 = new Blob(59.0, 61.0, 13.0, 5);
            blob1.addBlob(blob2);

            var middlePointMass = blob1.getMiddlePointMass();
            DrawDot(canvas, Brushes.Blue, middlePointMass.getMass(), middlePointMass.getXPos(), middlePointMass.getYPos());

            blob2.drawSimpleBody(canvas, 1.0);

            var joints = blob1.getJoints();
            foreach (var joint in joints)
            {
                PointMass pointMassA = joint.getPointMassA();
                PointMass pointMassB = joint.getPointMassB();
                DrawDot(canvas, Brushes.Red, 2.0, pointMassA.getXPos(), pointMassA.getYPos());
                DrawLine(canvas, Brushes.Black, 
                        pointMassA.getXPos(), pointMassA.getYPos(),
                        pointMassB.getXPos(), pointMassB.getYPos());
            }

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void scaleTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var translateTransform = new TranslateTransform(41.0, 43.0);
            var blob = new Blob(41.0, 43.0, 23.0, 5);
            blob.scale(3.0);
            blob.drawSmile(canvas, 1.0, translateTransform);
            blob.drawEyesOpen(canvas, 1.0, translateTransform);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void moveTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            Blob blob = new Blob(41.0, 43.0, 23.0, 5);
            blob.setForce(new Vector(3.0, 3.0));
            blob.move(2.0);

            var middlePointMass = blob.getMiddlePointMass();
            DrawDot(canvas, Brushes.Blue, 2.0, middlePointMass.getXPos(), middlePointMass.getYPos());
            blob.drawSimpleBody(canvas, 1.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void scTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var environment = new Environment(0.0, 0.0, 100.0, 100.0);
            var blob = new Blob(71.0, 67.0, 23.0, 5);
            blob.setForce(new Vector(3.0, 3.0));
            blob.move(3.0);
            blob.sc(environment);
            blob.drawSimpleBody(canvas, 1.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void setForceTest()
        {
            // TODO:
        }

        [Test]
        public void moveToTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            Blob blob = new Blob(41.0, 43.0, 23.0, 5);
            blob.moveTo(61.0, 59.0);
            blob.drawSimpleBody(canvas, 1.0);

            var middlePointMass = blob.getMiddlePointMass();
            DrawDot(canvas, Brushes.Blue, 2.0, middlePointMass.getXPos(), middlePointMass.getXPos());

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void addForceTest()
        {
            // TODO:
        }

        [Test]
        public void drawEyesOpenTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawEyesOpen(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void drawEyesClosedTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawEyesClosed(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void drawSmileTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawSmile(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void drawOpenMouthTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawOpenMouth(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void drawOohFaceTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawOohFace(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void drawFaceTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var translateTransform = new TranslateTransform(50.0, 50.0);
            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawFace(canvas, 3.0, translateTransform);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void drawSimpleBodyTest()
        {
            var canvas = new Canvas {Width = 100, Height = 100};

            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawSimpleBody(canvas, 1.0);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void drawTest()
        {
            var canvas = new Canvas {Width = 200, Height = 200};

            var blob = new Blob(13.0, 17.0, 11.0, 5);
            blob.draw(canvas, 5.0);

            var wpf = new ContentControl {Content = canvas};
            WpfApprovals.Verify(wpf);
        }
    }
}