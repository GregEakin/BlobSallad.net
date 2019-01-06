using System.Windows.Controls;

namespace BlobSallad
{
    public class Gravity : Force
    {
        public Gravity(PointMass pointMassA, PointMass pointMassB) : base(pointMassA, pointMassB)
        {
        }

        public override void Scale(double scaleFactor)
        {
            throw new System.NotImplementedException();
        }

        public override void Sc(Environment env)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw(Canvas canvas, double scaleFactor)
        {
            throw new System.NotImplementedException();
        }
    }
}