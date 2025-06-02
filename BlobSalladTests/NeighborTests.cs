using BlobSallad;
using Xunit;

namespace BlobSalladTests;

public class NeighborTests
{
    private Neighbor _collision;
    private PointMass _pointMassA;
    private PointMass _pointMassB;

    [Fact]
    public void CtorNeighborTest()
    {
        var cxA = 41.0;
        var cyA = 43.0;
        var massA = 4.0;
        _pointMassA = new PointMass(cxA, cyA, massA);

        var cxB = 71.0;
        var cyB = 67.0;
        var massB = 1.0;
        _pointMassB = new PointMass(cxB, cyB, massB);

        // sum of the two radii
        var distance = 17.0;
        _collision = new Neighbor(_pointMassA, _pointMassB, distance);

        Assert.Equal(distance, _collision.Limit, 0.01);
    }
}