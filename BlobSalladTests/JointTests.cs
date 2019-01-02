using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class JointTests
    {
        private Joint joint;
        private PointMass pointMassA;
        private PointMass pointMassB;

        [Test]
        public void ctorTest()
        {
            Assert.AreEqual(36.498, joint.getShortConst(), 0.01);
            Assert.AreEqual(40.340, joint.getLongConst(), 0.01);
        }

        [Test]
        public void setDistTest()
        {
            joint.setDist(38.5, 36.4);

            Assert.AreEqual(38.5, joint.getShortConst(), 0.01);
            Assert.AreEqual(36.4, joint.getLongConst(), 0.01);

        }

        [Test]
        public void scaleTest()
        {
            joint.scale(10.0);

            Assert.AreEqual(364.978, joint.getShortConst(), 0.01);
            Assert.AreEqual(403.397, joint.getLongConst(), 0.01);
        }

        [Test]
        public void scShortTest()
        {
            joint.setDist(38.5, 36.4);
            joint.sc();

            Assert.AreEqual(40.968, pointMassA.getXPos(), 0.01);
            Assert.AreEqual(42.975, pointMassA.getYPos(), 0.01);
            Assert.AreEqual(71.032, pointMassB.getXPos(), 0.01);
            Assert.AreEqual(67.025, pointMassB.getYPos(), 0.01);
        }

        [Test]
        public void scLongTest()
        {
            joint.setDist(36.498, 36.4);
            joint.sc();

            Assert.AreEqual(41.809, pointMassA.getXPos(), 0.01);
            Assert.AreEqual(43.647, pointMassA.getYPos(), 0.01);
            Assert.AreEqual(70.191, pointMassB.getXPos(), 0.01);
            Assert.AreEqual(66.353, pointMassB.getYPos(), 0.01);
        }

        [SetUp]
        public void createJoint()
        {
            double cxA = 41;
            double cyA = 43;
            double massA = 4.0;
            pointMassA = new PointMass(cxA, cyA, massA);

            double cxB = 71.0;
            double cyB = 67.0;
            double massB = 1.0;
            pointMassB = new PointMass(cxB, cyB, massB);

            double low = 0.95;
            double high = 1.05;
            joint = new Joint(pointMassA, pointMassB, low, high);
        }
    }
}