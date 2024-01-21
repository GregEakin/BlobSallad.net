﻿using BlobSallad;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BlobSalladTests
{
    public class EnvironmentTests
    {
        [Test]
        public void CtorTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            ClassicAssert.AreEqual(37.0, environment.Left);
            ClassicAssert.AreEqual(56.0, environment.Right);
            ClassicAssert.AreEqual(31.0, environment.Top);
            ClassicAssert.AreEqual(48.0, environment.Bottom);
        }

        [Test]
        public void WidthTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            environment.Width = 23.0;

            ClassicAssert.AreEqual(37.0, environment.Left);
            ClassicAssert.AreEqual(60.0, environment.Right);
            ClassicAssert.AreEqual(31.0, environment.Top);
            ClassicAssert.AreEqual(48.0, environment.Bottom);
        }

        [Test]
        public void HeightTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            environment.Height = 23.0;

            ClassicAssert.AreEqual(37.0, environment.Left);
            ClassicAssert.AreEqual(56.0, environment.Right);
            ClassicAssert.AreEqual(31.0, environment.Top);
            ClassicAssert.AreEqual(54.0, environment.Bottom);
        }

        [Test]
        public void NonCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(47.0, 39.0);
            var collision = environment.Collision(curPos, curPos);
            ClassicAssert.False(collision);
        }

        [Test]
        public void LeftCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(36.0, 39.0);
            var collision = environment.Collision(curPos, curPos);
            ClassicAssert.True(collision);
            ClassicAssert.AreEqual(37.0, curPos.X);
        }

        [Test]
        public void RightCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(57.0, 39.0);
            var collision = environment.Collision(curPos, curPos);
            ClassicAssert.True(collision);
            ClassicAssert.AreEqual(56.0, curPos.X);
        }

        [Test]
        public void TopCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(47.0, 49.0);
            var collision = environment.Collision(curPos, curPos);
            ClassicAssert.True(collision);
            ClassicAssert.AreEqual(48.0, curPos.Y);
        }

        [Test]
        public void BottomCollisionTest()
        {
            var environment = new Environment(37.0, 31.0, 19.0, 17.0);
            var curPos = new Vector(47.0, 30.0);
            var collision = environment.Collision(curPos, curPos);
            ClassicAssert.True(collision);
            ClassicAssert.AreEqual(31.0, curPos.Y);
        }
    }
}