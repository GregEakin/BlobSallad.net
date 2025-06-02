using System.Windows.Controls;

namespace BlobSallad;

public abstract class Connection
{
    protected Connection(PointMass pointMassA, PointMass pointMassB)
    {
        PointMassA = pointMassA;
        PointMassB = pointMassB;
    }

    public PointMass PointMassA { get; }

    public PointMass PointMassB { get; }

    public abstract void Scale(double scaleFactor);

    public abstract void Sc(Environment env);

    public abstract void Draw(Canvas canvas, double scaleFactor);
}