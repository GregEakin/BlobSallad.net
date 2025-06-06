﻿using BlobSallad;
using Xunit;

namespace BlobSalladTests;

public class BoneTests
{
    private Bone _bone;
    private PointMass _pointMassA;
    private PointMass _pointMassB;

    [Fact]
    public void CtorBodyJointTest()
    {
        var cxA = 41.0;
        var cyA = 43.0;
        var massA = 4.0;
        _pointMassA = new PointMass(cxA, cyA, massA);

        var cxB = 71.0;
        var cyB = 67.0;
        var massB = 1.0;
        _pointMassB = new PointMass(cxB, cyB, massB);

        // The blob can change shape by plus or minus 5%
        var low = 0.95;
        var high = 1.05;
        _bone = new Bone(_pointMassA, _pointMassB, low, high);

        Assert.Equal(36.498, _bone.ShortLimit, 0.01);
        Assert.Equal(40.340, _bone.LongLimit, 0.01);
    }

    [Fact]
    public void ScaleTest()
    {
        double cxA = 41;
        double cyA = 43;
        var massA = 4.0;
        _pointMassA = new PointMass(cxA, cyA, massA);

        var cxB = 71.0;
        var cyB = 67.0;
        var massB = 1.0;
        _pointMassB = new PointMass(cxB, cyB, massB);

        var low = 0.95;
        var high = 1.05;
        _bone = new Bone(_pointMassA, _pointMassB, low, high);
        _bone.Scale(10.0);

        Assert.Equal(364.978, _bone.ShortLimit, 0.01);
        Assert.Equal(403.397, _bone.LongLimit, 0.01);
    }
}