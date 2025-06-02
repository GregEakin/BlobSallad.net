using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using BlobSallad;
using System.Windows.Controls;
using Xunit;
using Vector = BlobSallad.Vector;

namespace BlobSalladTests;

[UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
public class PointMassTests
{
    [Fact]
    public void CtorTest()
    {
        var pointMass = new PointMass(31.0, 23.0, 11.0);
        Assert.Equal(11.0, pointMass.Mass);
    }

    [Fact]
    public void MoveTest()
    {
        var pointMass = new PointMass(31.0, 23.0, 11.0);
        var force = new Vector(7, 13);
        pointMass.Force = force;

        pointMass.Move(3.0);
        Assert.Equal(31.0, pointMass.XPrev, 0.01);
        Assert.Equal(23.0, pointMass.YPrev, 0.01);
        Assert.Equal(36.727, pointMass.XPos, 0.01);
        Assert.Equal(33.636, pointMass.YPos, 0.01);
    }

    [StaFact]
    public void DrawTest()
    {
        var panel = new Canvas { Width = 100, Height = 100 };

        var pointMass = new PointMass(11.0, 13.0, 11.0);
        pointMass.Draw(panel, 2.0);

        var wpf = new ContentControl { Content = panel };
        WpfApprovals.Verify(wpf);
    }
}