using System.Threading;
using System.Windows.Controls;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.AreSame(massA, stick.PointMassA);
            ClassicAssert.AreSame(massB, stick.PointMassB);
            ClassicAssert.AreEqual(5.656, stick.Length, 0.01);
            ClassicAssert.AreEqual(32.0, stick.LengthSquared, 0.01);
        }

        [Test]
        public void ScaleTest()
        {
            var massA = new PointMass(13.0, 31.0, 5.0);
            var massB = new PointMass(17.0, 35.0, 7.0);
            var stick = new Skin(massA, massB);
            stick.Scale(2.0);
            ClassicAssert.AreEqual(11.313, stick.Length, 0.01);
            ClassicAssert.AreEqual(128.0, stick.LengthSquared, 0.01);
        }

        [Test]
        public void LengthTest()
        {
            var massA = new PointMass(13.0, 31.0, 5.0);
            var massB = new PointMass(17.0, 35.0, 7.0);
            var stick = new Skin(massA, massB);

            ClassicAssert.AreEqual(5.657, stick.Length, 0.01);
            ClassicAssert.AreEqual(32.000, stick.LengthSquared, 0.01);
        }

        [Test]
        public void ScTest()
        {
            var massA = new PointMass(13.0, 17.0, 5.0);
            var massB = new PointMass(43.0, 41.0, 7.0);
            var stick = new Skin(massA, massB);
            stick.Sc(null);

            ClassicAssert.AreEqual(13.0, stick.PointMassA.XPos, 0.01);
            ClassicAssert.AreEqual(17.0, stick.PointMassA.YPos, 0.01);
            ClassicAssert.AreEqual(43.0, stick.PointMassB.XPos, 0.01);
            ClassicAssert.AreEqual(41.0, stick.PointMassB.YPos, 0.01);
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