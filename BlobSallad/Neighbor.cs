using System.Windows.Controls;

namespace BlobSallad;

public class Neighbor : Connection
{
    private double _slSquared;

    public Neighbor(PointMass pointMassA, PointMass pointMassB, double limit)
        : base(pointMassA, pointMassB)
    {
        Limit = limit;
        _slSquared = limit * limit;
    }

    public double Limit { get; private set; }

    public override void Scale(double scaleFactor)
    {
        Limit *= scaleFactor;
        _slSquared = Limit * Limit;
    }

    public override void Sc(Environment env)
    {
        var delta = PointMassB.Pos - PointMassA.Pos;
        var distance = delta.DotProd(delta);
        if (distance >= _slSquared) return;
        var scaleFactor = _slSquared / (distance + _slSquared) - 0.5;
        delta.Scale(scaleFactor);
        PointMassA.Pos.Sub(delta);
        PointMassB.Pos.Add(delta);
    }

    public override void Draw(Canvas canvas, double scaleFactor)
    {
    }
}