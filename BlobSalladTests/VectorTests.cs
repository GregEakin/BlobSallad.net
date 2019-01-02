using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class VectorTests
    {
        [Test]
        public void CtorTest()
        {
            Vector vector = new Vector(71, 67);
            Assert.AreEqual(71.0, vector.GetX());
            Assert.AreEqual(67.0, vector.GetY());
        }

        [Test]
        public void AddXTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.AddX(100.0);
            Assert.AreEqual(171.0, vector.GetX());
        }

        [Test]
        public void AddYTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.AddY(100.0);
            Assert.AreEqual(167.0, vector.GetY());
        }

        [Test]
        public void SetTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(61.0, 59.0);
            vector.Set(setter);
            Assert.AreEqual(61.0, vector.GetX());
            Assert.AreEqual(59.0, vector.GetY());
        }

        [Test]
        public void AddTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(13.0, 11.0);
            vector.Add(setter);
            Assert.AreEqual(84.0, vector.GetX());
            Assert.AreEqual(78.0, vector.GetY());
        }

        [Test]
        public void SubTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(13.0, 11.0);
            vector.Sub(setter);
            Assert.AreEqual(58.0, vector.GetX());
            Assert.AreEqual(56.0, vector.GetY());
        }

        [Test]
        public void DotTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(13.0, 11.0);
            double dot = vector.DotProd(setter);
            Assert.AreEqual(1660.0, dot);
        }

        [Test]
        public void LengthTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            double length = vector.Length();
            Assert.AreEqual(97.621, length, 0.01);
        }

        [Test]
        public void ScaleTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.Scale(2.0);
            Assert.AreEqual(142.0, vector.GetX());
            Assert.AreEqual(134.0, vector.GetY());
        }

        [Test]
        public void StringTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Assert.AreEqual("(X: 71, Y: 67)", vector.ToString());
        }

        [Test]
        public void SetXTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.SetX(99.0);
            Assert.AreEqual(99.0, vector.GetX());
        }

        [Test]
        public void SetYTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.SetY(99.0);
            Assert.AreEqual(99.0, vector.GetY());
        }
    }
}