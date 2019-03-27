using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class CollisionTests
    {
        private Collision _collision;
        private PointMass _pointMassA;
        private PointMass _pointMassB;

        [Test]
        public void CtorCollisionTest()
        {
            var cxA = 41.0;
            var cyA = 43.0;
            var massA = 4.0;
            _pointMassA = new PointMass(cxA, cyA, massA);

            var cxB = 71.0;
            var cyB = 67.0;
            var massB = 1.0;
            _pointMassB = new PointMass(cxB, cyB, massB);

            // sum of the two radii
            var dist = 17;
            _collision = new Collision(_pointMassA, _pointMassB, dist);

            Assert.AreEqual(17.000, _collision.ShortLimit, 0.01);
        }
    }
}