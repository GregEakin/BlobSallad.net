using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class NeighborTests
    {
        private Neighbor _collision;
        private PointMass _pointMassA;
        private PointMass _pointMassB;

        [Test]
        public void CtorNeighborTest()
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
            var dist = 17.0;
            _collision = new Neighbor(_pointMassA, _pointMassB, dist);

            Assert.AreEqual(dist, _collision.ShortLimit, 0.01);
        }
    }
}