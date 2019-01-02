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
    public class ControllerTests
    {
        // [Test]
        public void drawTest()
        {
            var canvas = new Canvas { Width = 100, Height = 100 };

            var controller = new Controller();
            controller.paintComponent(canvas);

            var wpf = new ContentControl { Content = canvas };
            WpfApprovals.Verify(wpf);

        }
    }
}