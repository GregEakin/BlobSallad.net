using System.Windows.Controls;

namespace BlobSallad
{
    public class Collision : Force
    {
        private double _slSquared;
        private double _llSquared;

        public Collision(PointMass pointMassA, PointMass pointMassB, double shortLimit)
            : base(pointMassA, pointMassB)
        {
            ShortLimit = shortLimit;
            LongLimit = double.PositiveInfinity;
            _slSquared = ShortLimit * ShortLimit;
            _llSquared = LongLimit * LongLimit;
        }

        public double LongLimit { get; private set; }

        public double ShortLimit { get; private set; }

        public override void Scale(double scaleFactor)
        {
            ShortLimit *= scaleFactor;
            LongLimit *= scaleFactor;
            _slSquared = ShortLimit * ShortLimit;
            _llSquared = LongLimit * LongLimit;
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
            else if (dp > _llSquared)
            {
                var scaleFactor = _llSquared / (dp + _llSquared) - 0.5;
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