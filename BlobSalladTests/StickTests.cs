using ApprovalTests;
using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class StickTests
    {
        [Test]
        public void CtorTest()
        {
            PointMass massA = new PointMass(13.0, 31.0, 5.0);
            PointMass massB = new PointMass(17.0, 35.0, 7.0);
            Stick stick = new Stick(massA, massB);
            Assert.AreSame(massA, stick.GetPointMassA());
            Assert.AreSame(massB, stick.GetPointMassB());
            Assert.AreEqual(5.656, stick.GetLength(), 0.01);
            Assert.AreEqual(32.0, stick.GetLengthSquared(), 0.01);
        }

        [Test]
        public void PointMassDistTest()
        {
            PointMass massA = new PointMass(13.0, 31.0, 5.0);
            PointMass massB = new PointMass(17.0, 35.0, 7.0);
            double dist = Stick.PointMassDist(massA, massB);
            Assert.AreEqual(5.656, dist, 0.01);
        }

        [Test]
        public void ScaleTest()
        {
            PointMass massA = new PointMass(13.0, 31.0, 5.0);
            PointMass massB = new PointMass(17.0, 35.0, 7.0);
            Stick stick = new Stick(massA, massB);
            stick.Scale(2.0);
            Assert.AreEqual(11.313, stick.GetLength(), 0.01);
            Assert.AreEqual(128.0, stick.GetLengthSquared(), 0.01);
        }

        [Test]
        public void LengthTest()
        {
            PointMass massA = new PointMass(13.0, 31.0, 5.0);
            PointMass massB = new PointMass(17.0, 35.0, 7.0);
            Stick stick = new Stick(massA, massB);

            Assert.AreEqual(5.657, stick.GetLength(), 0.01);
            Assert.AreEqual(32.000, stick.GetLengthSquared(), 0.01);
        }

        [Test]
        public void ScTest()
        {
            //PointMass massA = new PointMass(13.0, 17.0, 5.0);
            //PointMass massB = new PointMass(53.0, 59.0, 7.0);
            //Stick stick = new Stick(massA, massB);
            //stick.sc(null);
            //
            //Assert.AreEqual(13.0, stick.getPointMassA().getXPos(), 0.01);
            //Assert.AreEqual(17.0, stick.getPointMassA().getYPos(), 0.01);
            //Assert.AreEqual(53.0, stick.getPointMassB().getXPos(), 0.01);
            //Assert.AreEqual(59.0, stick.getPointMassB().getYPos(), 0.01);
        }

        //    [Test]
        //public void drawTest()
        //    {
        //        PointMass massA = new PointMass(13.0, 19.0, 5.0);
        //        PointMass massB = new PointMass(11.0, 17.0, 7.0);
        //        Stick stick = new Stick(massA, massB);

        //        BufferedImage image = new BufferedImage(50, 50, TYPE_INT_RGB);
        //        Graphics2D graphics = image.createGraphics();
        //        try
        //        {
        //            graphics.setPaint(Color.WHITE);
        //            graphics.fillRect(0, 0, image.getWidth(), image.getHeight());

        //            stick.draw(graphics, 2.0);
        //            Approvals.verify(image);
        //        }
        //        finally
        //        {
        //            graphics.dispose();
        //        }
        //    }
        }
    }