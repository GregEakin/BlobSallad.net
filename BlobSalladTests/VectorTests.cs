using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class VectorTests
    {
        [Test]
        public void CtorTest()
        {
            var vector = new Vector(71, 67);
            Assert.AreEqual(71.0, vector.GetX());
            Assert.AreEqual(67.0, vector.GetY());
        }

        [Test]
        public void AddXTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.AddX(100.0);
            Assert.AreEqual(171.0, vector.GetX());
        }

        [Test]
        public void AddYTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.AddY(100.0);
            Assert.AreEqual(167.0, vector.GetY());
        }

        [Test]
        public void SetTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(61.0, 59.0);
            vector.Set(setter);
            Assert.AreEqual(61.0, vector.GetX());
            Assert.AreEqual(59.0, vector.GetY());
        }

        [Test]
        public void AddTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(13.0, 11.0);
            vector.Add(setter);
            Assert.AreEqual(84.0, vector.GetX());
            Assert.AreEqual(78.0, vector.GetY());
        }

        [Test]
        public void SubTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(13.0, 11.0);
            vector.Sub(setter);
            Assert.AreEqual(58.0, vector.GetX());
            Assert.AreEqual(56.0, vector.GetY());
        }

        [Test]
        public void DotTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(13.0, 11.0);
            var dot = vector.DotProd(setter);
            Assert.AreEqual(1660.0, dot);
        }

        [Test]
        public void LengthTest()
        {
            var vector = new Vector(71.0, 67.0);
            var length = vector.Length();
            Assert.AreEqual(97.621, length, 0.01);
        }

        [Test]
        public void ScaleTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.Scale(2.0);
            Assert.AreEqual(142.0, vector.GetX());
            Assert.AreEqual(134.0, vector.GetY());
        }

        [Test]
        public void StringTest()
        {
            var vector = new Vector(71.0, 67.0);
            Assert.AreEqual("(X: 71, Y: 67)", vector.ToString());
        }

        [Test]
        public void SetXTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.SetX(99.0);
            Assert.AreEqual(99.0, vector.GetX());
        }

        [Test]
        public void SetYTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.SetY(99.0);
            Assert.AreEqual(99.0, vector.GetY());
        }
    }
}