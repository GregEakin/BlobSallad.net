﻿using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    public class EnvironmentTests
    {
        [Test]
        public void CtorTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            Assert.AreEqual(37.0, environment.GetLeft());
            Assert.AreEqual(56.0, environment.GetRight());
            Assert.AreEqual(31.0, environment.GetTop());
            Assert.AreEqual(48.0, environment.GetBottom());
        }

        [Test]
        public void WidthTest()
        {
            var environment1 = new Environment(37.0, 31.0, 19.0, 17.0);
            var environment = environment1.SetWidth(23.0);

            Assert.AreNotSame(environment, environment1);
            Assert.AreEqual(37.0, environment.GetLeft());
            Assert.AreEqual(60.0, environment.GetRight());
            Assert.AreEqual(31.0, environment.GetTop());
            Assert.AreEqual(48.0, environment.GetBottom());
        }

        [Test]
        public void HeightTest()
        {
            var environment1 = new Environment(37.0, 31.0, 19.0, 17.0);
            var environment = environment1.SetHeight(23.0);

            Assert.AreNotSame(environment, environment1);
            Assert.AreEqual(37.0, environment.GetLeft());
            Assert.AreEqual(56.0, environment.GetRight());
            Assert.AreEqual(31.0, environment.GetTop());
            Assert.AreEqual(54.0, environment.GetBottom());
        }

        [Test]
        public void NonCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(47.0, 39.0);
            var collision = environment.Collision(curPos, curPos);
            Assert.False(collision);
        }

        [Test]
        public void LeftCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(36.0, 39.0);
            var collision = environment.Collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(37.0, curPos.GetX());
        }

        [Test]
        public void RightCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(57.0, 39.0);
            var collision = environment.Collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(56.0, curPos.GetX());
        }

        [Test]
        public void TopCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(47.0, 49.0);
            var collision = environment.Collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(48.0, curPos.GetY());
        }

        [Test]
        public void BottomCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(47.0, 30.0);
            var collision = environment.Collision(curPos, curPos);
            Assert.True(collision);
            Assert.AreEqual(31.0, curPos.GetY());
        }
    }
}