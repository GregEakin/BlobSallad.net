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
        public void ctorTest()
        {
            BlobCollective collective = new BlobCollective(71.0, 67.0, 4);

            Assert.AreEqual(4, collective.getMaxNum());
            Assert.AreEqual(1, collective.getNumActive());
        }

        [Test]
        public void splitTest()
        {
            var canvas = new Canvas { Width = 200, Height = 200 };

            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.split();
            Assert.AreEqual(2, collective.getNumActive());
            collective.draw(canvas, 100.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void joinTest()
        {
            var canvas = new Canvas { Width = 200, Height = 200 };

            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.split();
            // collective.join();
            // Assert.AreEqual(2, collective.getNumActive());
            collective.draw(canvas, 100.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }

        [Test]
        public void selectBlobMissTest()
        {
            BlobCollective collective = new BlobCollective(1.0, 1.0, 4);
            collective.selectBlob(2.0, 2.0);
            Assert.IsNull(collective.getSelectedBlob());
        }

        [Test]
        public void selectBlobHitTest()
        {
            BlobCollective collective = new BlobCollective(1.0, 1.0, 4);
            collective.selectBlob(1.0, 1.1);
            Assert.NotNull(collective.getSelectedBlob());
            Assert.True(collective.getSelectedBlob().getSelected());
        }

        [Test]
        public void drawTest()
        {
            var canvas = new Canvas { Width = 200, Height = 200 };

            var collective = new BlobCollective(1.0, 1.0, 4);
            collective.draw(canvas, 100.0);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);
        }
    }
}