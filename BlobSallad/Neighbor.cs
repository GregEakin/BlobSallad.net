using System.Windows.Controls;

namespace BlobSallad
{
    public class Neighbor : Force
    {
        private double _slSquared;

        public Neighbor(PointMass pointMassA, PointMass pointMassB, double shortLimit)
            : base(pointMassA, pointMassB)
        {
            ShortLimit = shortLimit;
            _slSquared = ShortLimit * ShortLimit;
        }

        public double ShortLimit { get; private set; }

        public override void Scale(double scaleFactor)
        {
            ShortLimit *= scaleFactor;
            _slSquared = ShortLimit * ShortLimit;
        }

        public override void Sc(Environment env)
        {
            var delta = PointMassB.Pos - PointMassA.Pos;
            var dp = delta.DotProd(delta);
            if (dp < _slSquared)
            {
                var scaleFactor = _slSquared / (dp + _slSquared) - 0.5;
                delta.Scale(scaleFactor);
                PointMassA.Pos.Sub(delta);
                PointMassB.Pos.Add(delta);
            }
        }

        public override void Draw(Canvas canvas, double scaleFactor)
        {
        }
    }
}