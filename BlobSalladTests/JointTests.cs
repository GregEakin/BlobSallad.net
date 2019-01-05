using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class JointTests
    {
        private Joint _joint;
        private PointMass _pointMassA;
        private PointMass _pointMassB;

        [Test]
        public void CtorTest()
        {
            Assert.AreEqual(36.498, _joint.ShortLimit, 0.01);
            Assert.AreEqual(40.340, _joint.LongLimit, 0.01);
        }

        [Test]
        public void SetDistTest()
        {
            _joint.SetLimit(38.5, 36.4);

            Assert.AreEqual(38.5, _joint.ShortLimit, 0.01);
            Assert.AreEqual(36.4, _joint.LongLimit, 0.01);

        }

        [Test]
        public void ScaleTest()
        {
            _joint.Scale(10.0);

            Assert.AreEqual(364.978, _joint.ShortLimit, 0.01);
            Assert.AreEqual(403.397, _joint.LongLimit, 0.01);
        }

        [Test]
        public void ScShortTest()
        {
            _joint.SetLimit(38.5, 36.4);
            _joint.Sc();

            Assert.AreEqual(40.968, _pointMassA.XPos, 0.01);
            Assert.AreEqual(42.975, _pointMassA.YPos, 0.01);
            Assert.AreEqual(71.032, _pointMassB.XPos, 0.01);
            Assert.AreEqual(67.025, _pointMassB.YPos, 0.01);
        }

        [Test]
        public void ScLongTest()
        {
            _joint.SetLimit(36.498, 36.4);
            _joint.Sc();

            Assert.AreEqual(41.809, _pointMassA.XPos, 0.01);
            Assert.AreEqual(43.647, _pointMassA.YPos, 0.01);
            Assert.AreEqual(70.191, _pointMassB.XPos, 0.01);
            Assert.AreEqual(66.353, _pointMassB.YPos, 0.01);
        }

        [SetUp]
        public void CreateJoint()
        {
            double cxA = 41;
            double cyA = 43;
            var massA = 4.0;
            _pointMassA = new PointMass(cxA, cyA, massA);

            var cxB = 71.0;
            var cyB = 67.0;
            var massB = 1.0;
            _pointMassB = new PointMass(cxB, cyB, massB);

            var low = 0.95;
            var high = 1.05;
            _joint = new Joint(_pointMassA, _pointMassB, low, high);
        }
    }
}