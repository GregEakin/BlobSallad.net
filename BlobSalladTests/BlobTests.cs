using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using NUnit.Framework;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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

        //[Test]
        //public void ctorPointMassesTest()
        //{
        //    Blob blob = new Blob(41.0, 43.0, 23.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        graphics.setColor(Color.BLACK);
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);

        //        PointMass[] pointMasses = blob.getPointMasses();
        //        for (PointMass pointMass : pointMasses)
        //        {
        //            Arc2D.Double arc = new Arc2D.Double();
        //            final double x = pointMass.getXPos();
        //            final double y = pointMass.getYPos();
        //            final double radius = pointMass.getMass();
        //            final double angSt = 0.0;
        //            final double angExt = -360.0;
        //            final int closure = 0;
        //            arc.setArcByCenter(x, y, radius, angSt, angExt, closure);
        //            graphics.draw(arc);
        //        }

        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //public void ctorSticksTest()
        //{
        //    Blob blob = new Blob(41.0, 43.0, 23.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        graphics.setColor(Color.BLACK);
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);

        //        Stick[] sticks = blob.getSticks();
        //        for (Stick stick : sticks)
        //            stick.draw(graphics, 1.0);

        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //public void ctorJointsTest()
        //{
        //    Blob blob = new Blob(41.0, 43.0, 23.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);

        //        final PointMass middlePointMass = blob.getMiddlePointMass();
        //        DrawDot(graphics, Color.BLUE, 2.0, middlePointMass.getXPos(), middlePointMass.getYPos());

        //        Joint[] joints = blob.getJoints();
        //        for (Joint joint : joints)
        //        {
        //            PointMass pointMassA = joint.getPointMassA();
        //            PointMass pointMassB = joint.getPointMassB();
        //            DrawDot(graphics, Color.RED, 2.0, pointMassA.getXPos(), pointMassA.getYPos());
        //            DrawLine(graphics, Color.BLACK,
        //                    pointMassA.getXPos(), pointMassA.getYPos(),
        //                    pointMassB.getXPos(), pointMassB.getYPos());
        //        }

        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //private void DrawDot(Graphics2D graphics, Color color, double radius, double x, double y)
        //{
        //    graphics.setColor(color);
        //    Arc2D.Double arc = new Arc2D.Double();
        //    final double angSt = 0.0;
        //    final double angExt = -360.0;
        //    final int closure = 0;
        //    arc.setArcByCenter(x, y, radius, angSt, angExt, closure);
        //    graphics.draw(arc);
        //}

        //private void DrawLine(Graphics2D graphics, Color color, double x1, double y1, double x2, double y2)
        //{
        //    graphics.setColor(color);
        //    GeneralPath generalPath = new GeneralPath();
        //    generalPath.moveTo(x1, y1);
        //    generalPath.lineTo(x2, y2);
        //    graphics.draw(generalPath);
        //}

        [Test]
        public void ctorPointMassTest()
        {
            Blob blob = new Blob(71.0, 67.0, 11.0, 5);
            for (int i = 0; i < 5; i++)
            {
                PointMass pointMas = blob.getPointMass(i);

                double mass = i < 2 ? 4.0 : 1.0;
                Assert.AreEqual(mass, pointMas.getMass());

                double theta = (double)i * 2.0 * Math.PI / (double)5;
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

        //[Test]
        //public void addBlobTest()
        //{
        //    Blob blob1 = new Blob(17.0, 19.0, 11.0, 5);
        //    Blob blob2 = new Blob(59.0, 61.0, 13.0, 5);
        //    blob1.addBlob(blob2);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);

        //        final PointMass middlePointMass = blob1.getMiddlePointMass();
        //        DrawDot(graphics, Color.BLUE, 2.0, middlePointMass.getXPos(), middlePointMass.getYPos());

        //        blob2.drawSimpleBody(graphics, 1.0);

        //        Joint[] joints = blob1.getJoints();
        //        for (Joint joint : joints)
        //        {
        //            PointMass pointMassA = joint.getPointMassA();
        //            PointMass pointMassB = joint.getPointMassB();
        //            DrawDot(graphics, Color.RED, 2.0, pointMassA.getXPos(), pointMassA.getYPos());
        //            DrawLine(graphics, Color.BLACK,
        //                    pointMassA.getXPos(), pointMassA.getYPos(),
        //                    pointMassB.getXPos(), pointMassB.getYPos());
        //        }

        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //public void scaleTest()
        //{
        //    Blob blob = new Blob(41.0, 43.0, 23.0, 5);
        //    blob.scale(3.0);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);
        //        graphics.translate(50.0, 50.0);

        //        blob.drawSmile(graphics, 1.0);
        //        blob.drawEyesOpen(graphics, 1.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //void moveTest()
        //{
        //    Blob blob = new Blob(41.0, 43.0, 23.0, 5);
        //    blob.setForce(new Vector(3.0, 3.0));
        //    blob.move(2.0);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);

        //        final PointMass middlePointMass = blob.getMiddlePointMass();
        //        DrawDot(graphics, Color.BLUE, 2.0, middlePointMass.getXPos(), middlePointMass.getYPos());
        //        blob.drawSimpleBody(graphics, 1.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //void scTest()
        //{
        //    Environment environment = new Environment(0.0, 0.0, 100.0, 100.0);
        //    Blob blob = new Blob(71.0, 67.0, 23.0, 5);
        //    blob.setForce(new Vector(3.0, 3.0));
        //    blob.move(3.0);
        //    blob.sc(environment);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);

        //        blob.drawSimpleBody(graphics, 1.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        [Test]
        public void setForceTest()
        {
            // TODO:
        }

        //[Test]
        //void moveToTest()
        //{
        //    Blob blob = new Blob(41.0, 43.0, 23.0, 5);
        //    blob.moveTo(61.0, 59.0);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());
        //        BasicStroke stroke = new BasicStroke(1.0F);
        //        graphics.setStroke(stroke);

        //        final PointMass middlePointMass = blob.getMiddlePointMass();
        //        DrawDot(graphics, Color.BLUE, 2.0, middlePointMass.getXPos(), middlePointMass.getYPos());

        //        blob.drawSimpleBody(graphics, 1.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        [Test]
        public void addForceTest()
        {
            // TODO:
        }

        //[Test]
        //public void drawHappyEyes1Test()
        //{
        //    Blob blob = new Blob(7.0, 11.0, 13.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        final double tx = blob.getMiddlePointMass().getXPos() * 6.0;
        //        final double ty = (blob.getMiddlePointMass().getYPos() - 0.35 * 11.0) * 6.0;
        //        graphics.translate(tx, ty);

        //        blob.drawEyesOpen(graphics, 5.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //public void drawHappyEyes2Test()
        //{
        //    Blob blob = new Blob(7.0, 11.0, 13.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        final double tx = blob.getMiddlePointMass().getXPos() * 6.0;
        //        final double ty = (blob.getMiddlePointMass().getYPos() - 0.35 * 11.0) * 6.0;
        //        graphics.translate(tx, ty);

        //        blob.drawEyesClosed(graphics, 5.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //public void drawHappyFace1Test()
        //{
        //    Blob blob = new Blob(7.0, 11.0, 13.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        final double tx = blob.getMiddlePointMass().getXPos() * 6.0;
        //        final double ty = (blob.getMiddlePointMass().getYPos() - 0.35 * 11.0) * 6.0;
        //        graphics.translate(tx, ty);

        //        blob.drawSmile(graphics, 5.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //public void drawHappyFace2Test()
        //{
        //    Blob blob = new Blob(7.0, 11.0, 13.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        final double tx = blob.getMiddlePointMass().getXPos() * 6.0;
        //        final double ty = (blob.getMiddlePointMass().getYPos() - 0.35 * 11.0) * 6.0;
        //        graphics.translate(tx, ty);

        //        blob.drawOpenMouth(graphics, 5.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        [Test]
        public void drawOohFaceTest()
        {
            var panel = new Canvas {Width = 100, Height = 100};

            var blob = new Blob(50.0, 50.0, 25.0, 5);
            blob.drawOohFace(panel, 3.0);

            var wpf = new ContentControl {Content = panel};
            WpfApprovals.Verify(wpf);
        }

        //[Test]
        //public void drawFaceTest()
        //{
        //    Blob blob = new Blob(7.0, 11.0, 13.0, 5);

        //    BufferedImage image = new BufferedImage(100, 100, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        final double tx = blob.getMiddlePointMass().getXPos() * 6.0;
        //        final double ty = (blob.getMiddlePointMass().getYPos() - 0.35 * 11.0) * 6.0;
        //        graphics.translate(tx, ty);

        //        blob.drawFace(graphics, 6.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //[Test]
        //public void drawSimpleBodyTest()
        //{
        //    Blob blob = new Blob(13.0, 17.0, 11.0, 5);

        //    BufferedImage image = new BufferedImage(200, 200, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        blob.drawSimpleBody(graphics, 5.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}

        //// [Test]
        //public void drawTest()
        //{
        //    Blob blob = new Blob(13.0, 17.0, 11.0, 5);

        //    BufferedImage image = new BufferedImage(200, 200, TYPE_INT_RGB);
        //    Graphics2D graphics = image.createGraphics();
        //    try
        //    {
        //        graphics.setPaint(Color.WHITE);
        //        graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //        blob.draw(graphics, 5.0);
        //        Approvals.verify(image);
        //    }
        //    finally
        //    {
        //        graphics.dispose();
        //    }
        //}
    }
}