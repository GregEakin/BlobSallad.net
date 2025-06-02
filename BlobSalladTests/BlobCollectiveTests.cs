using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using System.Windows.Controls;
using Xunit;
using Environment = BlobSallad.Environment;

namespace BlobSalladTests;

[UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
public class BlobCollectiveTests
{
    [Fact]
    public void CtorTest()
    {
        var collective = new BlobCollective(71.0, 67.0, 4);

        Assert.Equal(4, collective.MaxNum);
        Assert.Equal(1, collective.NumActive);
    }

    [WpfFact]
    public void SplitTest()
    {
        var canvas = new Canvas { Width = 200, Height = 200 };

        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.Split();
        Assert.Equal(2, collective.NumActive);
        collective.Draw(canvas, 100.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [Fact]
    public void FindLargestTest()
    {
        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.Split();
        collective.Split();

        var motherBlob = collective.FindLargest(null);
        Assert.Equal(0.300, motherBlob.Radius, 0.01);
    }

    [Fact]
    public void FindSmallestTest()
    {
        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.Split();
        collective.Split();

        var largest = collective.FindLargest(null);

        // Find one of the two smallest blobs.
        var smallest1 = collective.FindSmallest(null);
        Assert.NotSame(largest, smallest1);
        Assert.Equal(0.225, smallest1.Radius, 0.01);

        // Find the other smallest blob.
        var smallest2 = collective.FindSmallest(smallest1);
        Assert.NotSame(largest, smallest2);
        Assert.NotSame(smallest1, smallest2);
        Assert.Equal(0.225, smallest2.Radius, 0.01);
    }

    [Fact]
    public void FindClosestTest()
    {
        var environment = new Environment(0.2, 0.2, 2.6, 1.6);

        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.Split();
        collective.Split();

        collective.Move(1.0);
        collective.Sc(environment);

        var largest = collective.FindLargest(null);
        var smallest1 = collective.FindSmallest(null);
        var smallest2 = collective.FindSmallest(smallest1);

        var closest = collective.FindClosest(largest);
        Assert.Same(smallest2, closest);
    }

    [WpfFact]
    public void JoinTest()
    {
        var canvas = new Canvas { Width = 200, Height = 200 };

        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.Split();
        collective.Join();
        Assert.Equal(1, collective.NumActive);
        collective.Draw(canvas, 100.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }

    [Fact]
    public void SelectBlobMissTest()
    {
        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.FindClosest(2.0, 2.0);
        Assert.Null(collective.SelectedBlob);
    }

    [Fact]
    public void SelectBlobHitTest()
    {
        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.FindClosest(1.0, 1.1);
        Assert.NotNull(collective.SelectedBlob);
        Assert.True(collective.SelectedBlob.Selected);
    }

    [WpfFact]
    public void DrawTest()
    {
        var canvas = new Canvas { Width = 200, Height = 200 };

        var collective = new BlobCollective(1.0, 1.0, 4);
        collective.Draw(canvas, 100.0);

        var wpf = new ContentControl { Content = canvas };
        WpfApprovals.Verify(wpf);
    }
}