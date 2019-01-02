using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class VectorTests
    {
        [Test]
        public void ctorTest()
        {
            Vector vector = new Vector(71, 67);
            Assert.AreEqual(71.0, vector.getX());
            Assert.AreEqual(67.0, vector.getY());
        }

        [Test]
        public void addXTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.addX(100.0);
            Assert.AreEqual(171.0, vector.getX());
        }

        [Test]
        public void addYTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.addY(100.0);
            Assert.AreEqual(167.0, vector.getY());
        }

        [Test]
        public void setTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(61.0, 59.0);
            vector.set(setter);
            Assert.AreEqual(61.0, vector.getX());
            Assert.AreEqual(59.0, vector.getY());
        }

        [Test]
        public void addTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(13.0, 11.0);
            vector.add(setter);
            Assert.AreEqual(84.0, vector.getX());
            Assert.AreEqual(78.0, vector.getY());
        }

        [Test]
        public void subTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(13.0, 11.0);
            vector.sub(setter);
            Assert.AreEqual(58.0, vector.getX());
            Assert.AreEqual(56.0, vector.getY());
        }

        [Test]
        public void dotTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Vector setter = new Vector(13.0, 11.0);
            double dot = vector.dotProd(setter);
            Assert.AreEqual(1660.0, dot);
        }

        [Test]
        public void lengthTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            double length = vector.length();
            Assert.AreEqual(97.621, length, 0.01);
        }

        [Test]
        public void scaleTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.scale(2.0);
            Assert.AreEqual(142.0, vector.getX());
            Assert.AreEqual(134.0, vector.getY());
        }

        [Test]
        public void stringTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            Assert.AreEqual("(X: 71, Y: 67)", vector.toString());
        }

        [Test]
        public void setXTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.setX(99.0);
            Assert.AreEqual(99.0, vector.getX());
        }

        [Test]
        public void setYTest()
        {
            Vector vector = new Vector(71.0, 67.0);
            vector.setY(99.0);
            Assert.AreEqual(99.0, vector.getY());
        }
    }
}