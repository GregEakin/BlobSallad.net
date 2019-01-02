using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using NUnit.Framework;
using System;
using System.Threading;
using System.Windows.Controls;
using Vector = BlobSallad.Vector;

namespace BlobSalladTests
{
    [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
    [Apartment(ApartmentState.STA)]
    public class PointMassTests
    {
        [Test]
        public void ctorTest()
        {
            PointMass pointMass = new PointMass(31.0, 23.0, 11.0);
            Assert.AreEqual(11.0, pointMass.getMass());
        }

        [Test]
        public void moveTest()
        {
            PointMass pointMass = new PointMass(31.0, 23.0, 11.0);
            Vector force = new Vector(7, 13);
            pointMass.setForce(force);

            pointMass.move(3.0);
            Assert.AreEqual(31.0, pointMass.getXPrevPos(), 0.01);
            Assert.AreEqual(23.0, pointMass.getYPrevPos(), 0.01);
            Assert.AreEqual(36.727, pointMass.getXPos(), 0.01);
            Assert.AreEqual(33.636, pointMass.getYPos(), 0.01);
        }

        [Test]
        [STAThread]
        public void drawTest()
        {
            var panel = new Canvas { Width = 100, Height = 100 };

            var pointMass = new PointMass(11.0, 13.0, 11.0);
            pointMass.draw(panel, 2.0);

            var wpf = new ContentControl { Content = panel };
            WpfApprovals.Verify(wpf);
        }
    }
}