using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class PointTests
    {
        [Test]
        public void CtorTest()
        {
            var point = new Point(23.0, 31.0);
            Assert.AreEqual(23.0, point.X);
            Assert.AreEqual(31.0, point.Y);
        }
    }
}