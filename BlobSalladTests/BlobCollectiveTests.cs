using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using NUnit.Framework;
using System.Threading;
using System.Windows.Controls;

namespace BlobSalladTests
{
    [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
    [Apartment(ApartmentState.STA)]
    public class BlobCollectiveTests
    {
        [Test]
        public void CtorTest()
        {
            var collective = new BlobCollective(71.0, 67.0, 4);

            Assert.AreEqual(4, collective.MaxNum);
            Assert.AreEqual(1, collective.NumActive);
        }

        [Test]
        public void SplitTest()
        {
            var canvas = new Canvas { Width = 200, Height = 200 };

            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.Split();
            Assert.AreEqual(2, collective.NumActive);
            collective.Draw(canvas, 100.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void JoinTest()
        {
            var canvas = new Canvas { Width = 200, Height = 200 };

            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.Split();
            collective.Join();
            Assert.AreEqual(1, collective.NumActive);
            collective.Draw(canvas, 100.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void SelectBlobMissTest()
        {
            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.SelectBlob(2.0, 2.0);
            Assert.IsNull(collective.SelectedBlob);
        }

        [Test]
        public void SelectBlobHitTest()
        {
            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.SelectBlob(1.0, 1.1);
            Assert.NotNull(collective.SelectedBlob);
            Assert.True(collective.SelectedBlob.Selected);
        }

        [Test]
        public void DrawTest()
        {
            var canvas = new Canvas { Width = 200, Height = 200 };

            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.Draw(canvas, 100.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }
    }
}