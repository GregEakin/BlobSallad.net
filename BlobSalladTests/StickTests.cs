using System.Threading;
using System.Windows.Controls;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using NUnit.Framework;

namespace BlobSalladTests
{
    [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
    [Apartment(ApartmentState.STA)]
    public class StickTests
    {
        [Test]
        public void CtorTest()
        {
            var massA = new PointMass(13.0, 31.0, 5.0);
            var massB = new PointMass(17.0, 35.0, 7.0);
            var stick = new Skin(massA, massB);
            Assert.AreSame(massA, stick.PointMassA);
            Assert.AreSame(massB, stick.PointMassB);
            Assert.AreEqual(5.656, stick.Length, 0.01);
            Assert.AreEqual(32.0, stick.LengthSquared, 0.01);
        }

        [Test]
        public void ScaleTest()
        {
            var massA = new PointMass(13.0, 31.0, 5.0);
            var massB = new PointMass(17.0, 35.0, 7.0);
            var stick = new Skin(massA, massB);
            stick.Scale(2.0);
            Assert.AreEqual(11.313, stick.Length, 0.01);
            Assert.AreEqual(128.0, stick.LengthSquared, 0.01);
        }

        [Test]
        public void LengthTest()
        {
            var massA = new PointMass(13.0, 31.0, 5.0);
            var massB = new PointMass(17.0, 35.0, 7.0);
            var stick = new Skin(massA, massB);

            Assert.AreEqual(5.657, stick.Length, 0.01);
            Assert.AreEqual(32.000, stick.LengthSquared, 0.01);
        }

        [Test]
        public void ScTest()
        {
            var massA = new PointMass(13.0, 17.0, 5.0);
            var massB = new PointMass(43.0, 41.0, 7.0);
            var stick = new Skin(massA, massB);
            stick.Sc(null);

            Assert.AreEqual(13.0, stick.PointMassA.XPos, 0.01);
            Assert.AreEqual(17.0, stick.PointMassA.YPos, 0.01);
            Assert.AreEqual(43.0, stick.PointMassB.XPos, 0.01);
            Assert.AreEqual(41.0, stick.PointMassB.YPos, 0.01);
        }

        [Test]
        public void DrawTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var massA = new PointMass(13.0, 17.0, 5.0);
            var massB = new PointMass(43.0, 41.0, 7.0);
            var stick = new Skin(massA, massB);


            stick.Draw(canvas, 2.0);
            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }
    }
}