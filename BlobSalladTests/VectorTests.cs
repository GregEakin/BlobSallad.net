using BlobSallad;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BlobSalladTests
{
    public class VectorTests
    {
        [Test]
        public void CtorTest()
        {
            var vector = new Vector(71, 67);
            ClassicAssert.AreEqual(71.0, vector.X);
            ClassicAssert.AreEqual(67.0, vector.Y);
        }

        [Test]
        public void AddXTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.AddX(100.0);
            ClassicAssert.AreEqual(171.0, vector.X);
        }

        [Test]
        public void AddYTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.AddY(100.0);
            ClassicAssert.AreEqual(167.0, vector.Y);
        }

        [Test]
        public void SetTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(61.0, 59.0);
            vector.Set(setter);
            ClassicAssert.AreEqual(61.0, vector.X);
            ClassicAssert.AreEqual(59.0, vector.Y);
        }

        [Test]
        public void AddTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(13.0, 11.0);
            vector.Add(setter);
            ClassicAssert.AreEqual(84.0, vector.X);
            ClassicAssert.AreEqual(78.0, vector.Y);
        }

        [Test]
        public void SubTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(13.0, 11.0);
            vector.Sub(setter);
            ClassicAssert.AreEqual(58.0, vector.X);
            ClassicAssert.AreEqual(56.0, vector.Y);
        }

        [Test]
        public void OperatorSubtractTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(13.0, 11.0);
            var b = vector - setter;
            ClassicAssert.AreEqual(58.0, b.X);
            ClassicAssert.AreEqual(56.0, b.Y);
        }

        [Test]
        public void DotTest()
        {
            var vector = new Vector(71.0, 67.0);
            var setter = new Vector(13.0, 11.0);
            var dot = vector.DotProd(setter);
            ClassicAssert.AreEqual(1660.0, dot);
        }

        [Test]
        public void LengthTest()
        {
            var vector = new Vector(71.0, 67.0);
            var length = vector.Length;
            ClassicAssert.AreEqual(97.621, length, 0.01);
        }

        [Test]
        public void ScaleTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.Scale(2.0);
            ClassicAssert.AreEqual(142.0, vector.X);
            ClassicAssert.AreEqual(134.0, vector.Y);
        }

        [Test]
        public void StringTest()
        {
            var vector = new Vector(71.0, 67.0);
            ClassicAssert.AreEqual("(X: 71, Y: 67)", vector.ToString());
        }

        [Test]
        public void SetXTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.X = 99.0;
            ClassicAssert.AreEqual(99.0, vector.X);
        }

        [Test]
        public void SetYTest()
        {
            var vector = new Vector(71.0, 67.0);
            vector.Y = 99.0;
            ClassicAssert.AreEqual(99.0, vector.Y);
        }
    }
}