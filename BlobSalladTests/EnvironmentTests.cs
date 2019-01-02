using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class EnvironmentTests
    {
        [Test]
        public void ctorTest()
        {
            Environment environment = new Environment(37.0, 31.0, 19.0, 17.0);
            Assert.AreEqual(37.0, environment.getLeft());
            Assert.AreEqual(56.0, environment.getRight());
            Assert.AreEqual(31.0, environment.getTop());
            Assert.AreEqual(48.0, environment.getBottom());
        }

        [Test]
        public void widthTest()
        {
            Environment environment1 = new Environment(37.0, 31.0, 19.0, 17.0);
            Environment environment = environment1.setWidth(23.0);

            Assert.AreNotSame(environment, environment1);
            Assert.AreEqual(37.0, environment.getLeft());
            Assert.AreEqual(60.0, environment.getRight());
            Assert.AreEqual(31.0, environment.getTop());
            Assert.AreEqual(48.0, environment.getBottom());
        }

        [Test]
        public void heightTest()
        {
            Environment environment1 = new Environment(37.0, 31.0, 19.0, 17.0);
            Environment environment = environment1.setHeight(23.0);

            Assert.AreNotSame(environment, environment1);
            Assert.AreEqual(37.0, environment.getLeft());
            Assert.AreEqual(56.0, environment.getRight());
            Assert.AreEqual(31.0, environment.getTop());
            Assert.AreEqual(54.0, environment.getBottom());
        }

        [Test]
        public void nonCollisionTest()
        {
            Environment environment = new Environment(37.0, 31.0, 19.0, 17.0);
            Vector curPos = new Vector(47.0, 39.0);
            var collision = environment.collision(curPos, curPos);
            Assert.False(collision);
        }

        [Test]
        public void leftCollisionTest()
        {
            Environment environment = new Environment(37.0, 31.0, 19.0, 17.0);
            Vector curPos = new Vector(36.0, 39.0);
            var collision = environment.collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(37.0, curPos.getX());
        }

        [Test]
        public void rightCollisionTest()
        {
            Environment environment = new Environment(37.0, 31.0, 19.0, 17.0);
            Vector curPos = new Vector(57.0, 39.0);
            var collision = environment.collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(56.0, curPos.getX());
        }

        [Test]
        public void topCollisionTest()
        {
            Environment environment = new Environment(37.0, 31.0, 19.0, 17.0);
            Vector curPos = new Vector(47.0, 49.0);
            var collision = environment.collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(48.0, curPos.getY());
        }

        [Test]
        public void bottomCollisionTest()
        {
            Environment environment = new Environment(37.0, 31.0, 19.0, 17.0);
            Vector curPos = new Vector(47.0, 30.0);
            var collision = environment.collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(31.0, curPos.getY());
        }
    }
}